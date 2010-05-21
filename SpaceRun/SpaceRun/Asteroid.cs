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
            foreach (Entity player in EntityManager.get().playerShips)
            {
                if (Vector3.Distance(position, player.position) < radius_m)
                {
                    Ship playerShip = ((Ship)player);

                    playerShip.takeDamage(playerShip.velocity.Length() / 1000);
                    playerShip.velocity = -player.velocity * 0.1f;
                    playerShip.acceleration = Vector3.Zero;

                }
            }
        }
    }
}