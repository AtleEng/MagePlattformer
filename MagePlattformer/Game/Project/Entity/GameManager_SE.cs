using System.Numerics;
using System.Collections.Generic;
using Raylib_cs;
using Animation;
using Physics;

namespace Engine
{
    public class GameManager : GameEntity
    {
        public GameManager()
        {
            name = "GameManager";

            /*
            GameEntity Playerentity = new()
            {
                name = "Player",
            };

            //sprite
            Sprite sprite = new Sprite
            {
                spriteSheet = Raylib.LoadTexture(@"Game\Project\Sprites\PlayerSpriteSheet.png"),
                spriteGrid = new Vector2(4, 2),
                FrameIndex = 4
            };
            Playerentity.AddComponent<Sprite>(sprite);

            //animation
            Animator animator = new(sprite);

            Animation.Animation idleAnimation = new(new int[] { 4, 5 }, 0.5f, true);
            animator.AddAnimation("Idle", idleAnimation);

            Animation.Animation runAnimation = new(new int[] { 0, 1, 2, 3 }, 0.1f, true);
            animator.AddAnimation("Run", runAnimation);

            Animation.Animation fallAnimation = new(new int[] { 7, }, 0.1f, false);
            animator.AddAnimation("Fall", fallAnimation);

            Animation.Animation jumpAnimation = new(new int[] { 3 }, 0.1f, false);
            animator.AddAnimation("Jump", jumpAnimation);

            Playerentity.AddComponent<Animator>(animator);

            //physics
            PhysicsBody physicsBody = new PhysicsBody
            {
                dragX = 10f,
                dragY = 0,
                Gravity = new Vector2(0, 50),
                velocity = Vector2.Zero
            };
            Playerentity.AddComponent<PhysicsBody>(physicsBody);

            Collider collider = new Collider
            (
                false, 0
            )
            {
                size = new Vector2(0.75f, 1)
            };
            Playerentity.AddComponent<Collider>(collider);

            //Other scripts
            PlayerMovement playerMovement = new();
            Playerentity.AddComponent<PlayerMovement>(playerMovement);

            //ground check (child of player)
            GroundCheck groundCheck = new();
            groundCheck.transform.position = new Vector2(0, 0.4f);
            groundCheck.transform.size = new Vector2(0.5f, 0.5f);
            Playerentity.children.Add(groundCheck);

            EntityManager.SpawnEntity(groundCheck, new Vector2(0, 0.4f), new Vector2(0.5f, 0.5f), this);
            playerMovement.groundCheck = groundCheck.GetComponent<Collider>();


            LoadingManager.SaveEntity(Playerentity, "Player");


            GameEntity player = LoadingManager.LoadEntity("Player");
            EntityManager.SpawnEntity(player);
            */

            EntityManager.SpawnEntity(new Player(), new Vector2(0, -5));

            EntityManager.SpawnEntity(new JumpPad(), new Vector2(5, 4));

            EntityManager.SpawnEntity(new WalkingEnemy(), new Vector2(3, 2));
            EntityManager.SpawnEntity(new JumpingEnemy(), new Vector2(5, 0));

            EntityManager.SpawnEntity(new Block(), new Vector2(0, 7), new Vector2(11, 3));
            EntityManager.SpawnEntity(new Block(), new Vector2(-3, -5), new Vector2(5, 5));
            EntityManager.SpawnEntity(new Block(), new Vector2(10, 3), new Vector2(9, 7));
            EntityManager.SpawnEntity(new Block(), new Vector2(0, 4), new Vector2(4, 3));
        }
    }
}