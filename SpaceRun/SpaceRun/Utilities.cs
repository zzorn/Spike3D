using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceRun
{
    public class Utilities
    {
        /// <summary> 
        /// The function converts a Microsoft.Xna.Framework.Quaternion into a Microsoft.Xna.Framework.Vector3 
        /// </summary> 
        /// <param name="q">The Quaternion to convert</param> 
        /// <returns>An equivalent Vector3</returns> 
        /// <remarks> 
        /// This function was extrapolated by reading the work of Martin John Baker. All credit for this function goes to Martin John. 
        /// http://www.euclideanspace.com/maths/geometry/rotations/conversions/quaternionToEuler/index.htm 
        /// </remarks> 
        public static Vector3 QuaternionToEuler(Quaternion q)
        {
            Vector3 v = new Vector3();

            v.X = (float)Math.Atan2
            (
                2 * q.Y * q.W - 2 * q.X * q.Z,
                1 - 2 * Math.Pow(q.Y, 2) - 2 * Math.Pow(q.Z, 2)
            );

            v.Y = (float)Math.Asin
            (
                2 * q.X * q.Y + 2 * q.Z * q.W
            );

            v.Z = (float)Math.Atan2
            (
                2 * q.X * q.W - 2 * q.Y * q.Z,
                1 - 2 * Math.Pow(q.X, 2) - 2 * Math.Pow(q.Z, 2)
        );

            if (q.X * q.Y + q.Z * q.W == 0.5)
            {
                v.X = (float)(2 * Math.Atan2(q.X, q.W));
                v.Z = 0;
            }

            else if (q.X * q.Y + q.Z * q.W == -0.5)
            {
                v.X = (float)(-2 * Math.Atan2(q.X, q.W));
                v.Z = 0;
            }

            return v;
        }
    }
}
