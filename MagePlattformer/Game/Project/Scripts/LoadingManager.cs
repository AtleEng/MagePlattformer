using System.Collections.Generic;
using System.Numerics;
using CoreEngine;
using Engine;
using System.Text.Json;
using Physics;

namespace Engine
{
    public static class LoadingManager
    {
        static public int currentLevel = 0;
        static string prePath = @"Game\Project\Levels\";
        static Dictionary<int, Type> entitysInLevel = new()
        {
            {1, typeof(Block)},
            {2, typeof(JumpPad)},
            { 3, typeof(WalkEnemy)},
            { 4, typeof(JumpingEnemy)},
            {5, typeof(RandomEnemy)},
            {9, typeof(Portal)},
        };

        static Dictionary<int, string> levels = new()
        {
            {1, "Level1"},
            {2, "Level2"},
            {3, "Level3"}
        };

        static List<GameEntity> levelEntities = new();


        static JsonSerializerOptions options = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true
        };

        public static void SaveLevel(string path, int[,] level)
        {
            // omvandla 2D array till en jagged array (för att json inte stödjer det)
            int[][] jaggedArray = ConvertToJaggedArray(level);

            // omvandla jagged array to JSON
            string json = JsonSerializer.Serialize(jaggedArray);

            // Spara json på filen
            File.WriteAllText(Path.Combine(prePath, $"{path}.json"), json);

            Console.WriteLine($"JSON saved to {path}");
        }

        static public void Load(int i)
        {
            if (levels.ContainsKey(i))
            {
                currentLevel = i;
                ClearLevel();
                int[,] level = LoadLevel(levels[i]);
                SpawEntitiesInLevel(level);
            }
        }
        public static int[,] LoadLevel(string path)
        {
            try
            {
                // Load JSON string from file
                string json = File.ReadAllText(Path.Combine(prePath, $"{path}.json"));

                // Deserialize JSON to jagged array
                int[][] jaggedArray = JsonSerializer.Deserialize<int[][]>(json, options);

                // Convert jagged array to 2D array
                int[,] level = ConvertTo2DArray(jaggedArray);

                if (level != null)
                {
                    System.Console.WriteLine($"{path} loaded");
                    return level;
                }
                else
                {
                    System.Console.WriteLine($"Deserialization failed, {path} was not loaded");
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error loading {path}: {e.Message}");
                return null;
            }
        }

        static void SpawEntitiesInLevel(int[,] level)
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
        static void ClearLevel()
        {
            foreach (GameEntity entity in levelEntities)
            {
                EntityManager.DestroyEntity(entity);
            }
            levelEntities.Clear();
        }
        // Method to spawn GameEntity based on int key
        static GameEntity GetEntityInstance(int key)
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


        // Convert 2D array to jagged array
        static int[][] ConvertToJaggedArray(int[,] array)
        {
            int rows = array.GetLength(0);
            int cols = array.GetLength(1);

            int[][] jaggedArray = new int[rows][];

            for (int x = 0; x < rows; x++)
            {
                jaggedArray[x] = new int[cols];
                for (int y = 0; y < cols; y++)
                {
                    jaggedArray[x][y] = array[x, y];
                }
            }
            return jaggedArray;
        }
        // Convert jagged array to 2D array
        static int[,] ConvertTo2DArray(int[][] jaggedArray)
        {
            int rows = jaggedArray.Length;
            int cols = jaggedArray[0].Length;

            int[,] array = new int[rows, cols];

            for (int x = 0; x < rows; x++)
            {
                for (int y = 0; y < cols; y++)
                {
                    array[x, y] = jaggedArray[x][y];
                }
            }
            return array;
        }
    }
}