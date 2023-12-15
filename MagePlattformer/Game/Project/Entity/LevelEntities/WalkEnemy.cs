using System.Numerics;
using System.Collections.Generic;
using Raylib_cs;
using Animation;
using Physics;

namespace Engine
{
    public class WalkEnemy : GameEntity
    {
        static Texture2D texture;
        public WalkEnemy()
        {
            if (texture.Id == 0)
            {
                texture = Raylib.LoadTexture(@"Game\Project\Sprites\EnemiesSprites.png");
            }
            name = "GoatEnemy";

            //sprite
            Sprite sprite = new Sprite
            {
                spriteSheet = texture,
                spriteGrid = new Vector2(5, 4),
                FrameIndex = 10
            };
            AddComponent<Sprite>(sprite);

            //animation
            Animator animator = new(sprite);

            animator.AddAnimation("Run", new(new int[] { 10, 11, 12, 13 }, 0.1f, true));
            animator.AddAnimation("Fall", new(new int[] { 12, }, 0.1f, false));

            AddComponent<Animator>(animator);

            //physics
            PhysicsBody physicsBody = new PhysicsBody
            {
                dragX = 10f,
                dragY = 0,
                Gravity = new Vector2(0, 50),
                velocity = Vector2.Zero
            };
            AddComponent<PhysicsBody>(physicsBody);

            Collider collider = new Collider
            (
                false, 3
            )
            {
                scale = new Vector2(0.75f, 1)
            };
            AddComponent<Collider>(collider);

            Check wallCheck = new(2);
            EntityManager.SpawnEntity(wallCheck, new Vector2(-0.3f, 0f), new Vector2(0.4f, 0.4f), this);

            EnemyAIBase enemyAI = new();
            enemyAI.beheviors.Add(new EnemyRun(30, wallCheck.GetComponent<Collider>(), null, physicsBody));
            AddComponent<EnemyAIBase>(enemyAI);
        }
    }
}