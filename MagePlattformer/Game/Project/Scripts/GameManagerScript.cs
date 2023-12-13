using System.Numerics;
using System.Collections.Generic;
using Raylib_cs;
using Engine;
using Physics;
using Animation;

namespace Engine
{
    public class GameManagerScript : Component, IScript
    {
        Dictionary<int, Type> entitysInLevel = new()
        {
            {1, typeof(Block)},
            {2, typeof(JumpPad)},
            { 3, typeof(WalkEnemy)},
            { 4, typeof(JumpingEnemy)},
            {5, typeof(RandomEnemy)}
        };

        List<GameEntity> levelEntities = new();

        // Create and initialize a 15x19 grid with ones at the borders and zeros elsewhere
        int[,] _level1 = new int[,]
        {
            {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 1},
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 1},
            {1, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 4, 3, 1, 1, 0, 2, 1},
            {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1}
        };
        public override void Start()
        {
            LoadingManager.SaveLevel("Level1", _level1);

            int[,] level1 = LoadingManager.LoadLevel("Level1");
            SpawLevel(level1);
        }
        void SpawLevel(int[,] level)
        {
            for (int x = 0; x < level.GetLength(0); x++)
            {
                for (int y = 0; y < level.GetLength(1); y++)
                {
                    if (level[x, y] > 0)
                    {
                        GameEntity entity = GetEntityInstance(level[x, y]);
                        levelEntities.Add(entity);

                        Vector2 spawPos = new Vector2(y - level.GetLength(1) / 2, x - level.GetLength(0) / 2);

                        EntityManager.SpawnEntity(entity, spawPos);
                    }
                }
            }
        }
        void ClearLevel()
        {
            foreach (GameEntity entity in levelEntities)
            {
                EntityManager.DestroyEntity(entity);
            }
            levelEntities.Clear();
        }
        // Method to spawn GameEntity based on int key
        public GameEntity GetEntityInstance(int key)
        {
            // Check if the key exists in the dictionary
            if (entitysInLevel.ContainsKey(key))
            {
                Type entityType = entitysInLevel[key];
                // Use reflection to create an instance of the specified type
                GameEntity newEntity = (GameEntity)Activator.CreateInstance(entityType);

                if (newEntity == null) { System.Console.WriteLine($"Error, the number {key} is wrong"); }
                // Return the spawned GameEntity
                return newEntity;
            }
            else
            {
                Console.WriteLine($"Entity: {key} is not");
                return null;
            }
        }
    }
}