using System.Numerics;
using System.Collections.Generic;
using Raylib_cs;
using Engine;
using Physics;
using CoreAnimation;

namespace Engine
{
    public class PlayerMovement : Component, IScript
    {
        //X Movement
        private float moveSpeed = 30;
        private float moveInput;

        //Normal jumping
        private float jumpForce = 20;

        private float jumpTime = 0.3f;
        private float jumpTimeCounter;

        private bool isJumping;

        private float hangTime = 0.1f;
        private float hangTimeCounter;

        private float jumpBufferLength = 0.1f;
        private float jumpBufferLengthCounter;

        //isGrounded
        private Transform feetPos;

        private float checkRadius;

        //private LayerMask whatIsGround;
        private bool isGrounded = true;

        //Physics
        float maxVelocityX = 10;
        float maxVelocityY = 30;
        private PhysicsBody pB;

        //Animation
        private Animator anim;

        public override void Start()
        {
            pB = gameEntity.GetComponent<PhysicsBody>();
            anim = gameEntity.GetComponent<Animator>();
        }
        public override void Update(float delta)
        {
            Inputs(delta);
            CheckWorld();

            Flip();

            Jump();
            xMovement();

            pB.velocity.X = Math.Clamp(pB.velocity.X, -maxVelocityX, maxVelocityX);
            pB.velocity.Y = Math.Clamp(pB.velocity.Y, -maxVelocityY, maxVelocityY);
        }
        void Inputs(float delta)
        {
            //manage hangtime
            if (isGrounded)
            {
                hangTimeCounter = hangTime;
            }
            else
            {
                hangTimeCounter -= delta;
            }
            //manage jump buffer
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_SPACE))
            {
                jumpBufferLengthCounter = jumpBufferLength;
            }
            else
            {
                jumpBufferLengthCounter -= delta;
            }

            //manage jump
            if (jumpBufferLengthCounter >= 0 && hangTimeCounter > 0)
            {
                isJumping = true;
                jumpTimeCounter = jumpTime;
                jumpBufferLengthCounter = 0;
                hangTimeCounter = 0;
            }

            if (Raylib.IsKeyDown(KeyboardKey.KEY_SPACE) && isJumping == true)
            {
                if (jumpTimeCounter > 0)
                {
                    jumpTimeCounter -= delta;
                }
                else
                {
                    isJumping = false;
                }
            }

            if (!Raylib.IsKeyDown(KeyboardKey.KEY_SPACE))
            {
                isJumping = false;
            }
        }
        void CheckWorld()
        {
            //isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);
        }
        void xMovement()
        {
            if (Raylib.IsKeyDown(KeyboardKey.KEY_D))
            {
                moveInput++;
            }
            if (Raylib.IsKeyDown(KeyboardKey.KEY_A))
            {
                moveInput--;
            }
            pB.velocity = new Vector2(moveInput * moveSpeed, pB.velocity.Y);
            moveInput = 0;
        }
        void Flip()
        {
            if (moveInput > 0)
            {

            }
            else if (moveInput < 0)
            {

            }
        }
        void Jump()
        {
            if (isJumping)
            {
                pB.velocity = new Vector2(0, -1) * jumpForce;
            }
        }
    }
}
