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

            Collider collider = new
            (
                true, 3, new int[1]
            );
            AddComponent<Collider>(collider);
            AddComponent<Sprite>(new Sprite());
        }
    }
}