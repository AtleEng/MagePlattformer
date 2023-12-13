using System.Numerics;
using System.Collections.Generic;
using Raylib_cs;
using Engine;
using Physics;
using Animation;

namespace Engine
{
    [Serializable]
    public class EnemyAIBase : Component, IScript
    {
        public List<EnemyBehevior> beheviors = new();

        public bool isGrounded = true;

        //Physics
        float maxVelocityX = 10;
        float maxVelocityY = 30;
        public PhysicsBody pB;

        //Sprite & Animation
        Sprite sprite;
        Animator anim;

        EnemyStates enemyStates = EnemyStates.idle;
        public override void Start()
        {
            pB = gameEntity.GetComponent<PhysicsBody>();
            anim = gameEntity.GetComponent<Animator>();
            sprite = gameEntity.GetComponent<Sprite>();

            foreach (EnemyBehevior behevior in beheviors)
            {
                behevior.enemyBase = this;
            }
        }
        public override void Update(float delta)
        {
            foreach (EnemyBehevior behevior in beheviors)
            {
                behevior.BeheviorUpdate(delta);
            }

            if (isGrounded)
            {
                if (pB.velocity.X != 0)
                {
                    if (enemyStates != EnemyStates.running)
                    {
                        enemyStates = EnemyStates.running;
                        anim.PlayAnimation("Run");
                    }
                }
                else
                {
                    if (enemyStates != EnemyStates.idle)
                    {
                        enemyStates = EnemyStates.idle;
                        anim.PlayAnimation("Idle");
                    }
                }
            }
            else
            {
                if (pB.velocity.Y < 0)
                {
                    if (enemyStates != EnemyStates.jump)
                    {
                        enemyStates = EnemyStates.jump;
                        anim.PlayAnimation("Jump");
                    }
                }
                else
                {
                    if (enemyStates != EnemyStates.fall)
                    {
                        enemyStates = EnemyStates.fall;
                        anim.PlayAnimation("Fall");
                    }
                }
            }
            if (pB.velocity.X > 0)
            {
                sprite.isFlipedX = false;
            }
            else if (pB.velocity.X < 0)
            {
                sprite.isFlipedX = true;
            }

            pB.velocity.X = Math.Clamp(pB.velocity.X, -maxVelocityX, maxVelocityX);
            pB.velocity.Y = Math.Clamp(pB.velocity.Y, -maxVelocityY, maxVelocityY);
        }
        public enum EnemyStates
        {
            idle, running, jump, fall, landing,
        }
    }

    public abstract class EnemyBehevior
    {
        public EnemyAIBase enemyBase;
        public virtual void BeheviorUpdate(float delta) { }
    }
}