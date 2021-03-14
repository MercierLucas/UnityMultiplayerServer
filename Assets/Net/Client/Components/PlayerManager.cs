using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("Meta")]
    public int UID;

    [Header("Flags")]
    public EntityFlag Flags;

    [Header("Visuals")]
    public byte MeshID;
    public byte MaterialID;

    public Entity entity {get; private set;}


    private void Start()
    {
        //entity = new Entity();
    }

    public void SetupEntity(int uid)
    {
        UID = uid;
        entity = new Entity(uid, EntityType.player);
        entity.Flags = Flags;
        entity.MeshID = MeshID;
        entity.MaterialID = MaterialID;

        entity.Dirty = EntityFlag.all;
    }

}