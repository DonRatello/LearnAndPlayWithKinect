using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace LearnAndPlayWithKinect.Other
{
    class PointerAdjustment
    {
        //dla 640x480
        private const float SkeletonMaxX = 0.6f;
        private const float SkeletonMaxY = 0.4f;

        private static float Adjust(int primaryScreenResolution, float maxJointPosition, float jointPosition)
        {
            var value = (((((float)primaryScreenResolution) / maxJointPosition) / 2f) * jointPosition)
                + (primaryScreenResolution / 2);

            if (value > primaryScreenResolution || value < 0f) return 0f;

            return value;
        }

        /// <summary>
        /// Wez aktualna pozycje jointa i dopasuj do rozdzielczosci ekranu
        /// </summary>
        /// <param name="joint">Joint to Adjust</param>
        /// <returns></returns>
        public static Microsoft.Kinect.Vector4 AdjustToScreen(Joint joint)
        {
            var newVector = new Microsoft.Kinect.Vector4
            {
                X = Adjust(Settings.WINDOW_WIDTH, SkeletonMaxX, joint.Position.X),
                Y = Adjust(Settings.WINDOW_HEIGHT, SkeletonMaxY, -joint.Position.Y),
                Z = joint.Position.Z,
                W = 0,
            };

            return newVector;
        }
    }
}
