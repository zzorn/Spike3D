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
    abstract public class Entity
    {
        public  Entity()
        {
            id = 0;
            position = Vector3.Zero;
            velocity = Vector3.Zero;
            acceleration = Vector3.Zero;
            heading = Quaternion.Identity;
            rotation = Vector3.Zero;
            torque = Vector3.Zero;
            mass_kg = 1;
            radius_m = 1;
            thrustVector_N = Vector3.Zero;
            torqueThrustVector_N = Vector3.Zero;
        }

        public long id { get; set; }

        public Vector3 position { get; set; }
        public Vector3 velocity { get; set; }
        public Vector3 acceleration { get; set; }

        public Quaternion heading { get; set; }
        public Vector3 rotation { get; set; }
        public Vector3 torque { get; set; }

        public float mass_kg { get; set; }
        public float radius_m { get; set; }
        public float rotationalInertia { get { return (2 * mass_kg * radius_m * radius_m) / 5.0f; } private set{} } // Assumes solid sphere approximation
        public Vector3 thrustVector_N { get; set; }
        public Vector3 torqueThrustVector_N { get; set; }

        public Model model {get; set; }

        /**
         * Updates AI, player input, game logic, physics, movement, etc.
         */ 
        public void Update(GameTime time)
        {
            // Logic
            LogicUpdate(time);

            // Physics
            // TODO: Get surrounding forces (planet gravitation etc)
            Vector3 environmentForces_N = new Vector3();
            UpdatePhysics((float)time.ElapsedGameTime.TotalSeconds, environmentForces_N);

            // Movement
            UpdateMovement((float)time.ElapsedGameTime.TotalSeconds);
        }

        /**
         * Render the entity to the specified context.
         */
        public virtual void Render(GraphicsDeviceManager graphics)
        {
            // TODO: Render model
        }

        /**
         * Does logic update for the entity (AI or player control, state changes, etc).
         * Can also update the own acceleration and torque of the entity.
         */
        public abstract void LogicUpdate(GameTime time);

        /**
         * Update movement based on physical forces.
         */ 
        public void UpdatePhysics(float time, Vector3 environmentForces_N)
        {
            // Thrust is in local coordinate space, so rotate it with the heading
            Matrix headingMatrix = Matrix.CreateFromQuaternion(heading);
            Vector3 rotatedThrust_N = Vector3.Transform(thrustVector_N, headingMatrix);

            // Calculate acceleration
            Vector3 forces_N = rotatedThrust_N + environmentForces_N;
            acceleration = forces_N / mass_kg;

            // Turning
            torque = torqueThrustVector_N / rotationalInertia;
        }

        /**
         * Calculates new position and heading for the entity.
         */ 
        public void UpdateMovement(float time)
        {
            velocity += acceleration * time;
            position += velocity * time;

            rotation += torque * time;

            Vector3 r = rotation * time; 
            heading += Quaternion.CreateFromYawPitchRoll(r.X, r.Y, r.Z);
        }

        /**
         * Ships and other destroyable entities can overwrite this.
         * Entity manager will then know to remove it from the list.
         */
        public virtual bool isDestroyed()
        {
            return false;
        }
    }
}
