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
    public class Ship : Entity
    {
        public float hull { get; protected set; }
        public float shield { get; protected set; }
        
        protected float maxVelocity { get; set; }
        protected float maxThrust { get; set; }

        public Ship()
        {
            hull = 1.0f;
            shield = 1.0f;
        }

        public override void LogicUpdate(GameTime time)
        {
            shield += (float)time.ElapsedGameTime.TotalSeconds / 10.0f;
            if (shield > 1)
            {
                shield = 1;
            }
        }

        public override void Render(GraphicsDeviceManager graphics)
        {
        }

        public void takeDamage(float damageAmount)
        {
            if (shield >= 0)
            {
                if (shield >= damageAmount)
                {
                    shield -= damageAmount;
                    damageAmount = 0;
                }
                else
                {
                    damageAmount -= shield;
                    shield = 0;
                }
            }

            if (damageAmount > 0)
            {
                hull -= damageAmount;
            }
        }

        public override bool isDestroyed()
        {
            return hull <= 0;
        }
    }

}