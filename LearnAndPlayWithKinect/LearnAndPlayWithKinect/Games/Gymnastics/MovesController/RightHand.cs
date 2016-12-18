using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;

namespace LearnAndPlayWithKinect.Games.Gymnastics.MovesController
{
    class RightHand : Hand
    {
        /* WYKAZ RUCHOW
         *
         * RIGHT_HAND_LEFT,
         * RIGHT_HAND_RIGHT,
         * RIGHT_HAND_UP,
         * RIGHT_HAND_DOWN,
         * 
         */

        public RightHand(int TIME_MAX)
        {
            this.TIME_MAX = TIME_MAX;
        }
    }
}
