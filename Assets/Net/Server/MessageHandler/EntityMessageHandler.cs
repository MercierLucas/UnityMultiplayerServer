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
    }
}
