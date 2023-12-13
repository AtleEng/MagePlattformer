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

            Camera.zoom = 1.5f;

            AddComponent<GameManagerScript>(new GameManagerScript());

            EntityManager.SpawnEntity(new Player(), new Vector2(0, -5));

            EntityManager.SpawnEntity(new JumpPad(), new Vector2(5, 4));

            EntityManager.SpawnEntity(new JumpingEnemy(), new Vector2(8, -5));
            EntityManager.SpawnEntity(new WalkEnemy(), new Vector2(5, 0));
            EntityManager.SpawnEntity(new RandomEnemy(), new Vector2(8, -6));
        }


    }
}