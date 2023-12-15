using System.Numerics;
using System.Collections.Generic;
using Raylib_cs;
using Engine;
using Physics;
using Animation;

namespace Engine
{
    public class JumpPad : GameEntity
    {
        static Texture2D texture;
        public JumpPad()
        {
            if (texture.Id == 0)
            {
                texture = Raylib.LoadTexture(@"Game\Project\Sprites\EnemiesSprites.png");
            }
            name = "JumpPad";

            Sprite sprite = new Sprite
            {
                spriteSheet = texture,
                spriteGrid = new Vector2(5, 4),
                FrameIndex = 4
            };
            AddComponent<Sprite>(sprite);

            //animation
            Animator animator = new(sprite);

            Animation.Animation jumpAnimation = new(new int[] { 4, 9, 14 }, 0.1f, false);
            animator.AddAnimation("Jump", jumpAnimation);

            AddComponent<Animator>(animator);

            Collider collider = new Collider
            (
                true, 1
            )
            {
                scale = new Vector2(0.5f, 0.5f)
            };
            AddComponent<Collider>(collider);

            PhysicsBody physicsBody = new PhysicsBody
            {
                physicsType = PhysicsBody.PhysicsType.staticType
            };
            AddComponent<PhysicsBody>(physicsBody);

            AddComponent<JumpPadScript>(new JumpPadScript());
        }
    }
}