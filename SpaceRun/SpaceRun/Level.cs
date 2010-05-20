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
        public abstract void populateLevel();

        public void createWaypoint(Vector3 position)
        {
            Waypoint waypoint = new Waypoint();
            waypoint.radius_m = 10.0f;
            waypoint.position = position;
            EntityManager.get().waypoints.Add(waypoint);
        }
    }
}