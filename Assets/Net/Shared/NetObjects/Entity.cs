using UnityEngine;
using System;
using Unity.Networking.Transport;

[Flags]
public enum EntityFlag
{
    none=0,
    meta=1,
    position=2,
    visual=4,

    all=15,
}

public enum EntityType
{
    player=0,
    npc=1
}


public class Entity
{
    // META
    public int UID {get; private set;}
    public EntityType Type {get; private set;}
    public EntityFlag Flags {get;set;}
    public EntityFlag Dirty {get;set;}
    public String Name {get; set;}

    // Visual
    public byte MeshID {get;set;}
    public byte MaterialID {get;set;}

    // Position
    public Vector3 Position {get; set;}
    public Quaternion Rotation {get; set;}

    // Previous values
    private byte oldMeshID;
    private byte oldMaterialID;
    private Vector3 oldPosition;
    private Quaternion oldRotation;

    public Entity(int uid, EntityType type)
    {
        UID = uid;
        Type = type;
        MeshID = 0;
        MaterialID = 0;

        Position = Vector3.zero;
        Rotation = Quaternion.identity;

        oldPosition = Position;
        oldRotation = Rotation;
        oldMeshID = MeshID;
        oldMaterialID = MaterialID;
    }

    public virtual void UpdateDirty(Entity entity)
    {
        EntityFlag dirty = entity.Dirty;

        if(dirty.HasFlag(EntityFlag.position))
        {
            Position = entity.Position;
            Rotation = entity.Rotation;
        }
        if(dirty.HasFlag(EntityFlag.visual))
        {
            MeshID = entity.MeshID;
            MaterialID = entity.MaterialID;
        }
    }

    public virtual void Update()
    {
        Dirty = EntityFlag.none;        // reset flag

        if (Flags.HasFlag(EntityFlag.position)) UpdatePosition();
        if (Flags.HasFlag(EntityFlag.visual)) UpdateVisual();
    }

    public virtual void UpdatePosition()
    {
        if(oldPosition != Position || oldRotation != Rotation)
        {
            oldPosition = Position;
            oldRotation = Rotation;

            Dirty |= EntityFlag.position;
        }
    }

    public virtual void UpdateVisual()
    {
        if(oldMaterialID != MaterialID || oldMeshID != MeshID)
        {
            oldMaterialID = MaterialID;
            oldMeshID = MeshID;

            Dirty |= EntityFlag.visual;
        }
    }

}