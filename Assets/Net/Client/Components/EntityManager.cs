using UnityEngine;

public class EntityManager : MonoBehaviour
{
    [SerializeField] private Entity entity;
    private void Start()
    {
        
    }

    public void Setup(Entity entity)
    {
        this.entity = entity;
    }

    public void UpdateEntity(Entity entity)
    {
        this.entity.UpdateDirty(entity);
        if(entity.Dirty.Equals(EntityFlag.none))
        {
            Debug.Log($"Updating {entity.UID} position : {entity.Dirty}");
        }
        transform.position = entity.Position;
        transform.rotation = entity.Rotation;
    }
}