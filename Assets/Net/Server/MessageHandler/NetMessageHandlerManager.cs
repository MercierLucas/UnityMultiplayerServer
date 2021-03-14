using Unity.Networking.Transport;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Server
{
    public class NetMessageHandlerManager
    {
        private BaseServer server;
        private delegate void NetMessageHandler(DataStreamReader reader);
        private Dictionary<OpCode,NetMessageHandler> netMessagesActions;

        public NetMessageHandlerManager(BaseServer server)
        {
            this.server = server;
            SetupHandler();
        }

        private void SetupHandler()
        {
            EntityMessageHandler entityMessageHandler = new EntityMessageHandler(server);
            netMessagesActions = new Dictionary<OpCode, NetMessageHandler>();
            
            netMessagesActions.Add(OpCode.CHAT_MESSAGE,ChatMessageHandler.Handle);
            netMessagesActions.Add(OpCode.PLAYER_JOIN,entityMessageHandler.OnPlayerJoined);
            netMessagesActions.Add(OpCode.ENTITY,entityMessageHandler.ReceiveEntity);
        }

        public void OnMessageReceived(DataStreamReader reader)
        {
            uint opCodeValue = reader.ReadByte();
            OpCode code = (OpCode)Enum.Parse(typeof(OpCode), opCodeValue.ToString());

            if(netMessagesActions.ContainsKey(code))
            {
                netMessagesActions[code]?.Invoke(reader);
            }
            else
            {
                Logs.Error($"Received unknown OPCODE: {code}");
            }
        }
    }
}
