using System.Numerics;
using Raylib_cs;
using CoreEngine;

namespace Engine
{
    public static class WorldSpace
    {
        public static int pixelsPerUnit = 30;
        static SpriteSystem RenderSystem => (SpriteSystem)Core.systems[typeof(SpriteSystem)];
        public static Vector2 GetVirtualMousePos() // Uppdatera virtuella musen (låst till spelfönstret)
        {
            // Virtual mouse position
            Vector2 virtualMousePosition;

            // Get the mouse position
            Vector2 mousePosition = Raylib.GetMousePosition();

            // Calculate the virtual mouse position adjusted to the game window
            virtualMousePosition.X = (mousePosition.X - (Raylib.GetScreenWidth() - (WindowSettings.gameScreenWidth * RenderSystem.scale)) * 0.5f) / RenderSystem.scale;
            virtualMousePosition.Y = (mousePosition.Y - (Raylib.GetScreenHeight() - (WindowSettings.gameScreenHeight * RenderSystem.scale)) * 0.5f) / RenderSystem.scale;

            // Clamp the virtual mouse position to the game window boundaries
            virtualMousePosition.X = Math.Clamp(virtualMousePosition.X, 0f, WindowSettings.gameScreenWidth);
            virtualMousePosition.Y = Math.Clamp(virtualMousePosition.Y, 0f, WindowSettings.gameScreenHeight);

            virtualMousePosition.X = (virtualMousePosition.X - WindowSettings.gameScreenWidth / 2) / Camera.zoom / pixelsPerUnit;
            virtualMousePosition.Y = (virtualMousePosition.Y - WindowSettings.gameScreenHeight / 2) / Camera.zoom / pixelsPerUnit;

            virtualMousePosition += Camera.position;

            return virtualMousePosition;
        }

        public static int BaseUnitsToPixels(float units)
        {
            int pixels = (int)(units * pixelsPerUnit);
            return pixels;
        }
        public static float PixelsToBaseUnits(int pixels)
        {
            float units = (float)pixels / pixelsPerUnit;
            return units;
        }
        public static Vector2 ConvertToCameraPosition(Vector2 v)
        {
            v.X = (v.X - Camera.position.X) * pixelsPerUnit * Camera.zoom + WindowSettings.gameScreenWidth / 2;
            v.Y = (v.Y - Camera.position.Y) * pixelsPerUnit * Camera.zoom + WindowSettings.gameScreenHeight / 2;

            return v;
        }
        public static Vector2 ConvertToCameraSize(Vector2 v)
        {
            return v * Camera.zoom * pixelsPerUnit;
        }
    }

    public static class WindowSettings
    {
        readonly public static int startWindowWidth = 800;
        readonly public static int startWindowHeight = 450;

        public static int gameScreenWidth = 1120;
        public static int gameScreenHeight = 800;
    }
    public static class Camera
    {
        public static Vector2 position = new();
        public static float zoom = 1;
    }
}