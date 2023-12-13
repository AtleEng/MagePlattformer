using System.Numerics;
using System.Collections.Generic;
using Raylib_cs;
using Engine;
using Physics;
using Animation;

namespace Engine
{
    [Serializable]
    public class JumpPadScript : Component, IScript
    {
        public JumpPadScript() { }

        float jumpForce = -25;

        Animator anim;

        public override void Start()
        {
            anim = gameEntity.GetComponent<Animator>();
        }

        public override void OnTrigger(Collider other)
        {
            PhysicsBody? pB = other.gameEntity.GetComponent<PhysicsBody>();
            if (pB != null)
            {
                if (pB.physicsType == PhysicsBody.PhysicsType.dynamicType)
                {
                    pB.velocity.Y = jumpForce;
                    anim.PlayAnimation("Jump");
                }
            }
        }

    }
}
