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

        public NetMessageHandlerManager()
        {
            SetupHandler();
        }


        private void SetupHandler()
        {
            netMessagesActions = new Dictionary<OpCode, NetMessageHandler>();
            
            netMessagesActions.Add(OpCode.ENTITY, EntityMessageHandler.PlayerAppears);
            netMessagesActions.Add(OpCode.PLAYER_JOIN, EntityMessageHandler.PlayerJoined);
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