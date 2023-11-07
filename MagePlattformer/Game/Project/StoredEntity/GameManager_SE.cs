using System.Numerics;
using System.Collections.Generic;
using Raylib_cs;

namespace Engine
{
    public class GameManager : GameEntity
    {
        public GameManager()
        {
            name = "GameManager";

            EntityManager.SpawnEntity(new Player(), Vector2.Zero);

            EntityManager.SpawnEntity(new Block(), new Vector2(0,7), new Vector2(10,3));
        }
    }
}