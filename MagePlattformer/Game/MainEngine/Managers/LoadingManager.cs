using System.Collections.Generic;
using System.Numerics;
using CoreEngine;
using Engine;
using System.Text.Json;

namespace Engine
{
    public static class LoadingManager
    {
        static string prePath = @"Game\Project\StoredEntity\";

        static JsonSerializerOptions options = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true
        };

        public static GameEntity LoadEntity(string path)
        {
            try
            {
                string jsonString = File.ReadAllText(Path.Combine(prePath, $"{path}.json"));
                GameEntity gE = JsonSerializer.Deserialize<GameEntity>(jsonString, options);

                if (gE != null)
                {
                    System.Console.WriteLine("GameEntity Loaded");
                    return gE;
                }
                else
                {
                    System.Console.WriteLine("Deserialization failed, no entity was loaded");
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error loading entity {path}: {e.Message}");
                return null;
            }
        }

        public static void SaveEntity(GameEntity gE, string name)
        {
            // Serialize the GameEntity to JSON
            string json = JsonSerializer.Serialize<GameEntity>(gE, options);

            // Save the JSON string to a file
            File.WriteAllText(Path.Combine(prePath, $"{name}.json"), json);
        }
    }

    public class Level : GameEntity
    {
        List<GameEntity> gameEntitiesInScene = new()
        {
            new GameManager()
        };
        public override void OnInnit()
        {
            foreach (GameEntity gameEntity in gameEntitiesInScene)
            {
                EntityManager.SpawnEntity(gameEntity, gameEntity.transform.position, gameEntity.transform.size, this);
            }
        }
    }
}