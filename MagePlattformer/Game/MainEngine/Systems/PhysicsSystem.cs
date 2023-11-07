using System.Drawing;
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

            Collider? collider = gameEntity.components.ContainsKey(typeof(Collider)) ? gameEntity.components[typeof(Collider)] as Collider : null;
            if (collider != null && physicsBody != null)
            {
                foreach (GameEntity otherGameEntity in Core.activeGameEntities)
                {
                    Collider? otherCollider = otherGameEntity.components.ContainsKey(typeof(Collider)) ? otherGameEntity.components[typeof(Collider)] as Collider : null;
                    if (otherCollider != null && otherCollider != collider)
                    {
                        // Calculate the overlap on both X and Y axes
                       float xOverlap = Math.Min(gameEntity.transform.position.X + collider.width, otherGameEntity.transform.position.X + otherCollider.width) - Math.Max(gameEntity.transform.position.X, otherGameEntity.transform.position.X);
                        float yOverlap = Math.Min(gameEntity.transform.position.Y + collider.height, otherGameEntity.transform.position.Y + otherCollider.height) - Math.Max(otherGameEntity.transform.position.Y, otherGameEntity.transform.position.Y);

                        // Determine the axis of minimum overlap
                        if (xOverlap < yOverlap)
                        {
                            // If the overlap is smaller on the X-axis, resolve the collision along the X-axis
                            if (gameEntity.transform.position.X < otherGameEntity.transform.position.X)
                            {
                                gameEntity.transform.position.X = otherGameEntity.transform.position.X - collider.width;
                            }
                            else
                            {
                                gameEntity.transform.position.X = otherGameEntity.transform.position.X + otherCollider.width;
                            }
                        }
                        else
                        {
                            // If the overlap is smaller on the Y-axis, resolve the collision along the Y-axis
                            if (gameEntity.transform.position.Y < otherGameEntity.transform.position.Y)
                            {
                                gameEntity.transform.position.Y = otherGameEntity.transform.position.Y - collider.height;
                            }
                            else
                            {
                                gameEntity.transform.position.Y = otherGameEntity.transform.position.Y + otherCollider.height;
                            }
                        }
                    }
                }
            }
        }
    }
}