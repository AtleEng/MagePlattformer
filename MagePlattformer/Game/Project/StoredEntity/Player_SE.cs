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
                spriteSheet = Raylib.LoadTexture(@"C:\Users\atlee\Documents\GameDev\Projects\MagePlattformer\MagePlattformer\Game\Project\Sprites\simple.png"),
                spriteGrid = new Vector2(2, 2)
            };
            AddComponent<Sprite>(sprite);
            PhysicsBody physicsBody = new PhysicsBody
            {
                drag = 0.1f,
                Gravity = new Vector2(0, 30f)
            };
            AddComponent<PhysicsBody>(physicsBody);

            AddComponent<PlayerMovement>(new PlayerMovement());
        }
    }
}