using System.Collections.Generic;
using System.Numerics;
using CoreEngine;
using Engine;

namespace Engine
{
    //handles all entities for the components
    public static class EntityManager
    {
        public static void SpawnEntity(GameEntity entity) { SpawnEntity(entity, Vector2.Zero); }
        public static void SpawnEntity(GameEntity entity, Vector2 position) { SpawnEntity(entity, position, Vector2.One); }
        public static void SpawnEntity(GameEntity entity, Vector2 position, Vector2 size) { SpawnEntity(entity, position, size, null); }
        public static void SpawnEntity(GameEntity entity, Vector2 position, Vector2 size, GameEntity parent)
        {
            entity.transform.position = position;
            entity.transform.size = size;

            if (parent != null)
            {
                entity.parent = parent;
            }
            else
            {
                entity.parent = Core.currentScene;
            }
            entity.parent.children.Add(entity);

            entity.OnInnit();
            foreach (Component component in entity.components)
            {
                component.Start();
            }
            Core.entitiesToAdd.Add(entity);
        }

        public static void DestroyEntity(GameEntity entity)
        {
            foreach (Component component in entity.components)
            {
                component.OnDestroy();
            }
            foreach (GameEntity child in entity.children)
            {
                DestroyEntity(child);
            }
            Core.entitiesToRemove.Add(entity);
        }

    }
}