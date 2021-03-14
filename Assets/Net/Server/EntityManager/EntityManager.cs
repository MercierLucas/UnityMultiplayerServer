using System.Collections.Generic;

namespace Server
{
    public class EntityManager
    {
        private Dictionary<int, Entity> entities;

        public EntityManager()
        {
            entities = new Dictionary<int, Entity>();
            AddNPC();
        }

        private void AddNPC()
        {
            Entity npc = new Entity(0,EntityType.npc);
            npc.MeshID = 2;
            npc.MaterialID = 2;
            //npc.Position.x = 1f;

            ReceiveEntity(npc);
        }

        public void ReceiveEntity(Entity entity)
        {
            if(entities.ContainsKey(entity.UID))
            {
                entities[entity.UID] = entity;
            }
            else
            {
                entity.Dirty = EntityFlag.all; // First time we see this entity, need to have all info about it
                entities.Add(entity.UID, entity);
            }
        }

        public Dictionary<int, Entity> Entities
        {
            get
            {
                return entities;
            }
        }
    }
}



