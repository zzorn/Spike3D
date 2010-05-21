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
                    ks.IsKeyDown(Keys.Left) ? 0.1f : (ks.IsKeyDown(Keys.Right) ? -0.1f : 0),
                    ks.IsKeyDown(Keys.Down) ? -0.1f : (ks.IsKeyDown(Keys.Up) ? 0.1f : 0),
                    0);
            }
            if (thrustVector_N.LengthSquared() == 0 && Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                thrustVector_N = new Vector3(0, 0, -1000);
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

            RotationStabilization(t);
        }

        public void DrawDebug(SpriteBatch spriteBatch, SpriteFont font)
        {
            Vector3 playerShipEulerAngles = QuaternionToEuler(heading);

            spriteBatch.DrawString(
                    font,
                    "PlayerShip position[" + Math.Round(position.X) + "," + Math.Round(position.Y) + "," + Math.Round(position.Z) + "]\n" +
                    "PlayerShip headAngs[" + Math.Round(MathHelper.ToDegrees(playerShipEulerAngles.X)) + "," + Math.Round(MathHelper.ToDegrees(playerShipEulerAngles.Y)) + "," + Math.Round(MathHelper.ToDegrees(playerShipEulerAngles.Z)) + "]\n" +
                    "PlayerShip velocity[" + Math.Round(velocity.X) + "," + Math.Round(velocity.Y) + "," + Math.Round(velocity.Z) + "]",
                    new Vector2(10.0f, 10.0f),
                    Color.Yellow);
        }

        /// <summary> 
        /// The function converts a Microsoft.Xna.Framework.Quaternion into a Microsoft.Xna.Framework.Vector3 
        /// </summary> 
        /// <param name="q">The Quaternion to convert</param> 
        /// <returns>An equivalent Vector3</returns> 
        /// <remarks> 
        /// This function was extrapolated by reading the work of Martin John Baker. All credit for this function goes to Martin John. 
        /// http://www.euclideanspace.com/maths/geometry/rotations/conversions/quaternionToEuler/index.htm 
        /// </remarks> 
        private Vector3 QuaternionToEuler(Quaternion q) 
        { 
            Vector3 v = new Vector3(); 
 
            v.X = (float)Math.Atan2 
            ( 
                2 * q.Y * q.W - 2 * q.X * q.Z,  
                1 - 2*Math.Pow(q.Y, 2) - 2*Math.Pow(q.Z, 2) 
            ); 
 
            v.Y = (float)Math.Asin 
            ( 
                2*q.X*q.Y + 2*q.Z*q.W 
            ); 
 
            v.Z = (float)Math.Atan2 
            ( 
                2*q.X*q.W-2*q.Y*q.Z, 
                1 - 2*Math.Pow(q.X, 2) - 2*Math.Pow(q.Z, 2) 
        ); 
 
            if(q.X*q.Y + q.Z*q.W == 0.5) 
            { 
                v.X = (float)(2 * Math.Atan2(q.X,q.W)); 
                v.Z = 0;     
            } 
 
            else if(q.X*q.Y + q.Z*q.W == -0.5) 
            { 
                v.X = (float)(-2 * Math.Atan2(q.X, q.W)); 
                v.Z = 0; 
            } 
 
            return v; 
        }
    }
}