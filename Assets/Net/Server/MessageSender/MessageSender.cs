using Unity.Networking.Transport;

namespace Server
{
    public class MessageSender
    {
        public BaseServer server;

        public MessageSender(BaseServer server)
        {
            this.server = server;
        }

        public virtual void SendToClient(NetMessage message, NetworkConnection connection)
        {
            DataStreamWriter writer = new DataStreamWriter();
            server.driver.BeginSend(default(NetworkPipeline), connection, out writer);
            message.Serialize(ref writer);
            server.driver.EndSend(writer);
        }
    }
}
