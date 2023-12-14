using System.Numerics;
using Raylib_cs;
using CoreEngine;
using Engine;
using System.ComponentModel;

namespace Physics
{
    public class PhysicsSystem : GameSystem
    {
        public override void Update(float delta)
        {
            if (Raylib.IsWindowResized()) { return; }
            foreach (GameEntity gameEntity in Core.activeGameEntities)
            {
                PhysicsBody? physicsBody = gameEntity.GetComponent<PhysicsBody>();
                Collider? collider = gameEntity.GetComponent<Collider>();
                if (physicsBody != null)
                {
                    UpdatePhysics(physicsBody, delta);

                    if (collider != null)
                    {
                        CheckCollision(gameEntity, collider, physicsBody);
                    }
                    Core.UpdateChildren(physicsBody.gameEntity.parent);
                }
                else
                if (collider != null)
                {
                    if (collider.isTrigger)
                    {
                        CheckCollision(gameEntity, collider, physicsBody);
                    }
                }

            }
        }
        void UpdatePhysics(PhysicsBody physicsBody, float delta)
        {
            if (physicsBody.physicsType == PhysicsBody.PhysicsType.staticType)
            {
                return;
            }
            // Apply drag separately to X and Y
            physicsBody.velocity.X *= 1 - physicsBody.dragX * delta;
            physicsBody.velocity.Y *= 1 - physicsBody.dragY * delta;

            physicsBody.acceleration += physicsBody.Gravity;

            physicsBody.velocity += physicsBody.acceleration * delta;

            physicsBody.gameEntity.transform.position += physicsBody.velocity * delta;

            physicsBody.acceleration = Vector2.Zero;
        }
        void CheckCollision(GameEntity gameEntity, Collider collider, PhysicsBody physicsBody)
        {
            collider.isColliding = false;

            Rectangle colliderBox = new Rectangle(
            gameEntity.worldTransform.position.X + collider.offset.X - collider.gameEntity.worldTransform.size.X * collider.scale.X / 2,
            gameEntity.worldTransform.position.Y + collider.offset.Y - collider.gameEntity.worldTransform.size.Y * collider.scale.Y / 2,
            gameEntity.worldTransform.size.X * collider.scale.X,
            gameEntity.worldTransform.size.Y * collider.scale.Y
            );

            foreach (GameEntity otherGameEntity in Core.activeGameEntities)
            {
                Collider? otherCollider = otherGameEntity.GetComponent<Collider>();
                if (otherCollider != null && otherCollider != collider)
                {
                    try
                    {
                        bool b = PhysicsSettings.collisionMatrix[collider.layer, otherCollider.layer];
                    }
                    catch (IndexOutOfRangeException)
                    {
                        System.Console.WriteLine($"Collision Layers: {collider.layer}, {otherCollider.layer} is out of range of the matrix (look in physics settings)");
                        throw;
                    }

                    if (PhysicsSettings.collisionMatrix[collider.layer, otherCollider.layer])
                    {
                        Rectangle otherColliderBox = new Rectangle(
                                        otherGameEntity.worldTransform.position.X + otherCollider.offset.X - otherGameEntity.worldTransform.size.X * otherCollider.scale.X / 2,
                                        otherGameEntity.worldTransform.position.Y + otherCollider.offset.Y - otherGameEntity.worldTransform.size.Y * otherCollider.scale.Y / 2,
                                        otherGameEntity.worldTransform.size.X * otherCollider.scale.X,
                                        otherGameEntity.worldTransform.size.Y * otherCollider.scale.Y
                                    );
                        if (Raylib.CheckCollisionRecs(colliderBox, otherColliderBox))
                        {
                            collider.isColliding = true;
                            if (collider.isTrigger)
                            {
                                collider.gameEntity.OnTrigger(otherCollider);
                            }
                            else if (physicsBody != null && !otherCollider.isTrigger)
                            {
                                Solve(collider, colliderBox, otherColliderBox, physicsBody);
                            }
                        }
                    }
                }
            }
        }
        void Detect()
        {

        }
        void Solve(Collider collider, Rectangle box, Rectangle otherBox, PhysicsBody physicsBody)
        {
            float xOverlap = Math.Min(box.X + box.Width, otherBox.X + otherBox.Width) - Math.Max(box.X, otherBox.X);
            float yOverlap = Math.Min(box.Y + box.Height, otherBox.Y + otherBox.Height) - Math.Max(box.Y, otherBox.Y);

            if (xOverlap < yOverlap)
            {
                int direction = Math.Sign(physicsBody.velocity.X);

                if (box.X < otherBox.X)
                {
                    collider.gameEntity.transform.position = new Vector2(otherBox.X - box.Width / 2, collider.gameEntity.transform.position.Y);
                }
                else
                {
                    collider.gameEntity.transform.position = new Vector2(otherBox.X + otherBox.Width + box.Width / 2, collider.gameEntity.transform.position.Y);
                }

                // Adjust position before inverting velocity
                physicsBody.velocity.X *= -direction * physicsBody.elasticity;
            }
            else
            {
                int direction = Math.Sign(physicsBody.velocity.Y);

                if (box.Y < otherBox.Y)
                {
                    collider.gameEntity.transform.position = new Vector2(collider.gameEntity.transform.position.X, otherBox.Y - box.Height / 2);
                }
                else
                {
                    collider.gameEntity.transform.position = new Vector2(collider.gameEntity.transform.position.X, otherBox.Y + otherBox.Height + box.Height / 2);
                }

                // Adjust position before inverting velocity
                physicsBody.velocity.Y *= -direction * physicsBody.elasticity;
            }
            Core.UpdateChildren(collider.gameEntity.parent);
        }
    }
    public static class PhysicsSettings
    {
        public static bool[,] collisionMatrix = new bool[4, 4]
        {
            //true = collide / false = ignore collision
            //player ground check enemy
            { false, true, false, true}, //player
            { true, false, true, true}, //ground
            { false, true, false, false}, //check
            { false, true, false, false} //enemy
        };
    }
}