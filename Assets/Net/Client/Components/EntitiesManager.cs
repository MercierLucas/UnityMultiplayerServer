using UnityEngine;
using System.Collections.Generic;

public class EntitiesManager : MonoBehaviour
{
    [SerializeField] private Dictionary<int, Entity> entities;
    [SerializeField] private Dictionary<int, GameObject> entitiesPrefabs;
    [SerializeField] private ResourceIndex resourceIndex;

    private void Start()
    {
        entities = new Dictionary<int, Entity>();
        entitiesPrefabs = new Dictionary<int, GameObject>();
    }

    public void TryAddEntity(Entity entity)
    {
        if(!entities.ContainsKey(entity.UID))
        {
            entities.Add(entity.UID, entity);
            GameObject go = CreateEntityGO(entity);
            entitiesPrefabs.Add(entity.UID, go);
        }
    }

    public GameObject CreateEntityGO(Entity entity)
    {
        GameObject mesh = resourceIndex.GetMesh(entity.MeshID);
        Material mat = resourceIndex.GetMaterial(entity.MaterialID);

        GameObject go = GameObject.Instantiate(
            mesh,
            entity.Position,
            entity.Rotation
            );

        go.GetComponent<Renderer>().material = mat;
        return go;
    }

}