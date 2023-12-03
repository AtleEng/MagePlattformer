using System.Numerics;
using System.Collections.Generic;
using Raylib_cs;
using Engine;
using Physics;
using Animation;

namespace Engine
{
    [Serializable]
    public class EnemyMovement : Component, IScript
    {
        public EnemyMovement() { }

        public EnemyMovement(float moveSpeed, float jumpForce)
        {
            this.moveSpeed = moveSpeed;
            this.jumpForce = jumpForce;
        }

        //X Movement
        private float moveSpeed = 20;
        private float moveInput = -1;

        //Normal jumping
        private float jumpForce = 0;

        //isGrounded
        public Transform feetPos;

        public Collider groundCheck;
        public Collider wallCheck;
        private bool isGrounded = true;

        //Physics
        float maxVelocityX = 10;
        float maxVelocityY = 30;
        public PhysicsBody pB;

        //Sprite & Animation
        Sprite sprite;
        Animator anim;
        EnemyStates playerStates = EnemyStates.idle;


        public override void Start()
        {
            pB = gameEntity.GetComponent<PhysicsBody>();
            anim = gameEntity.GetComponent<Animator>();
            sprite = gameEntity.GetComponent<Sprite>();
        }
        public override void Update(float delta)
        {
            if (isGrounded)
            {
                if (pB.velocity.X != 0)
                {
                    if (playerStates != EnemyStates.running)
                    {
                        playerStates = EnemyStates.running;
                        anim.PlayAnimation("Run");
                    }
                }
                else
                {
                    if (playerStates != EnemyStates.idle)
                    {
                        playerStates = EnemyStates.idle;
                        anim.PlayAnimation("Idle");
                    }
                }
            }
            else
            {
                if (pB.velocity.Y < 0)
                {
                    if (playerStates != EnemyStates.jump)
                    {
                        playerStates = EnemyStates.jump;
                        anim.PlayAnimation("Jump");
                    }
                }
                else
                {
                    if (playerStates != EnemyStates.fall)
                    {
                        playerStates = EnemyStates.fall;
                        anim.PlayAnimation("Fall");
                    }
                }
            }

            Jump();
            xMovement(delta);

            pB.velocity.X = Math.Clamp(pB.velocity.X, -maxVelocityX, maxVelocityX);
            pB.velocity.Y = Math.Clamp(pB.velocity.Y, -maxVelocityY, maxVelocityY);
        }
        void xMovement(float delta)
        {
            pB.velocity.X += moveInput * moveSpeed * delta;

            if (wallCheck.isColliding)
            {
                System.Console.WriteLine("Bounce");
                wallCheck.gameEntity.transform.position = new Vector2(-wallCheck.gameEntity.transform.position.X, wallCheck.gameEntity.transform.position.Y);
                moveInput *= -1;
                if (moveInput > 0)
                {
                    sprite.isFlipedX = false;
                }
                else if (moveInput < 0)
                {
                    sprite.isFlipedX = true;
                }
            }
        }
        void Jump()
        {
            if (groundCheck.isColliding)
            {
                pB.velocity.Y = -jumpForce;
            }
        }
        public enum EnemyStates
        {
            idle, running, jump, fall, landing,
        }
    }
}
