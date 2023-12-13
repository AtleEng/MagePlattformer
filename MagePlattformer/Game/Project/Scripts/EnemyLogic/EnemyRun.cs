using System.Numerics;
using System.Collections.Generic;
using Raylib_cs;
using Engine;
using Physics;
using Animation;
public class EnemyRun : EnemyBehevior
{
    //X Movement
    private float moveSpeed = 20;
    private float moveInput = -1;

    Collider? wallCheck;
    Collider? ledgeCheck;
    PhysicsBody pB;

    public EnemyRun(float moveSpeed, Collider? wallCheck, Collider? ledgeCheck, PhysicsBody pB)
    {
        this.moveSpeed = moveSpeed;
        this.wallCheck = wallCheck;
        this.ledgeCheck = ledgeCheck;
        this.pB = pB;
    }
    public override void BeheviorUpdate(float delta)
    {
        pB.velocity.X += moveInput * moveSpeed * delta;
        if (wallCheck != null)
        {
            if (wallCheck.isColliding)
            {
                wallCheck.gameEntity.transform.position = new Vector2(-wallCheck.gameEntity.transform.position.X, wallCheck.gameEntity.transform.position.Y);
                moveInput *= -1;
            }
        }
        if (ledgeCheck != null)
        {
            if (!ledgeCheck.isColliding && pB.velocity.Y <= 0)
            {
                ledgeCheck.gameEntity.transform.position = new Vector2(-ledgeCheck.gameEntity.transform.position.X, ledgeCheck.gameEntity.transform.position.Y);
                moveInput *= -1;
            }
        }
    }
}