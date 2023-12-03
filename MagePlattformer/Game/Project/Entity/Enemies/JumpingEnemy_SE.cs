using System.Numerics;
using System.Collections.Generic;
using Raylib_cs;
using Animation;
using Physics;

namespace Engine
{
    public class JumpingEnemy : GameEntity
    {
        public JumpingEnemy()
        {
            name = "JumpingEnemy";

            //sprite
            Sprite sprite = new Sprite
            {
                spriteSheet = Raylib.LoadTexture(@"Game\Project\Sprites\PlayerSpriteSheet.png"),
                spriteGrid = new Vector2(4, 2),
                FrameIndex = 4
            };
            AddComponent<Sprite>(sprite);

            //animation
            Animator animator = new(sprite);

            animator.AddAnimation("Idle", new(new int[] { 4, 5 }, 0.5f, true));
            animator.AddAnimation("Run", new(new int[] { 0, 1, 2, 3 }, 0.1f, true));
            animator.AddAnimation("Fall", new(new int[] { 6, }, 0.1f, false));
            animator.AddAnimation("Jump", new(new int[] { 3 }, 0.1f, false));

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
                false, 0
            )
            {
                size = new Vector2(0.5f, 1)
            };
            AddComponent<Collider>(collider);

            //Other scripts
            EnemyMovement enemyAI = new(20, 20);
            AddComponent<EnemyMovement>(enemyAI);

            //ground check (child of player)
            Check groundCheck = new(2);
            EntityManager.SpawnEntity(groundCheck, new Vector2(0, 0.4f), new Vector2(0.5f, 0.5f), this);
            enemyAI.groundCheck = groundCheck.GetComponent<Collider>();

            Check wallCheck = new(2);
            EntityManager.SpawnEntity(wallCheck, new Vector2(-0.3f, 0f), new Vector2(0.4f, 0.4f), this);
            enemyAI.wallCheck = wallCheck.GetComponent<Collider>();
        }
    }
}