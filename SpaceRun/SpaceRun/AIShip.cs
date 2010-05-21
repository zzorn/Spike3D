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
    public class AIShip : Ship
    {
        private const float WAKE_UP_RANGE = 20000.0f;
        private const float AVOIDANCE_RANGE = 200.0f;

        private Entity target;

        public AIShip()
        {
            maxThrust = 100.0f;
        }

        public override void LogicUpdate(GameTime time, float t)
        {
            base.LogicUpdate(time, t);

            thrustVector_N = Vector3.Zero;

            if (target == null)
            {
                target = getNearestPlayerShip(WAKE_UP_RANGE);
            }

            if (target != null)
            {
                Vector3 vectorToPlayer = target.position - position;
                if (vectorToPlayer.Length() < AVOIDANCE_RANGE)
                {
                    vectorToPlayer *= -1;
                }
                vectorToPlayer.Normalize();
                thrustVector_N = vectorToPlayer * maxThrust;
            }

            Vector3 flockingVector = Vector3.Zero;
            Vector3 avoidanceVector = Vector3.Zero;
            foreach (Entity otherShip in EntityManager.get().aiShips)
            {
                float distance = Vector3.Distance(position, otherShip.position);
                if (distance > 1.0f)
                {
                    if (distance < WAKE_UP_RANGE)
                    {
                        int directionMultiplier = 1;
                        if (distance < AVOIDANCE_RANGE)
                        {
                            directionMultiplier = -1;
                        }
                        flockingVector += directionMultiplier * Vector3.One / (otherShip.position - position);
                    }
                }
            }
            if (flockingVector.LengthSquared() != 0)
            {
                flockingVector.Normalize();
                thrustVector_N += flockingVector * maxThrust;
            }
        }

        private Entity getNearestPlayerShip(float maxDistance)
        {
            float nearestDistance = maxDistance;
            Entity nearestPlayer = null;
            foreach (Entity player in EntityManager.get().playerShips)
            {
                float distance = Vector3.Distance(position, player.position);
                if (distance <= nearestDistance)
                {
                    nearestDistance = distance;
                    nearestPlayer = player;
                }
            }
            return nearestPlayer;
        }
    }
}