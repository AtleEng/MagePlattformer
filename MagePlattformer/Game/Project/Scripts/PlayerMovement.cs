using System.Numerics;
using System.Collections.Generic;
using Raylib_cs;
using CoreEngine;
using Physics;

namespace Engine
{
    public class PlayerMovement : Component, IScript
    {
        float maxHorizantalSpeed = 7;
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
                physicsBody.velocity.Y = -15;
            }
            if (Raylib.IsKeyDown(KeyboardKey.KEY_D))
            {
                physicsBody.AddForce(new Vector2(20,0));
            }
            if (Raylib.IsKeyDown(KeyboardKey.KEY_A))
            {
                physicsBody.AddForce(new Vector2(-20,0));
            }
            physicsBody.velocity.X = Math.Clamp(physicsBody.velocity.X, -maxHorizantalSpeed, maxHorizantalSpeed);
        }
    }
}