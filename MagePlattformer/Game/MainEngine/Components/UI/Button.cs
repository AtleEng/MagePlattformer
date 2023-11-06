using System.Numerics;
using System.Collections.Generic;
using Raylib_cs;
using CoreEngine;
using Engine;

namespace UI
{
    public class Button : Component, IScript
    {
        Action OnKilcked;
        public bool isHovering;
        public Button(Action KlickAction)
        {
            OnKilcked = KlickAction;
        }
        public override void Update(float delta)
        {
            Vector2 mPos = WorldSpace.GetVirtualMousePos();
            if (Raylib.CheckCollisionPointRec
                (mPos, new Rectangle(gameEntity.worldTransform.position.X - gameEntity.worldTransform.size.X / 2,
                gameEntity.worldTransform.position.Y - gameEntity.worldTransform.size.Y / 2,
                gameEntity.worldTransform.size.X, gameEntity.worldTransform.size.Y)))
            {
                isHovering = true;
                if (Raylib.IsMouseButtonDown(0))
                {
                    OnKilcked();
                }
            }
            else { isHovering = false; }
        }
    }
}