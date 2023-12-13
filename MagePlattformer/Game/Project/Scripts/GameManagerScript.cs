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

         Dictionary<int, string> levels = new()
        {
            {1, "Level1"},
            {2, "Level2"},
            {3, "Level3"}
        };

        List<GameEntity> levelEntities = new();

        // Create and initialize a level grid
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
        //WIP
        static List<Collider> GenerateColliders(int[,] grid)
        {
            //skapar listan av nya colliders
            List<Collider> colliders = new List<Collider>();
            int rows = grid.GetLength(0);
            int cols = grid.GetLength(1);

            //kollar så att positionen är rätt och att det är ett block
            bool IsValidPos(int row, int col)
            {
                return row >= 0 && row < rows && col >= 0 && col < cols && grid[row, col] == 1;
            }

            // Visited array to keep track of processed positions
            bool[,] visited = new bool[rows, cols];

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    if (grid[row, col] == 1 && !visited[row, col])
                    {
                        //Bygger en collider
                        Collider collider = new Collider();

                    }
                }
            }

            return colliders;
        }
    }
}