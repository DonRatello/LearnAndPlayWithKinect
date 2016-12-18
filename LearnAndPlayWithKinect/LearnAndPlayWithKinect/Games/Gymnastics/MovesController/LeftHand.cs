using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;

namespace LearnAndPlayWithKinect.Games.Gymnastics.MovesController
{
    class LeftHand : Hand
    {
        /* WYKAZ RUCHOW
         *
         * LEFT_HAND_LEFT,
         * LEFT_HAND_RIGHT,
         * LEFT_HAND_UP,
         * LEFT_HAND_DOWN,
         * 
         */

        public LeftHand(int TIME_MAX)
        {
            this.TIME_MAX = TIME_MAX;
        }
    }
}
