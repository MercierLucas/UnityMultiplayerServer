using Unity.Networking.Transport;
using System;
using System.Collections;
using System.Collections.Generic;

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

        public virtual void Broadcast(NetMessage message)
        {
            // Todo : implement restrictions based on distance?

            for (int i = 0; i < server.Connections.Length; i++)
            {
                if(server.Connections[i].IsCreated)
                {
                    SendToClient(message, server.Connections[i]);
                }
            }
        }

        public virtual void PropagateEntities(Dictionary<int, Entity> entities)
        {

            foreach (var item in entities)
            {
                NetMessage_Entity message = new NetMessage_Entity(item.Value);
                Broadcast(message);
            }
        }
    }
}
