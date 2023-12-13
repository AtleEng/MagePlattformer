using System.Numerics;
using System.Collections.Generic;
using Raylib_cs;
using Engine;
using Physics;
using Animation;

namespace Engine
{
    public class Portal : GameEntity
    {
        public Portal()
        {
            name = "Portal";

            Sprite sprite = new Sprite
            {
                spriteSheet = Raylib.LoadTexture(@"Game\Project\Sprites\EnemiesSprites.png"),
                spriteGrid = new Vector2(2, 4),
                FrameIndex = 0
            };
            AddComponent<Sprite>(sprite);

            //animation
            Animator animator = new(sprite);

            Animation.Animation animation = new(new int[] { 0, 1, 2,3 }, 0.1f, true);
            animator.AddAnimation("Idle", animation);

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