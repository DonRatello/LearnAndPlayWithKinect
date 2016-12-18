using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;

namespace LearnAndPlayWithKinect.Games.Gymnastics.MovesController
{
    class LeftLeg : Leg
    {
        /* WYKAZ RUCHOW
         *
         * LEFT_LEG_RIGHT,
         * LEFT_LEG_LEFT,
         * LEFT_LEG_UP,
         * 
         */

        public LeftLeg(int TIME_MAX)
        {
            this.TIME_MAX = TIME_MAX;
        } 
    }
}
