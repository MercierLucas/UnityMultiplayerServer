using Unity.Networking.Transport;

public class NetMessage_JoinServer : NetMessage
{
    public int UID {get;set;}

    public NetMessage_JoinServer(int uid)
    {
        Code = OpCode.PLAYER_JOIN;
        UID = uid;
    }

    public NetMessage_JoinServer(DataStreamReader reader):base(reader)
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