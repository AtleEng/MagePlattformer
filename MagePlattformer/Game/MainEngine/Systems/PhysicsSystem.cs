using System.Numerics;
using Raylib_cs;
using CoreEngine;
using Engine;
using Physics;
public class PhysicsSystem : GameSystem
{
    public override void Update(float delta)
    {
        foreach (GameEntity gameEntity in Core.activeGameEntities)
        {
            bool hasPhysics = false;
            PhysicsBody? physicsBody = gameEntity.components.ContainsKey(typeof(PhysicsBody)) ? gameEntity.components[typeof(PhysicsBody)] as PhysicsBody : null;

            if (physicsBody != null)
            {
                hasPhysics = true;
                UpdatePhysics(physicsBody, delta);
            }

            Collider? collider = gameEntity.components.ContainsKey(typeof(Collider)) ? gameEntity.components[typeof(Collider)] as Collider : null;
            if (collider != null)
            {
                collider.isColliding = false;

                Rectangle colliderBox = new Rectangle(
                gameEntity.worldTransform.position.X + collider.origin.X - collider.gameEntity.worldTransform.size.X * collider.size.X / 2,
                gameEntity.worldTransform.position.Y + collider.origin.Y - collider.gameEntity.worldTransform.size.Y * collider.size.Y / 2,
                gameEntity.worldTransform.size.X * collider.size.X,
                gameEntity.worldTransform.size.Y * collider.size.Y
                );

                foreach (GameEntity otherGameEntity in Core.activeGameEntities)
                {
                    Collider? otherCollider = otherGameEntity.components.ContainsKey(typeof(Collider)) ? otherGameEntity.components[typeof(Collider)] as Collider : null;
                    if (otherCollider != null && otherCollider != collider)
                    {
                        Rectangle otherColliderBox = new Rectangle(
                            otherGameEntity.worldTransform.position.X + otherCollider.origin.X - otherGameEntity.worldTransform.size.X * otherCollider.size.X / 2,
                            otherGameEntity.worldTransform.position.Y + otherCollider.origin.Y - otherGameEntity.worldTransform.size.Y * otherCollider.size.Y / 2,
                            otherGameEntity.worldTransform.size.X * otherCollider.size.X,
                            otherGameEntity.worldTransform.size.Y * otherCollider.size.Y
                        );
                        if (Raylib.CheckCollisionRecs(colliderBox, otherColliderBox))
                        {
                            collider.isColliding = true;
                            if (collider.isTrigger)
                            {
                                collider.gameEntity.OnTrigger();
                                System.Console.WriteLine("Trigger");
                            }
                            else if (hasPhysics && !otherCollider.isTrigger)
                            {
                                UpdateCollision(collider, otherCollider, colliderBox, otherColliderBox, physicsBody);
                            }
                        }
                    }
                }
            }
        }
    }
    void UpdatePhysics(PhysicsBody physicsBody, float delta)
    {
        // Apply drag separately to X and Y
        physicsBody.velocity.X *= 1 - physicsBody.dragX * delta;
        physicsBody.velocity.Y *= 1 - physicsBody.dragY * delta;

        physicsBody.acceleration += physicsBody.Gravity;

        physicsBody.velocity += physicsBody.acceleration * delta;

        physicsBody.gameEntity.transform.position += physicsBody.velocity * delta;

        physicsBody.acceleration = Vector2.Zero;
    }

    void UpdateCollision(Collider collider, Collider otherCollider, Rectangle box, Rectangle otherBox, PhysicsBody physicsBody)
    {
        if (physicsBody == null)
        {
            return;
        }
        float xOverlap = Math.Min(box.X + box.Width, otherBox.X + otherBox.Width) - Math.Max(box.X, otherBox.X);
        float yOverlap = Math.Min(box.Y + box.Height, otherBox.Y + otherBox.Height) - Math.Max(box.Y, otherBox.Y);

        if (xOverlap < yOverlap)
        {
            if (box.X < otherBox.X)
            {
                collider.gameEntity.transform.position.X = otherBox.X - box.Width / 2;
            }
            else
            {
                collider.gameEntity.transform.position.X = otherBox.X + otherBox.Width + box.Width / 2;
            }
            physicsBody.velocity.X *= -1 * physicsBody.elasticity;
        }
        else
        {
            if (box.Y < otherBox.Y)
            {
                collider.gameEntity.transform.position.Y = otherBox.Y - box.Height / 2;
            }
            else
            {
                collider.gameEntity.transform.position.Y = otherBox.Y + otherBox.Height + box.Height / 2;
            }
            physicsBody.velocity.Y *= -1 * physicsBody.elasticity;
        }
    }
}