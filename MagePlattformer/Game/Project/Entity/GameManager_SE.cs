using System.Numerics;
using System.Collections.Generic;
using Raylib_cs;
using Animation;
using Physics;

namespace Engine
{
    public class GameManager : GameEntity
    {

        public GameManager()
        {
            name = "GameManager";

            Camera.zoom = 1.6f;

            AddComponent<GameManagerScript>(new GameManagerScript());

            EntityManager.SpawnEntity(new Player(), new Vector2(0, -5));
        }


    }
}