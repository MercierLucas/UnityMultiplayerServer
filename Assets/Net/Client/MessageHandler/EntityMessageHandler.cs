using Unity.Networking.Transport;
using UnityEngine;

namespace Client
{
    public class EntityMessageHandler
    {
        private BaseClient client;

        public EntityMessageHandler(BaseClient client)
        {
            this.client = client;
        }

        public void PlayerAppears(DataStreamReader reader)
        {
            NetMessage_Entity message = new NetMessage_Entity(reader);
            client.entitiesManager.TryAddEntity(message.Entity);
        }

        public void PlayerJoined(DataStreamReader reader)
        {
            NetMessage_JoinServer joinServer = new NetMessage_JoinServer(reader);
            client.playerManager.SetupEntity(joinServer.UID);
        }

    }
}