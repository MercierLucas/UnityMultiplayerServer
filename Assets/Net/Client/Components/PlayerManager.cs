using UnityEngine;
using Unity.Collections;

public class PlayerManager : MonoBehaviour
{
    [Header("Meta")]
    public int UID;

    [Header("Flags")]
    public EntityFlag Flags;

    [Header("Visuals")]
    public string MeshID;
    public string MaterialID;

    public Entity entity; //{get; private set;}
    private GameObject player;

    [Header("Resources")]
    public GameObject prefab;

    [Header("Events")]
    [SerializeField] private GameObjectEventChannelSO goEventChannel;

    private void Start()
    {
        //entity = new Entity();
    }

    public void Setup(int uid)
    {
        UID = uid;
        entity = new Entity(uid, EntityType.player);
        entity.Flags = EntityFlag.all;
        entity.MeshID = MeshID;
        entity.MaterialID = MaterialID;
        entity.Dirty = EntityFlag.all;

        player = Instantiate(prefab, entity.Position, entity.Rotation);
        GameObject model = CreateEntityGO();
        model.transform.parent = player.transform;

        goEventChannel?.Raise(model);
    }

    private GameObject CreateEntityGO()
    {
        GameObject mesh = ResourcesManager.GetMesh(MeshID);
        Material mat = ResourcesManager.GetMaterial(MaterialID);

        GameObject go = GameObject.Instantiate(mesh, Vector3.zero, Quaternion.identity);

        if(go.GetComponent<Renderer>() != null)
            go.GetComponent<Renderer>().material = mat;

        return go;
    }

    public void UpdateEntity()
    {
        if(player == null) return;

        entity.Position = player.transform.position;
        entity.Rotation = player.transform.rotation;

        entity.Update();
    }

}