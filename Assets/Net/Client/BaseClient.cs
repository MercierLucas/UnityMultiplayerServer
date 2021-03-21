using Unity.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Networking.Transport;

namespace Client
{
    public class BaseClient : MonoBehaviour
    {
        private bool clientStarted;
        public NetworkDriver driver;
        protected NetworkConnection serverConnection;
        protected NetMessageHandlerManager messageHandler;
        [SerializeField] public EntitiesManager entitiesManager {get; private set;}
        [SerializeField] public PlayerManager playerManager {get; private set;}
        private MessageSender messageSender;

        [Header("Events")]
        [SerializeField] private LogEventChannelSO eventChannel;

        //#if UNITY_EDITOR
        //private void Start(){Init();}
        private void Update(){UpdateClient();}
        private void LateUpdate(){ if(clientStarted) messageSender.SendOwnEntity(playerManager);}
        private void OnDestroy(){Shutdown();}
        #if UNITY_EDITOR
        private void OnApplicationQuit() { Shutdown();}
        #endif

        public virtual void Init()
        {
            serverConnection = default(NetworkConnection);
            messageHandler = new NetMessageHandlerManager(this, eventChannel);
            entitiesManager = GetComponent<EntitiesManager>();
            playerManager = GetComponent<PlayerManager>();
            driver =  NetworkDriver.Create();
            NetworkEndPoint endPoint = NetworkEndPoint.LoopbackIpv4;     // LoopbackIpv4 == localhost
            endPoint.Port = 5522;
            serverConnection = driver.Connect(endPoint);
            messageSender = new MessageSender(driver, serverConnection);
            clientStarted = true;
        }

        public virtual void UpdateClient()
        {
            if(!clientStarted) return;

            driver.ScheduleUpdate().Complete();         // job is complete so unlock thread
            CheckAlive();
            UpdateMessagePump();
            playerManager?.UpdateEntity();
        }
        public virtual void Shutdown()
        {
            eventChannel?.Raise("Disconnecting", EventLogType.Error);
            Debug.Log("Disconnect");

            if(driver.IsCreated)
            {
                if(serverConnection.IsCreated)
                {
                    serverConnection.Disconnect(driver);
                }
                driver.Dispose();
            }
                
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
                    eventChannel?.Raise("Connected to server", EventLogType.Success);
                }
                else if(cmd == NetworkEvent.Type.Data)
                {
                    messageHandler.OnMessageReceived(stream);
                }
                else if(cmd == NetworkEvent.Type.Disconnect)
                {
                    eventChannel?.Raise("You've been disconnected from the server", EventLogType.Error);
                }
            }
        }
    }

}
