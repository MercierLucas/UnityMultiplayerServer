using Unity.Networking.Transport;
using System.Collections;
using System.Collections.Generic;
using Server.Logs;
using System;

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
        
        netMessagesActions.Add(OpCode.CHAT_MESSAGE,ChatMessageHandler.Handle);
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