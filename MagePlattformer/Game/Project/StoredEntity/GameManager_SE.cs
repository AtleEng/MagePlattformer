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

            EntityManager.SpawnEntity(new Player(), new Vector2(0, -5));

            EntityManager.SpawnEntity(new Block(), new Vector2(0, 7), new Vector2(11, 3));
            EntityManager.SpawnEntity(new Block(), new Vector2(-3, -5), new Vector2(5, 5));
            EntityManager.SpawnEntity(new Block(), new Vector2(8, 2), new Vector2(9, 7));
        }
    }
}