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
        public Ship()
        {
            maxEnergy = 100;
            maxShield = 100;
            maxHull = 100;
            maxThrust = 10;
            maxTorqueThrust = 10;
            shieldRechargeRate = 5.0f;

            energy = maxEnergy;
            shield = maxShield;
            hull = maxHull;
        }

        public float energy { get; set; }
        public float hull { get; set; }
        public float shield { get; set; }

        public float maxEnergy { get; protected set; }
        public float maxHull { get; protected set; }
        public float maxShield { get; protected set; }
        public float shieldRechargeRate { get; set; }

        protected float maxThrust { get; set; }
        protected float maxTorqueThrust { get; set; }

        protected float stabilizerCounter; 


        public override void LogicUpdate(GameTime time, float t)
        {
            shield += shieldRechargeRate * t;
            if (shield > maxShield)
            {
                shield = maxShield;
            }
        }

        protected void Stabilization(float time)
        {
            /*
            Vector3 anti = new Vector3(rotation.X, rotation.Y, rotation.Z);
            anti.Normalize();
            torqueThrustVector_N -= anti * maxTorqueThrust * time;
            */
            stabilizerCounter += time;
            if (stabilizerCounter >= 0.1f)
            {
                rotation *= 0.95f;
                velocity *= 0.98f;
                stabilizerCounter -= 0.1f;
            }
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