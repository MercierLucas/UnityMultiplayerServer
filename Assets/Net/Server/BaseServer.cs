using Unity.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Networking.Transport;

using System.Linq;

namespace Server
{
    public class BaseServer : MonoBehaviour
    {
        #region CONSTS
        private const int PORT = 5522;
        private const int MAX_CLIENTS = 4;
        private const int TICKS_PER_SECONDS = 30; 
        
        #endregion

        private float updateTimer;
        private float lastUpdate;
        public NetworkDriver driver;
        private NativeList<NetworkConnection> connections;
        protected Dictionary<int, NetworkConnection> players;
        private NetMessageHandlerManager netMessageHandlerManager;
        private MessageSender messageSender;
        public EntityManager entityManager;

        //#if UNITY_EDITOR
        private void Start(){Init();}
        private void Update(){UpdateServer();}
        private void OnDestroy(){Shutdown();}
        //#endif


        public virtual void Init()
        {
            updateTimer = 1/TICKS_PER_SECONDS;
            lastUpdate = Time.time;

            driver =  NetworkDriver.Create();
            NetworkEndPoint endPoint = NetworkEndPoint.AnyIpv4;     // Allow anybody to connect to us
            endPoint.Port = PORT;

            netMessageHandlerManager = new NetMessageHandlerManager(this);
            messageSender = new MessageSender(this);
            entityManager = new EntityManager();
            players = new Dictionary<int, NetworkConnection>();

            if(driver.Bind(endPoint) != 0)
            {
                Logs.Error($"Error while binding to {PORT}");
                Shutdown();
            }
            else
            {
                driver.Listen();
                Logs.Print($"Server running at port {PORT} with {TICKS_PER_SECONDS} ticks/s");
            }

            connections = new NativeList<NetworkConnection>(MAX_CLIENTS, Allocator.Persistent);
        }

        public virtual void UpdateServer()
        {
            if(Time.time - lastUpdate > updateTimer)
            {
                driver.ScheduleUpdate().Complete();         // job is complete so unlock thread
                CleanupConnections();
                AcceptNewConnections();
                UpdateMessagePump();

                messageSender.PropagateEntities(entityManager.Entities);

                lastUpdate = Time.time;
            }
        }

        private void CleanupConnections()
        {
            // Use for clients drop connection without notifying it (atlf4, game or network crash etc.)
            for (int i = 0; i < connections.Length; i++)
            {
                if(!connections[i].IsCreated)
                {
                    Logs.Print($"Disconnect {i}");
                    connections.RemoveAtSwapBack(i);
                    messageSender.NotifyDisconnect(i);
                    --i;
                }
            }
        }

        private void AcceptNewConnections()
        {
            NetworkConnection c;
            while((c = driver.Accept()) != default(NetworkConnection))
            {
                connections.Add(c);
                NewClientConnect(c);
                //Logs.Print($"New connection arrived from client");
            }
        }

        protected virtual void UpdateMessagePump()
        {
            DataStreamReader stream;
            for (int i = 0; i < connections.Length; i++)
            {
                NetworkEvent.Type cmd;
                while((cmd = driver.PopEventForConnection(connections[i], out stream)) != NetworkEvent.Type.Empty)
                {
                    if(cmd == NetworkEvent.Type.Data)
                    {
                        netMessageHandlerManager.OnMessageReceived(stream);
                    }
                    else if(cmd == NetworkEvent.Type.Disconnect)
                    {
                        Logs.Print($"A client disconnected");
                        connections[i] = default(NetworkConnection);
                        messageSender.NotifyDisconnect(i);  
                    }
                }
            }
        }

        protected virtual void NewClientConnect(NetworkConnection connection)
        {
            int uid = GetNextUID();
            players.Add(uid,connection);
            messageSender.SendToClient(new NetMessage_JoinServer(uid),connection);
            messageSender.SendFullEntities(entityManager.Entities, connection);
            Logs.Print($"New client joined with id {uid}");
        }

        public int GetNextUID()
        {
            if (entityManager.Entities.Count == 0 && players.Count == 0)
                return 0;

            if (entityManager.Entities.Count > 0 && players.Count == 0)
                return entityManager.Entities.Keys.Max() + 1;

            return Mathf.Max((int)players.Keys.Max(), (int)entityManager.Entities.Keys.Max())+1;
        }

        public virtual void Shutdown()
        {
            driver.Dispose();
            connections.Dispose();
        }

        #region PROPERTIES
        public NativeList<NetworkConnection> Connections
        {
            get
            {
                return connections;
            }
        }
        #endregion
    }
}
