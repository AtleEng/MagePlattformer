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
                physicsBody.acceleration += physicsBody.Gravity;

                physicsBody.velocity += physicsBody.acceleration * delta;

                physicsBody.velocity *= 1 - physicsBody.drag;

                gameEntity.localTransform.position += physicsBody.velocity * delta;

                physicsBody.acceleration = Vector2.Zero;
            }
        }
    }
}