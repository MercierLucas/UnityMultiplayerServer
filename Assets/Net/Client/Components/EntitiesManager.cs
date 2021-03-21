using UnityEngine;
using System.Collections.Generic;

public class EntitiesManager : MonoBehaviour
{
    [SerializeField] private Dictionary<int, Entity> entities;
    [SerializeField] private Dictionary<int, GameObject> entitiesPrefabs;
    [SerializeField] private LogEventChannelSO eventChannel;

    public int PlayerUID;

    private void Start()
    {
        entities = new Dictionary<int, Entity>();
        entitiesPrefabs = new Dictionary<int, GameObject>();
    }

    public void TryAddEntity(Entity entity)
    {
        if(entity.UID == PlayerUID) return;
        
        if(!entities.ContainsKey(entity.UID))
        {
            entities.Add(entity.UID, entity);
            GameObject go = CreateEntityGO(entity);
            SetupEntity(go, entity);
            entitiesPrefabs.Add(entity.UID, go);

            if(entity.Type.Equals(EntityType.player))
            {
                eventChannel?.Raise($"Player {entity.UID} joined", EventLogType.Success);
            }
        }
        else
        {
            EntityManager manager = entitiesPrefabs[entity.UID].GetComponent<EntityManager>();
            if(manager != null)
            {
                manager.UpdateEntity(entity);
            }
        }
    }

    public void SetupEntity(GameObject go, Entity entity)
    {
        EntityManager manager = (EntityManager)go.AddComponent(typeof(EntityManager));
        manager.Setup(entity);
        go.name = $"Entity_{entity.UID}";
    }

    public GameObject CreateEntityGO(Entity entity)
    {
        GameObject mesh = ResourcesManager.GetMesh(entity.MeshID.ToString());
        Material mat = ResourcesManager.GetMaterial(entity.MaterialID.ToString());

        GameObject go = GameObject.Instantiate(
            mesh,
            entity.Position,
            entity.Rotation
            );

        if(go.GetComponent<Renderer>() != null)
            go.GetComponent<Renderer>().material = mat;

        return go;
    }

}