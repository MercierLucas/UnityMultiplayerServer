using Unity.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Networking.Transport;

namespace Client
{
    public class BaseClient : MonoBehaviour
    {
        public NetworkDriver driver;
        protected NetworkConnection serverConnection;
        protected NetMessageHandlerManager messageHandler;
        [SerializeField] public EntitiesManager entitiesManager {get; private set;}
        [SerializeField] public PlayerManager playerManager {get; private set;}

        //#if UNITY_EDITOR
        private void Start(){Init();}
        private void Update(){UpdateClient();}
        private void OnDestroy(){Shutdown();}
        //#endif

        public virtual void Init()
        {
            serverConnection = default(NetworkConnection);
            messageHandler = new NetMessageHandlerManager(this);
            entitiesManager = GetComponent<EntitiesManager>();
            playerManager = GetComponent<PlayerManager>();
            driver =  NetworkDriver.Create();
            NetworkEndPoint endPoint = NetworkEndPoint.LoopbackIpv4;     // LoopbackIpv4 == localhost
            endPoint.Port = 5522;
            serverConnection = driver.Connect(endPoint);
        }

        public virtual void UpdateClient()
        {
            driver.ScheduleUpdate().Complete();         // job is complete so unlock thread
            CheckAlive();
            UpdateMessagePump();
            SendOwnEntity();
        }
        public virtual void Shutdown()
        {
            driver.Dispose();
        }
        private void CheckAlive()
        {
            if(!serverConnection.IsCreated)
            {
                Debug.Log("Something went wront, lost connection to server");
            }
        }
        
        protected virtual void UpdateMessagePump()
        {
            DataStreamReader stream;

            NetworkEvent.Type cmd;
            while((cmd = serverConnection.PopEvent(driver, out stream)) != NetworkEvent.Type.Empty)
            {
                if(cmd == NetworkEvent.Type.Connect)
                {
                    Debug.Log("Connected to server");
                    //SendToServer(new NetMessage_JoinServer(0));
                }
                else if(cmd == NetworkEvent.Type.Data)
                {
                    messageHandler.OnMessageReceived(stream);
                }
                else if(cmd == NetworkEvent.Type.Disconnect)
                {
                    Debug.Log($"You've been disconnected from the server");
                }
            }
            
        }

        public virtual void SendToServer(NetMessage message)
        {
            DataStreamWriter writer;
            driver.BeginSend(default(NetworkPipeline), serverConnection, out writer);
            message.Serialize(ref writer);
            driver.EndSend(writer);
        }

        private void SendOwnEntity()
        {
            if(playerManager.entity != null)
            {
                SendToServer(new NetMessage_Entity(playerManager.entity));
            }
        }
    }

}
