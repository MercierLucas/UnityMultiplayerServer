using Unity.Networking.Transport;
using UnityEngine;

namespace Client
{
    public class EntityMessageHandler
    {
        private BaseClient client;
        private LogEventChannelSO eventChannel;

        public EntityMessageHandler(BaseClient client)
        {
            this.client = client;
        }

        public EntityMessageHandler(BaseClient client, LogEventChannelSO eventChannel)
        {
            this.client = client;
            this.eventChannel = eventChannel;
        }

        public void PlayerAppears(DataStreamReader reader)
        {
            NetMessage_Entity message = new NetMessage_Entity(reader);
            message.Entity.ShowDebug();
            client.entitiesManager.TryAddEntity(message.Entity);
        }

        public void PlayerJoined(DataStreamReader reader)
        {
            NetMessage_JoinServer joinServer = new NetMessage_JoinServer(reader);
            client.entitiesManager.PlayerUID = joinServer.UID;
            client.playerManager.Setup(joinServer.UID);
            eventChannel?.Raise($"Welcome to the game, your UID is {joinServer.UID}", EventLogType.Warning);
        }

        public void PlayerLeaved(DataStreamReader reader)
        {
            NetMessage_Disconnect leavedMessage = new NetMessage_Disconnect(reader);
            eventChannel?.Raise($"Player {leavedMessage.UID} disconnected", EventLogType.Warning);
            Debug.Log("Player leaved");
        }

    }
}