using System.Numerics;

using CoreEngine;
using Engine;

namespace CoreEngine
{
    public class ScriptSystem : GameSystem
    {
        public override void Start()
        {
            foreach (GameEntity gameEntity in Core.activeGameEntities)
            {
                foreach (Component component in gameEntity.components)
                {
                    if (component is IScript)
                    {
                        component.Start();
                    }
                }
            }
        }
        public override void Update(float delta)
        {
            foreach (GameEntity gameEntity in Core.gameEntities)
            {
                foreach (Component component in gameEntity.components)
                {
                    if (component is IScript)
                    {
                        component.Update(delta);
                    }
                }
            }
        }
    }
}