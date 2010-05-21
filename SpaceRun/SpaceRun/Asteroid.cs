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
    public class Asteroid : Entity
    {
        public override void LogicUpdate(GameTime time, float t)
        {
            checkCollisions(t, EntityManager.get().playerShips);
            checkCollisions(t, EntityManager.get().aiShips);
        }

        private void checkCollisions(float t, List<Entity> entities)
        {
            foreach (Entity entity in entities)
            {
                if (Vector3.Distance(position, entity.position + entity.velocity * t) < radius_m * scale + entity.radius_m * entity.scale)
                {
                    Ship ship = ((Ship)entity);

                    ship.takeDamage(ship.velocity.Length() * 2);
                    ship.velocity = -entity.velocity * 0.5f;
                    ship.acceleration = Vector3.Zero;
                }
            }
        }
    }
}