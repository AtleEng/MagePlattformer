using System.Numerics;
using System.Collections.Generic;
using Raylib_cs;
using Physics;

namespace Engine
{
    public class Check : GameEntity
    {
        public Check() : this(0)
        {

        }
        public Check(int colliderLayer)
        {
            name = "Check";

            Collider collider = new
            (
                true, colliderLayer
            );
            AddComponent<Collider>(collider);
            AddComponent<Sprite>(new Sprite());
        }
    }
}