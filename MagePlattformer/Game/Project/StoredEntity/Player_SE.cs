using System.Numerics;
using System.Collections.Generic;
using Raylib_cs;
using CoreAnimation;
using Physics;

namespace Engine
{
    public class Player : GameEntity
    {
        public Player()
        {
            name = "Player";

            //sprite
            Sprite sprite = new Sprite
            {
                spriteSheet = Raylib.LoadTexture(@"C:\Users\atlee\Documents\GameDev\Projects\MagePlattformer\MagePlattformer\Game\Project\Sprites\PlayerSpriteSheet.png"),
                spriteGrid = new Vector2(4, 2),
                FrameIndex = 4
            };
            AddComponent<Sprite>(sprite);

            //animation
            Animator animator = new(sprite);

            Animation idleAnimation = new(new int[] { 4, 5 }, 0.5f, true);
            animator.AddAnimation("Idle", idleAnimation);

            Animation runAnimation = new(new int[] { 0, 1, 2, 3 }, 0.1f, true);
            animator.AddAnimation("Run", runAnimation);

            Animation fallAnimation = new(new int[] { 7, }, 0.1f, false);
            animator.AddAnimation("Fall", fallAnimation);

            Animation jumpAnimation = new(new int[] {3}, 0.1f, false);
            animator.AddAnimation("Jump", jumpAnimation);

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
                size = new Vector2(0.75f, 1)
            };
            AddComponent<Collider>(collider);

            //Other scripts
            PlayerMovement playerMovement = new();
            AddComponent<PlayerMovement>(playerMovement);

            //ground check (child of player)
            GroundCheck groundCheck = new();
            EntityManager.SpawnEntity(groundCheck, new Vector2(0, 0.4f), new Vector2(0.5f, 0.5f), this);
            playerMovement.groundCheck = groundCheck.GetComponent<Collider>();
        }
    }
}