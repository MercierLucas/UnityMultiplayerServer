using Unity.Networking.Transport;

public class NetMessage_Disconnect : NetMessage
{
    public int UID {get;set;}


    public NetMessage_Disconnect (int uid)
    {
        Code = OpCode.PLAYER_LEAVE;
        UID = uid;
    }

    public NetMessage_Disconnect(DataStreamReader reader) : base(reader)
    {
        Deserialize(reader);
    }

    public override void Deserialize(DataStreamReader reader)
    {
        UID = reader.ReadInt();
    }

    public override void Serialize(ref DataStreamWriter writer)
    {
        writer.WriteByte((byte)Code);
        writer.WriteInt(UID);
    }
}