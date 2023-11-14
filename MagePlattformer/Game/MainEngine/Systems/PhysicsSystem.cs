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
                Rectangle colliderBox = new Rectangle(
                gameEntity.transform.position.X + collider.origin.X - collider.gameEntity.transform.size.X * collider.size.X / 2,
                gameEntity.transform.position.Y + collider.origin.Y - collider.gameEntity.transform.size.Y * collider.size.Y / 2,
                gameEntity.transform.size.X * collider.size.X,
                gameEntity.transform.size.Y * collider.size.Y
                );

                foreach (GameEntity otherGameEntity in Core.activeGameEntities)
                {
                    Collider? otherCollider = otherGameEntity.components.ContainsKey(typeof(Collider)) ? otherGameEntity.components[typeof(Collider)] as Collider : null;
                    if (otherCollider != null && otherCollider != collider)
                    {
                        Rectangle otherColliderBox = new Rectangle(
                            otherGameEntity.transform.position.X + otherCollider.origin.X - otherGameEntity.transform.size.X * otherCollider.size.X / 2,
                            otherGameEntity.transform.position.Y + otherCollider.origin.Y - otherGameEntity.transform.size.Y * otherCollider.size.Y / 2,
                            otherGameEntity.transform.size.X * otherCollider.size.X,
                            otherGameEntity.transform.size.Y * otherCollider.size.Y
                        );

                        if (Raylib.CheckCollisionRecs(colliderBox, otherColliderBox) && hasPhysics)
                        {
                            UpdateCollision(collider, otherCollider, colliderBox, otherColliderBox, physicsBody);
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
        if (collider.isTrigger || otherCollider.isTrigger)
        {
            collider.gameEntity.OnTrigger();
            return;
        }
        float xOverlap = Math.Min(box.x + box.width, otherBox.x + otherBox.width) - Math.Max(box.x, otherBox.x);
        float yOverlap = Math.Min(box.y + box.height, otherBox.y + otherBox.height) - Math.Max(box.y, otherBox.y);

        if (xOverlap < yOverlap)
        {
            if (box.x < otherBox.x)
            {
                collider.gameEntity.transform.position.X = otherBox.x - box.width / 2;
            }
            else
            {
                collider.gameEntity.transform.position.X = otherBox.x + otherBox.width + box.width / 2;
            }
            physicsBody.velocity.X *= -1 * physicsBody.elasticity;
        }
        else
        {
            if (box.y < otherBox.y)
            {
                collider.gameEntity.transform.position.Y = otherBox.y - box.height / 2;
            }
            else
            {
                collider.gameEntity.transform.position.Y = otherBox.y + otherBox.height + box.height / 2;
            }
            physicsBody.velocity.Y *= -1 * physicsBody.elasticity;
        }
    }
}