using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;

namespace LearnAndPlayWithKinect.Games.Gymnastics.MovesController
{
    class RightLeg : Leg
    {
        /* WYKAZ RUCHOW
         *
         * RIGHT_LEG_RIGHT,
         * RIGHT_LEG_LEFT,
         * RIGHT_LEG_UP,
         * 
         */

        public RightLeg(int TIME_MAX)
        {
            this.TIME_MAX = TIME_MAX;
        }
    }
}
