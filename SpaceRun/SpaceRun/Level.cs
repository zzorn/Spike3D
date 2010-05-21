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
    public abstract class Level
    {
        private Random rand = new Random();

        public abstract void populateLevel();


        public void createWaypoint(Vector3 position)
        {
            Waypoint waypoint = new Waypoint();
            waypoint.radius_m = 1.0f;
            waypoint.position = position;
            EntityManager.get().waypoints.Add(waypoint);
        }

        public void createAsteroid(Vector3 position)
        {
            Entity entity = new Asteroid();
            entity.radius_m = 0.7f;
            entity.position = position;
            entity.mass_kg = 100000;
            entity.scale = (float)(rand.NextDouble() * 8 + 0.2f);
            entity.rotation = new Vector3((float)rand.NextDouble(), (float)rand.NextDouble(), (float)rand.NextDouble()) * 0.3f;
            entity.velocity = new Vector3((float)rand.NextDouble(), (float)rand.NextDouble(), (float)rand.NextDouble()) * 0.1f;
            entity.heading = new Quaternion((float)rand.NextDouble(), (float)rand.NextDouble(), (float)rand.NextDouble(), (float)rand.NextDouble());
            entity.model = ModelManager.get().getModel(rand.NextDouble() < 0.5 ? ModelType.ASTEROID : ModelType.ASTEROID2);
            EntityManager.get().asteroids.Add(entity);
        }

        public void createEnemyFighter(Vector3 position)
        {
            Ship entity = new AIShip();
            entity.radius_m = 1.0f;
            entity.scale = 5.0f;
            entity.position = position;
            entity.mass_kg = 10000;
            entity.shieldRechargeRate = 1.0f;
            entity.model = ModelManager.get().getModel(ModelType.ENEMY_FIGHTER);
            EntityManager.get().aiShips.Add(entity);
        }

        public void createPlanet(Vector3 position)
        {
            Entity entity = new Planet();
            entity.radius_m = 1000.0f;
            entity.scale = 1000.0f;
            entity.position = position;
            entity.mass_kg = 1e20f;
            entity.rotation = new Vector3((float)rand.NextDouble(), (float)rand.NextDouble(), (float)rand.NextDouble()) * 0.01f;
            entity.model = ModelManager.get().getModel(ModelType.PLANET);
            EntityManager.get().planets.Add(entity);
        }
    }
}