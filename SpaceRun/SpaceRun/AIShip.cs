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
   
        private const float WAKE_UP_RANGE = 500.0f;
        private Entity target;

        public AIShip()
        {
            maxThrust = 100.0f;
        }

        public override void LogicUpdate(GameTime time)
        {
            base.LogicUpdate(time);

            if (target == null)
            {
                target = getNearestPlayerShip(WAKE_UP_RANGE);
            }

            if (target != null)
            {
                Vector3 vectorToPlayer = target.position - position;
                vectorToPlayer.Normalize();
                thrustVector_N = vectorToPlayer * maxThrust;
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