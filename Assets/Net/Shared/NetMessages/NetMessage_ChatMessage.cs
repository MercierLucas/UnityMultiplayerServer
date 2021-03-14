using Unity.Networking.Transport;

public class NetMessage_ChatMessage : NetMessage
{
    // 0 - 8 OPCODE
    // 8 - 256 STRING MESSAGE
    // TODO: Change fix length?
    public string MessageContent {get; private set;}

    public NetMessage_ChatMessage(string message)
    {
        Code = OpCode.CHAT_MESSAGE;
        MessageContent = message;
    }

    public NetMessage_ChatMessage(DataStreamReader reader) : base(reader)
    {
        Deserialize(reader);
    }

    public override void Serialize(ref DataStreamWriter writer)
    {
        writer.WriteByte((byte)Code);
        writer.WriteFixedString512(MessageContent);             // fixe byte lenght
    }

    public override void Deserialize(DataStreamReader reader)
    {
       MessageContent = reader.ReadFixedString512().ToString();
    }
}