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
        public JumpPad()
        {
            name = "JumpPad";

            Sprite sprite = new Sprite
            {
                spriteSheet = Raylib.LoadTexture(@"Game\Project\Sprites\JumpPad.png"),
                spriteGrid = new Vector2(3, 1),
                FrameIndex = 0
            };
            AddComponent<Sprite>(sprite);

            //animation
            Animator animator = new(sprite);

            Animation.Animation jumpAnimation = new(new int[] { 0, 1, 2 }, 0.1f, false);
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