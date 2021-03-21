using Unity.Networking.Transport;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Client
{
    public class NetMessageHandlerManager
    {
        private delegate void NetMessageHandler(DataStreamReader reader);

        private Dictionary<OpCode,NetMessageHandler> netMessagesActions;

        private LogEventChannelSO eventChannel;

        public NetMessageHandlerManager(BaseClient client)
        {
            SetupHandler(client);
            eventChannel = null;
        }

        public NetMessageHandlerManager(BaseClient client, LogEventChannelSO eventChannel)
        {
            SetupHandler(client);
            this.eventChannel = eventChannel;
        }


        private void SetupHandler(BaseClient client)
        {
            netMessagesActions = new Dictionary<OpCode, NetMessageHandler>();

            EntityMessageHandler entityMessageHandler = new EntityMessageHandler(client, eventChannel);
            
            netMessagesActions.Add(OpCode.ENTITY, entityMessageHandler.PlayerAppears);
            netMessagesActions.Add(OpCode.PLAYER_JOIN, entityMessageHandler.PlayerJoined);
            netMessagesActions.Add(OpCode.PLAYER_LEAVE, entityMessageHandler.PlayerLeaved);
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
                Debug.Log($"Received unknown OPCODE: {code}");
            }
        }
    }
}