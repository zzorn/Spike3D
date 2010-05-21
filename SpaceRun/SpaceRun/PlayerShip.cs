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
    public class PlayerShip : Ship
    {
        private int currentWaypointIndex;

        public override void LogicUpdate(GameTime time, float t)
        {
            base.LogicUpdate(time, t);

            thrustVector_N = new Vector3(0, 0, GamePad.GetState(PlayerIndex.One).Triggers.Right);
            torqueThrustVector_N = new Vector3(GamePad.GetState(PlayerIndex.One).ThumbSticks.Left, 0);

            if (torqueThrustVector_N.LengthSquared() == 0)
            {
                KeyboardState ks = Keyboard.GetState();
                torqueThrustVector_N = new Vector3(
                    ks.IsKeyDown(Keys.Left) ? -0.1f : (ks.IsKeyDown(Keys.Right) ? 0.1f : 0),
                    ks.IsKeyDown(Keys.Down) ? -0.1f : (ks.IsKeyDown(Keys.Up) ? 0.1f : 0),
                    0);
            }
            if (thrustVector_N.LengthSquared() == 0 && Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                thrustVector_N = new Vector3(0, 0, 10);
            }

            List<Entity> waypoints = EntityManager.get().waypoints;
            if (currentWaypointIndex < waypoints.Count)
            {
                Entity waypoint = waypoints[currentWaypointIndex];
                if (Vector3.Distance(waypoint.position, position) < waypoint.radius_m)
                {
                    currentWaypointIndex++;
                }
            }

           RotationStabilization( t);

        }

    }
}