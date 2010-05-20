using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;


namespace SpaceRun
{
    public class EntityManager
    {
        public List<Entity> asteroids { get; protected set; }
        public List<Entity> playerShips { get; protected set; }
        public List<Entity> aiShips { get; protected set; }
        public List<Entity> waypoints { get; protected set; }

        private static EntityManager instance;

        public static EntityManager get()
        {
            if (instance == null)
            {
                instance = new EntityManager();
            }
            return instance;
        }


        public void updateEntities(GameTime gameTime)
        {
            foreach (List<Entity> entityList in getNextEntityList())
            {
                foreach (Entity entity in entityList)
                {
                    entity.Update(gameTime);
                }
            }
        }

        public void renderEntities(GraphicsDeviceManager graphics)
        {
            foreach (List<Entity> entityList in getNextEntityList())
            {
                foreach (Entity entity in entityList)
                {
                    entity.Render(graphics);
                }
            }
        }
        private IEnumerable<List<Entity>> getNextEntityList()
        {
            yield return asteroids;
            yield return playerShips;
            yield return aiShips;
            yield return waypoints;
        }

        private EntityManager()
        {
            asteroids = new List<Entity>();
            playerShips = new List<Entity>();
            aiShips = new List<Entity>();
            waypoints = new List<Entity>();
        }


    }
}