using System.Numerics;
using System.Collections.Generic;
using Raylib_cs;
using Physics;

namespace Engine
{
    public class GroundCheck : GameEntity
    {
        public GroundCheck()
        {
            name = "GroundCheck";

            AddComponent<PlayerMovement>(new PlayerMovement());

            Collider collider = new Collider
            (
                false, 1, new int[1]
            );
            AddComponent<Collider>(collider);
        }
    }
}