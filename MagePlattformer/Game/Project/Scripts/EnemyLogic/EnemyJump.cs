using System.Numerics;
using System.Collections.Generic;
using Engine;
using Physics;
using Animation;
public class EnemyJump : EnemyBehevior
{
    //Normal jumping
    float jumpForce = 0;
    float jumpTimer;
    float jumpTime;

    //isGrounded
    Collider groundCheck;
    PhysicsBody pB;

    public EnemyJump(float jumpForce, float jumpTimer, Collider groundCheck, PhysicsBody pB)
    {
        this.jumpForce = jumpForce;
        this.jumpTimer = jumpTimer;
        this.groundCheck = groundCheck;
        this.pB = pB;
    }
    public override void BeheviorUpdate(float delta)
    {
        if (groundCheck.isColliding)
        {
            enemyBase.isGrounded = true;

            jumpTime += delta;
            if (jumpTime >= jumpTimer)
            {
                jumpTime = 0;
                pB.velocity.Y = -jumpForce;
            }
        }
        else
        {
            enemyBase.isGrounded = false;
        }
    }
}