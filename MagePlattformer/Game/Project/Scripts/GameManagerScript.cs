using System.Numerics;
using System.Collections.Generic;
using Raylib_cs;
using Engine;
using Physics;
using Animation;

namespace Engine
{
    public class GameManagerScript : Component, IScript
    {

        public override void Start()
        {
            //LoadingManager.SaveLevel("Level1", _level1);

            LoadingManager.Load(1);
        }

        public override void Update(float delta)
        {
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_BACKSPACE))
            {
                LoadingManager.Load(2);
            }
        }

    }
}