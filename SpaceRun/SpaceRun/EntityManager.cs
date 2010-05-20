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
        public List<Entity> playersShips { get; protected set; }
        public List<Entity> aiShips { get; protected set; }

        private static EntityManager instance;

        public static EntityManager get()
        {
            if (instance != null)
            {
                instance = new EntityManager();
            }
            return instance;
        }

        private EntityManager()
        {
            asteroids = new List<Entity>();
            playersShips = new List<Entity>();
            aiShips = new List<Entity>();
        }


    }
}