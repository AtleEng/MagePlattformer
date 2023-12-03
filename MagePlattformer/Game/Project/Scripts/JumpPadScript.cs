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

        float jumpForce = -50;

        Animator anim;

        public override void Start()
        {
            anim = gameEntity.GetComponent<Animator>();
        }

        public override void OnTrigger(Collider other)
        {
            PlayerMovement? player = other.gameEntity.GetComponent<PlayerMovement>();
            if (player != null)
            {
                if (player.pB.velocity.Y > 0)
                {
                    player.pB.velocity.Y = jumpForce;
                    anim.PlayAnimation("Jump");
                }
            }
        }

    }
}
