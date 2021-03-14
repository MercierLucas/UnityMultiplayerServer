using Unity.Networking.Transport;

namespace Server
{
    public class EntityMessageHandler
    {
        private BaseServer server;
        public EntityMessageHandler(BaseServer server)
        {
            this.server = server;
        }

        public void OnPlayerJoined(DataStreamReader reader)
        {
            Logs.Print("New client");
        }

        public void ReceiveEntity(DataStreamReader reader)
        {
            NetMessage_Entity message = new NetMessage_Entity(reader);
            server.entityManager.ReceiveEntity(message.Entity);
        }
    }
}
