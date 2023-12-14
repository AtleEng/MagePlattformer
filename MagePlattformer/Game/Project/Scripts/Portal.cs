using System.Numerics;
using System.Collections.Generic;
using Raylib_cs;
using Engine;
using Physics;
using Animation;

namespace Engine
{
    [Serializable]
    public class PortalScript : Component, IScript
    {
        public PortalScript() { }

        float jumpForce = -25;

        Animator anim;

        public override void Start()
        {
            anim = gameEntity.GetComponent<Animator>();
            anim.PlayAnimation("Idle");
        }

        public override void OnTrigger(Collider other)
        {
            PlayerMovement? player = other.gameEntity.GetComponent<PlayerMovement>();
            if (player != null)
            {
                LoadingManager.Load(LoadingManager.currentLevel + 1);
            }
        }

    }
}
