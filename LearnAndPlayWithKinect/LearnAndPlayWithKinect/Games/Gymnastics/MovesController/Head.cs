using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;

namespace LearnAndPlayWithKinect.Games.Gymnastics.MovesController
{
    class Head : BaseJoint
    {
        /* WYKAZ RUCHOW
         *
         * HEAD_LEFT,
         * HEAD_RIGHT,
         * 
         */

        public Head(int TIME_MAX)
        {
            this.TIME_MAX = TIME_MAX;
        }

        public bool moveLeft(int time, Joint head)
        {
            updateJoints(time, head);

            if (time >= TIME_MAX)
            {
                //Sprawdzenie ruchu
                if ((jointMax.Position.X - jointZero.Position.X) < -0.5f && isJointOk()) return true;
            }

            return false;
        }

        public bool moveRight(int time, Joint head)
        {
            updateJoints(time, head);

            if (time >= TIME_MAX)
            {
                //Sprawdzenie ruchu
                if ((jointMax.Position.X - jointZero.Position.X) > 0.5f && isJointOk()) return true;
            }

            return false;
        }
    }
}
