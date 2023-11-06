using System.Numerics;
using System.Collections.Generic;
using Raylib_cs;
using CoreEngine;
using Physics;

namespace Engine
{
    public class PlayerMovement : Component, IScript
    {
        PhysicsBody physicsBody;
        public override void Start()
        {
            physicsBody = gameEntity.GetComponent<PhysicsBody>();
        }
        public override void Update(float delta)
        {
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_SPACE))
            {
                physicsBody.velocity.Y = 0;
                physicsBody.AddForce(1000, 270);
            }
            if (Raylib.IsKeyDown(KeyboardKey.KEY_D))
            {
                physicsBody.AddForce(50, 0);
            }
            if (Raylib.IsKeyDown(KeyboardKey.KEY_A))
            {
                physicsBody.AddForce(-50, 0);
            }
        }
    }
}