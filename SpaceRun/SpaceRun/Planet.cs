using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    class Planet: Entity
    {
        public const double GravitationalConstant = 6.67428E-11;

        public Planet() 
        {
        }

        /**
         * Gravitational pull of the planet, calculatd from its mass and radius.
         * m/s2, that is, acceleration that nearby objects will have towards the planet.
         */ 
        public float gravitationalPull_m_per_s2 { get { return (float)(GravitationalConstant * mass_kg / (radius_m * radius_m)); } private set{} }

        public override void LogicUpdate(GameTime time)
        {
            // Planets dont do much :)
        }
    }
}
