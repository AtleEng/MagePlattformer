using System.Numerics;
using System.Collections.Generic;
using Raylib_cs;
using Physics;

namespace Engine
{
    public class Player : GameEntity
    {
        public Player()
        {
            name = "Player";

            Sprite sprite = new Sprite
            {
                spriteSheet = Raylib.LoadTexture(@"Project\Sprites\simple.png"),
                spriteGrid = new Vector2(2, 2)
            };
            AddComponent<Sprite>(sprite);
            PhysicsBody physicsBody = new PhysicsBody
            {
                dragX = 0.3f,
                dragY = 0,
                Gravity = new Vector2(0, 50)
            };
            AddComponent<PhysicsBody>(physicsBody);

            AddComponent<PlayerMovement>(new PlayerMovement());
        }
    }
}