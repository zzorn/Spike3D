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
        public List<Entity> planets { get; protected set; }

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
            foreach (List<Entity> entityList in getEntityLists())
            {
                // Update all entities unless they're destroyed.
                List<Entity> removedEntities = new List<Entity>();
                foreach (Entity entity in entityList)
                {
                    if (entity.isDestroyed())
                    {
                        removedEntities.Add(entity);
                    }
                    else
                    {
                        entity.Update(gameTime);
                    }
                }
                // Remove all destroyed entities.
                foreach (Entity entity in removedEntities)
                {
                    entityList.Remove(entity);
                }
            }
        }

        public void renderEntities(GraphicsDeviceManager graphics)
        {
            foreach (List<Entity> entityList in getEntityLists())
            {
                foreach (Entity entity in entityList)
                {
                    entity.Render(graphics);
                }
            }
        }

        private IEnumerable<List<Entity>> getEntityLists()
        {
            yield return asteroids;
            yield return playerShips;
            yield return aiShips;
            yield return waypoints;
            yield return planets;
        }

        private EntityManager()
        {
            asteroids = new List<Entity>();
            playerShips = new List<Entity>();
            aiShips = new List<Entity>();
            waypoints = new List<Entity>();
            planets = new List<Entity>();
        }


    }
}