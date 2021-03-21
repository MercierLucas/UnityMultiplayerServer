using UnityEngine;
using Unity.Networking.Transport;


public class NetMessage_Entity : NetMessage
{
    /* Format
    int UID
    int Dirty

    */
    
    public Entity Entity{get; private set;}
    
    public NetMessage_Entity(DataStreamReader reader) : base(reader)
    {
        Deserialize(reader);
    }

    public NetMessage_Entity(Entity entity)
    {
        Code = OpCode.ENTITY;
        Entity = entity;
    }

    #region Serializer
    public override void Serialize(ref DataStreamWriter writer)
    {
        writer.WriteByte((byte)Code);

        writer.WriteInt(Entity.UID);
        writer.WriteByte((byte)Entity.Type);
        writer.WriteByte((byte)Entity.Dirty);

        if(Entity.Dirty.HasFlag(EntityFlag.position)) SerializePosition(ref writer);
        if(Entity.Dirty.HasFlag(EntityFlag.visual)) SerializeVisual(ref writer);
    }

    public virtual void SerializePosition(ref DataStreamWriter writer)
    {
        // Position
        writer.WriteFloat(Entity.Position.x);
        writer.WriteFloat(Entity.Position.y);
        writer.WriteFloat(Entity.Position.z);

        // Rotation
        writer.WriteFloat(Entity.Rotation.x);
        writer.WriteFloat(Entity.Rotation.y);
        writer.WriteFloat(Entity.Rotation.z);
        writer.WriteFloat(Entity.Rotation.w);
    }

    public virtual void SerializeVisual(ref DataStreamWriter writer)
    {
        writer.WriteFixedString32(Entity.MeshID);
        writer.WriteFixedString32(Entity.MaterialID);
    }

    #endregion
    #region Deserializer
    public override void Deserialize(DataStreamReader reader)
    {
        int uid = reader.ReadInt();
        EntityType type = (EntityType)reader.ReadByte();
        Entity = new Entity(uid, type);

        Entity.Dirty = (EntityFlag)reader.ReadByte();

        if(Entity.Dirty.HasFlag(EntityFlag.position)) DeserializePosition(ref reader);
        if(Entity.Dirty.HasFlag(EntityFlag.visual)) DeserializeVisual(ref reader);
    }

    public void DeserializePosition(ref DataStreamReader reader)
    {
        float x,y,z,w;
        x = reader.ReadFloat();
        y = reader.ReadFloat();
        z = reader.ReadFloat();

        Entity.Position = new Vector3(x,y,z);

        x = reader.ReadFloat();
        y = reader.ReadFloat();
        z = reader.ReadFloat();
        w = reader.ReadFloat();

        Entity.Rotation = new Quaternion(x,y,z,w);
    }

    public void DeserializeVisual(ref DataStreamReader reader)
    {
        Entity.MeshID = reader.ReadFixedString32();
        Entity.MaterialID = reader.ReadFixedString32();
    }

    #endregion
}