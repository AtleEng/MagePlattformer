using System.Numerics;
using System.Collections.Generic;
using Raylib_cs;
using Engine;
using Physics;

namespace Engine
{
    public class Block : GameEntity
    {
        public Block()
        {
            name = "Block";

            Sprite sprite = new Sprite
            {
                spriteSheet = Raylib.LoadTexture(@"Game\Project\Sprites\BlocksSpriteSheet.png"),
                spriteGrid = new Vector2(4, 4),
                FrameIndex = 15
            };
            AddComponent<Sprite>(sprite);

            Collider collider = new Collider
            (
                false, 1
            );
            AddComponent<Collider>(collider);
        }
    }
}