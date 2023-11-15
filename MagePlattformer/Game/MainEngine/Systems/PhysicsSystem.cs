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
                        System.Console.WriteLine(Raylib.GetCollisionRec(colliderBox, otherColliderBox).Width + " " + Raylib.GetCollisionRec(colliderBox, otherColliderBox).Height);
                        if (Raylib.CheckCollisionRecs(colliderBox, otherColliderBox) && hasPhysics)
                        {
                            System.Console.WriteLine("!!!");
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
        if (physicsBody == null)
        {
            System.Console.WriteLine("Return");
            return;
        }
        if (collider.isTrigger || otherCollider.isTrigger)
        {
            collider.gameEntity.OnTrigger();
            System.Console.WriteLine("Trigger");
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
        System.Console.WriteLine(xOverlap + " " + yOverlap);
    }
}