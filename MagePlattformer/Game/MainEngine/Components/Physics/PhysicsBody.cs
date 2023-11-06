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
        public Vector2 velocity;
        public Vector2 acceleration;
        public float drag;
        
        public Vector2 Gravity = new Vector2(0, 9.82f);

        public void AddForce(float force, float angle)
        {
            double radians = angle * Math.PI / 180.0; // Convert degrees to radians

            // Calculate the x and y components of the vector
            double x = Math.Cos(radians);
            double y = Math.Sin(radians);

            // Normalize the vector to have a length of 1
            double length = Math.Sqrt(x * x + y * y);
            x /= length;
            y /= length;

            acceleration += new Vector2((float)x, (float)y) * force / mass;
        }
    }
}