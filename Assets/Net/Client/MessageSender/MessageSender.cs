using Unity.Networking.Transport;

namespace Client
{
    public class MessageSender 
    {
        private NetworkDriver driver;
        private NetworkConnection server;


        public MessageSender(NetworkDriver driver, NetworkConnection connection)
        {
            this.driver = driver;
            this.server = connection;
        }

        public void SendToServer(NetMessage message)
        {
            DataStreamWriter writer;
            driver.BeginSend(default(NetworkPipeline), server, out writer);
            message.Serialize(ref writer);
            driver.EndSend(writer);
        }

        public void SendOwnEntity(PlayerManager manager)
        {
            if(manager.entity != null)
            {
                if(!manager.entity.Dirty.Equals(EntityFlag.none))
                {
                    SendToServer(new NetMessage_Entity(manager.entity));
                }
            }
        }
    }
}
