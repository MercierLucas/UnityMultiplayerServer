using Unity.Networking.Transport;

public class NetMessage
{
    // When inheriting respect classname: NetMessage_XX
    // for player position: NetMessage_PlayerPosition

    // 0 - 8 OPCODE
    // 8 - 256 STRING MESSAGE

    protected DataStreamReader reader;

    // Two constructors
    // 1 For creating an object before sending it
    // 2 When receiving it and recreating it with reader
    
    public NetMessage(){}       
    public NetMessage(DataStreamReader reader)
    {
        this.reader = reader;
    }

    public OpCode Code {get;set;}

    public virtual void Serialize(ref DataStreamWriter writer){}

    public virtual void Deserialize(DataStreamReader reader){}
}
