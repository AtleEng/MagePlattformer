using System.Collections.Generic;
using System.Numerics;
using CoreEngine;
using Engine;
using System.Text.Json;

namespace Engine
{
    public static class LoadingManager
    {
        static string prePath = @"Game\Project\Levels\";

        static JsonSerializerOptions options = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true
        };

        public static void SaveLevel(string path, int[,] level)
        {
            // Convert 2D array to jagged array
            int[][] jaggedArray = ConvertToJaggedArray(level);

            // Convert the jagged array to JSON
            string json = JsonSerializer.Serialize(jaggedArray);

            // Save the JSON string to a file
            File.WriteAllText(Path.Combine(prePath, $"{path}.json"), json);

            Console.WriteLine($"JSON saved to {path}");
        }

        public static int[,]? LoadLevel(string path)
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

        // Convert 2D array to jagged array
        static int[][] ConvertToJaggedArray(int[,] array)
        {
            int rows = array.GetLength(0);
            int cols = array.GetLength(1);

            int[][] jaggedArray = new int[rows][];

            for (int i = 0; i < rows; i++)
            {
                jaggedArray[i] = new int[cols];
                for (int j = 0; j < cols; j++)
                {
                    jaggedArray[i][j] = array[i, j];
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

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    array[i, j] = jaggedArray[i][j];
                }
            }

            return array;
        }
    }
}