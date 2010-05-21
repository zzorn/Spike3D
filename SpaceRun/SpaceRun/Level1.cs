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

    public class Level1 : Level
    {
        public override void populateLevel()
        {
            createWaypoint(new Vector3(100.0f, 200.0f, 150.0f));
            createWaypoint(new Vector3(300.0f, 200.0f, 150.0f));
            createWaypoint(new Vector3(1500.0f, 200.0f, 150.0f));

            Random rand = new Random();
            for (int i = 0; i < 400; i++)
            {
                createAsteroid(new Vector3(rand.Next(50) - 25, rand.Next(50) - 25, rand.Next(500) - 500));
            }
            for (int i = 0; i < 20; i++)
            {
                createEnemyFighter(new Vector3(rand.Next(50) - 25, rand.Next(50) - 25, rand.Next(500) - 500));
            }
            createPlanet(new Vector3(0, 0, -1000));

        }
    }
}