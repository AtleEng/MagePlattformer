using System.Numerics;
using CoreEngine;
using Engine;
using Physics;
public class PhysicsSystem : GameSystem
{
    public override void Update(float delta)
    {
        foreach (GameEntity gameEntity in Core.activeGameEntities)
        {
            PhysicsBody? physicsBody = gameEntity.components.ContainsKey(typeof(PhysicsBody)) ? gameEntity.components[typeof(PhysicsBody)] as PhysicsBody : null;

            if (physicsBody != null)
            {
                // Apply drag separately to X and Y
                physicsBody.velocity.X *= 1 - physicsBody.dragX * delta;
                physicsBody.velocity.Y *= 1 - physicsBody.dragY * delta;

                physicsBody.acceleration += physicsBody.Gravity;

                physicsBody.velocity += physicsBody.acceleration * delta;

                gameEntity.transform.position += physicsBody.velocity * delta;

                physicsBody.acceleration = Vector2.Zero;
            }
        }
    }
}