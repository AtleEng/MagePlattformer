using System.Numerics;
using System.Collections.Generic;
using Raylib_cs;
using CoreEngine;
using Engine;

namespace Physics
{
    public class PhysicsBody : Component
    {
        public float mass = 1;
        public Vector2 velocity = Vector2.Zero;
        public Vector2 acceleration = Vector2.Zero;
        public float dragX = 0f;
        public float dragY = 0f;

        public float elasticity = 0f;

        public Vector2 Gravity = new Vector2(0, 9.82f);

        public void AddForce(Vector2 force)
        {
            if (mass == 0) { mass = 1; }

            acceleration += force / mass;
        }
        public override string PrintStats()
        {
            return $"Velocity: {velocity}";
        }
    }
}