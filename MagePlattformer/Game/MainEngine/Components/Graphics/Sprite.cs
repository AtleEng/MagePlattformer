using System.Numerics;
using System.Collections.Generic;
using Raylib_cs;
using CoreEngine;
using Engine;

namespace Engine
{
    public class Sprite : Component
    {
        public Texture2D spriteSheet;
        public Vector2 spriteGrid = Vector2.One;
        int frameIndex;
        public int FrameIndex
        {
            get
            {
                return frameIndex;
            }
            set
            {
                if (value > spriteGrid.X * spriteGrid.Y - 1)
                {
                    frameIndex = 0;
                }
                else if (value < 0)
                {
                    frameIndex = (int)(spriteGrid.X * spriteGrid.Y - 1);
                }
                else
                {
                    frameIndex = value;
                }
            }
        }
        public Color colorTint = Color.WHITE;
        public int layer;
        public bool isFlipedY;
        public bool isFlipedX;

        public override string PrintStats()
        {
            return $"SpriteGrid: {spriteGrid} FrameIndex: {frameIndex} Layer: {layer}";
        }
    }
}