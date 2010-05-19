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
    class Entity
    {
        private Vector3 position;
        private Vector3 velocity;
        private Vector3 acceleration;

        private Quaternion heading;
        private Quaternion rotation;
        private Quaternion torque;

        private Vector3 thrustVector;
        private Vector3 torqueThrustVector;

        public void Update(GameTime time)
        {
            LogicUpdate(time);

            // TODO: Get surrounding forces (planet gravitation etc)
            Vector3 surroundingForces = new Vector3();
            UpdatePhysics(time.ElapsedGameTime, surroundingForces);
            UpdateMovement(time.ElapsedGameTime);
        }

        /**
         * Does logic update for the entity (AI or player control, state changes, etc).
         * Can also update the own acceleration and torque of the entity.
         */
        public abstract void LogicUpdate(GameTime time);

        /**
         * Update movement based on physical forces.
         */ 
        public void UpdatePhysics(float time, Vector3 surroundingForces)
        {

        }

        /**
         * Calculates new position and heading for the entity.
         */ 
        public void UpdateMovement(float time)
        {
            velocity += acceleration * time;
            position += velocity * time;

            rotation += torque * time;
            heading += rotation * time;
        }
    }
}
