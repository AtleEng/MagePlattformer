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

            Sprite sprite = new Sprite
            {
                spriteSheet = Raylib.LoadTexture(@"C:\Users\atlee\Documents\GameDev\Projects\MagePlattformer\MagePlattformer\Game\Project\Sprites\PlayerSpriteSheet.png"),
                spriteGrid = new Vector2(4, 2),
                FrameIndex = 4
            };
            AddComponent<Sprite>(sprite);
            Animator animator = new(sprite);
            Animation idleAnimation = new(new int[] { 4, 5 }, 0.5f, true);
            animator.AddAnimation("Idle", idleAnimation);
            Animation runAnimation = new(new int[] { 0, 1, 2, 3 }, 0.1f, true);
            animator.AddAnimation("Run", runAnimation);
            AddComponent<Animator>(animator);

            PhysicsBody physicsBody = new PhysicsBody
            {
                dragX = 0.3f,
                dragY = 0,
                Gravity = new Vector2(0, 50),
                velocity = Vector2.Zero
            };
            AddComponent<PhysicsBody>(physicsBody);

            PlayerMovement playerMovement = new();
            AddComponent<PlayerMovement>(playerMovement);

            Collider collider = new Collider
            (
                false, 0
            )
            {
                size = new Vector2(0.75f, 1)
            };
            AddComponent<Collider>(collider);

            GroundCheck groundCheck = new();
            EntityManager.SpawnEntity(groundCheck, new Vector2(0, 0.4f), new Vector2(0.5f, 0.5f), this);
            playerMovement.groundCheck = groundCheck.GetComponent<Collider>();
        }
    }
}