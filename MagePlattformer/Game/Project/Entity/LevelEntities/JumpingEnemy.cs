using System.Numerics;
using System.Collections.Generic;
using Raylib_cs;
using Animation;
using Physics;

namespace Engine
{
    public class JumpingEnemy : GameEntity
    {
        static Texture2D texture;
        public JumpingEnemy()
        {
            if (texture.Id == 0)
            {
                texture = Raylib.LoadTexture(@"Game\Project\Sprites\EnemiesSprites.png");
            }
            name = "JumpingRobotEnemy";

            //sprite
            Sprite sprite = new Sprite
            {
                spriteSheet = texture,
                spriteGrid = new Vector2(5, 4),
                FrameIndex = 0
            };
            AddComponent<Sprite>(sprite);

            //animation
            Animator animator = new(sprite);

            animator.AddAnimation("Run", new(new int[] { 0 }, 0.1f, false));
            animator.AddAnimation("Fall", new(new int[] { 1 }, 0.1f, false));
            animator.AddAnimation("Jump", new(new int[] { 2 }, 0.1f, false));

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

            Check groundCheck = new(2);
            EntityManager.SpawnEntity(groundCheck, new Vector2(0, 0.4f), new Vector2(0.5f, 0.5f), this);

            Check wallCheck = new(2);
            EntityManager.SpawnEntity(wallCheck, new Vector2(-0.3f, 0f), new Vector2(0.4f, 0.4f), this);


            EnemyAIBase enemyAI = new();
            enemyAI.beheviors.Add(new EnemyRun(30, wallCheck.GetComponent<Collider>(), null, physicsBody));
            enemyAI.beheviors.Add(new EnemyJump(15, 0.1f, groundCheck.GetComponent<Collider>(), physicsBody));

            AddComponent<EnemyAIBase>(enemyAI);
        }
    }
}