using System.Numerics;
using System.Collections.Generic;
using Raylib_cs;
using CoreEngine;
using Engine;

namespace Physics
{
    public class Collider : Component
    {
        public Vector2 origin;
        public Vector2 size = Vector2.One;

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