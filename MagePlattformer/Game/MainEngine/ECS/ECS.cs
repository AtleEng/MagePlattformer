using System.Collections.Generic;
using System.Numerics;
using Engine;
using Physics;

namespace Engine
{
    [Serializable]
    public abstract class Component
    {
        public Component() { }

        public GameEntity gameEntity = new();

        public virtual void Start() { }
        public virtual void Update(float delta) { }
        public virtual void OnDestroy() { }
        public virtual void OnTrigger(Collider other) { }

        public virtual string PrintStats() { return ""; }
    }
    public class GameEntity
    {
        public GameEntity() { }

        public string name { get; set; } = "GameEntity";

        public bool isActive { get; set; } = true;
        public Transform worldTransform = new(Vector2.Zero, Vector2.One);
        public Transform transform { get; set; } = new(Vector2.Zero, Vector2.One);
        public List<Component> components { get; set; } = new();

        public GameEntity? parent;
        public List<GameEntity> children { get; set; } = new();
        public void OnTrigger(Collider other)
        {
            foreach (Component component in components)
            {
                component.OnTrigger(other);
            }
        }
        public virtual void OnInnit()
        {

        }
        public string PrintStats()
        {
            return $"isActive: {isActive}- -transform:{worldTransform.position},{worldTransform.size}-";
        }
        public bool HasComponent<T>()
        {
            foreach (Component c in components)
            {
                if (c.GetType() == typeof(T))
                {
                    return true;
                }
            }
            return false;
        }
        public T? GetComponent<T>() where T : Component
        {
            foreach (Component c in components)
            {
                if (c.GetType() == typeof(T))
                {
                    return (T)c;
                }
            }
            return null;
        }
        public void AddComponent<T>(Component component) where T : Component
        {
            component.gameEntity = this;
            components.Add(component);
        }
        public void RemoveComponent<T>()
        {
            foreach (Component c in components)
            {
                if (c.GetType() == typeof(T))
                {
                    components.Remove(c);
                    return;
                }
            }
        }
    }
    [Serializable]
    public class Transform
    {
        public Transform() { }
        public Transform(Vector2 position, Vector2 size)
        {
            this.position = position;
            this.size = size;
        }
        public Vector2 position { get; set; }
        public Vector2 size { get; set; }
    }
    public abstract class GameSystem
    {
        public List<Component> validComponents = new();
        public virtual void Start() { }
        public virtual void Update(float delta) { }
    }
    public interface IScript { }
}