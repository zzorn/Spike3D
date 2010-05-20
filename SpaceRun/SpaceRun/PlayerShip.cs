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

        public override void LogicUpdate(GameTime time)
        {
            thrustVector_N = new Vector3(0, 0, GamePad.GetState(PlayerIndex.One).Triggers.Right);
            torqueThrustVector_N = new Vector3(GamePad.GetState(PlayerIndex.One).ThumbSticks.Left, 0);

            Entity waypoint = EntityManager.get().waypoints[currentWaypointIndex];
        }

    }
}