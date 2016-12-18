using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;

namespace LearnAndPlayWithKinect.Games.Gymnastics.MovesController
{
    class Hip : BaseJoint
    {
        /* WYKAZ RUCHOW
         *
         * HIP_UP,
         * HIP_DOWN,
         * 
         */

        public Hip(int TIME_MAX)
        {
            this.TIME_MAX = TIME_MAX;
        }

        public bool moveUp(int time, Joint hand)
        {
            updateJoints(time, hand);

            if (time >= TIME_MAX)
            {
                //Sprawdzenie ruchu
                if ((jointMax.Position.Y - jointZero.Position.Y) > 0.4f && isJointOk()) return true;
            }

            return false;
        }

        public bool moveDown(int time, Joint hand)
        {
            updateJoints(time, hand);

            if (time >= TIME_MAX)
            {
                //Sprawdzenie ruchu
                if ((jointMax.Position.Y - jointZero.Position.Y) < -0.4f && isJointOk()) return true;
            }

            return false;
        }
    }
}
