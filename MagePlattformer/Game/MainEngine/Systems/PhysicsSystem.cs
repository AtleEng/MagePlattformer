using System.Numerics;
using Raylib_cs;
using CoreEngine;
using Engine;


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

            List<Rectangle> otherAabbs = new();
            List<Rectangle> collisionRecs = new();

            Rectangle aabb = GetCollisionRectangleFromCollider(gameEntity, collider);

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
                        Rectangle otherAabb = GetCollisionRectangleFromCollider(otherGameEntity, otherCollider);

                        Rectangle collisonRec = GetCollisionRec(aabb, otherAabb);
                        float area = collisonRec.X * collisonRec.Y;
                        if (area != 0)
                        {
                            collider.isColliding = true;
                            if (collider.isTrigger)
                            {
                                collider.gameEntity.OnTrigger(otherCollider);
                            }
                            else if (physicsBody != null && !otherCollider.isTrigger)
                            {
                                otherAabbs.Add(otherAabb);
                                collisionRecs.Add(collisonRec);
                            }
                        }
                    }
                }
            }
            if (otherAabbs.Count > 0)
            {
                //sorterar collider med hur stor deras area är
                SortOtherAabbsByArea(otherAabbs, collisionRecs);

                for (int i = 0; i < otherAabbs.Count; i++)
                {
                    float xOverlap = Math.Min(aabb.X + aabb.Width, otherAabbs[i].X + otherAabbs[i].Width) - Math.Max(aabb.X, otherAabbs[i].X);
                    float yOverlap = Math.Min(aabb.Y + aabb.Height, otherAabbs[i].Y + otherAabbs[i].Height) - Math.Max(aabb.Y, otherAabbs[i].Y);

                    if (xOverlap < yOverlap)
                    {
                        int direction = Math.Sign(physicsBody.velocity.X);

                        if (aabb.X < otherAabbs[i].X)
                        {
                            collider.gameEntity.transform.position = new Vector2(otherAabbs[i].X - aabb.Width / 2, collider.gameEntity.transform.position.Y);
                        }
                        else
                        {
                            collider.gameEntity.transform.position = new Vector2(otherAabbs[i].X + otherAabbs[i].Width + aabb.Width / 2, collider.gameEntity.transform.position.Y);
                        }

                        // Adjust position before inverting velocity
                        physicsBody.velocity.X *= -direction * physicsBody.elasticity;
                    }
                    else
                    {
                        int direction = Math.Sign(physicsBody.velocity.Y);

                        if (aabb.Y < otherAabbs[i].Y)
                        {
                            collider.gameEntity.transform.position = new Vector2(collider.gameEntity.transform.position.X, otherAabbs[i].Y - aabb.Height / 2);
                        }
                        else
                        {
                            collider.gameEntity.transform.position = new Vector2(collider.gameEntity.transform.position.X, otherAabbs[i].Y + otherAabbs[i].Height + aabb.Height / 2);
                        }

                        // Adjust position before inverting velocity
                        physicsBody.velocity.Y *= -direction * physicsBody.elasticity;
                    }
                    //update all collider to new pos
                    for (int j = i; j < otherAabbs.Count; j++)
                    {
                        float newXOverlap = Math.Min(otherAabbs[i].X + otherAabbs[i].Width, otherAabbs[j].X + otherAabbs[j].Width) - Math.Max(otherAabbs[i].X, otherAabbs[j].X);
                        float newYOverlap = Math.Min(otherAabbs[i].Y + otherAabbs[i].Height, otherAabbs[j].Y + otherAabbs[j].Height) - Math.Max(otherAabbs[i].Y, otherAabbs[j].Y);

                        if (newXOverlap < newYOverlap)
                        {
                            int direction = Math.Sign(physicsBody.velocity.X);

                            if (otherAabbs[i].X < otherAabbs[j].X)
                            {
                                otherAabbs[j] = new Rectangle(otherAabbs[j].X - otherAabbs[i].Width / 2, otherAabbs[j].Y, otherAabbs[j].Width, otherAabbs[j].Height);
                            }
                            else
                            {
                                otherAabbs[j] = new Rectangle(otherAabbs[j].X + otherAabbs[i].Width / 2, otherAabbs[j].Y, otherAabbs[j].Width, otherAabbs[j].Height);
                            }
                        }
                        else
                        {
                            int direction = Math.Sign(physicsBody.velocity.Y);

                            if (otherAabbs[i].Y < otherAabbs[j].Y)
                            {
                                otherAabbs[j] = new Rectangle(otherAabbs[j].X, otherAabbs[j].Y - otherAabbs[i].Height / 2, otherAabbs[j].Width, otherAabbs[j].Height);
                            }
                            else
                            {
                                otherAabbs[j] = new Rectangle(otherAabbs[j].X, otherAabbs[j].Y + otherAabbs[i].Height / 2, otherAabbs[j].Width, otherAabbs[j].Height);
                            }
                        }
                    }
                }
                Core.UpdateChildren(collider.gameEntity.parent);
            }
        }
        static Rectangle GetCollisionRectangleFromCollider(GameEntity entity, Collider collider)
        {
            return new Rectangle
            (
                entity.worldTransform.position.X + collider.offset.X - entity.worldTransform.size.X * collider.scale.X / 2,
                entity.worldTransform.position.Y + collider.offset.Y - entity.worldTransform.size.Y * collider.scale.Y / 2,
                entity.worldTransform.size.X * collider.scale.X,
                entity.worldTransform.size.Y * collider.scale.Y
            );
        }
        static public Rectangle GetCollisionRec(Rectangle rec, Rectangle other)
        {
            float x1 = Math.Max(rec.X, other.X);
            float y1 = Math.Max(rec.Y, other.Y);

            float x2 = Math.Min(rec.X + rec.Width, other.X + other.Width);
            float y2 = Math.Min(rec.Y + rec.Height, other.Y + other.Height);

            // Check for no-overlap conditions
            if (x1 >= x2 || y1 >= y2)
            {
                return new Rectangle(); // No collision
            }
            return new Rectangle(x1, y1, x2 - x1, y2 - y1);
        }
        static void SortOtherAabbsByArea(List<Rectangle> otherAabbs, List<Rectangle> collisionRecs)
        {
            // Check if the lists have the same length
            if (otherAabbs.Count != collisionRecs.Count)
            {
                throw new ArgumentException("Lists must have the same length dummy, i dont even know how you make this error happen");
            }

            // Create a comparer that compares rectangles by area in descending order
            var comparer = Comparer<Rectangle>.Create((rect1, rect2) =>
            {
                float area1 = rect1.Width * rect1.Height;
                float area2 = rect2.Width * rect2.Height;
                return area2.CompareTo(area1); // Compare in descending order
            });

            // Sort otherAabbs using the comparer
            otherAabbs.Sort((a, b) =>
            {
                int indexA = otherAabbs.IndexOf(a);
                int indexB = otherAabbs.IndexOf(b);
                return comparer.Compare(collisionRecs[indexA], collisionRecs[indexB]);
            });
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
