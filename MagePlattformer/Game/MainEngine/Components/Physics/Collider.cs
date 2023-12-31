using System.Numerics;
using System.Collections.Generic;
using Raylib_cs;
using CoreEngine;
using Engine;

namespace Physics
{
    [Serializable]
    public class Collider : Component
    {
        public Collider() { }

        public Vector2 offset;
        public Vector2 scale = Vector2.One;

        public bool isTrigger;

        public int layer;

        public bool isColliding;
        public Collider(bool isTrigger, int layer)
        {
            this.isTrigger = isTrigger;
            this.layer = layer;
        }
    }

}