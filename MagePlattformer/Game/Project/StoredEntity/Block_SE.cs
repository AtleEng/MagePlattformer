using System.Numerics;
using System.Collections.Generic;
using Raylib_cs;
using Engine;

namespace Engine
{
    public class Block : GameEntity
    {
        public Block()
        {
            name = "Block";

           Sprite sprite = new Sprite
            {
                spriteSheet = Raylib.LoadTexture(@"C:\Users\atlee\Documents\GameDev\Projects\MagePlattformer\MagePlattformer\Game\Project\Sprites\simple.png"),
                spriteGrid = new Vector2(2, 2),
                FrameIndex = 2
            };
            AddComponent<Sprite>(sprite);
        }
    }
}