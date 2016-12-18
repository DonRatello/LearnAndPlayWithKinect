using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;

namespace LearnAndPlayWithKinect.Games.Gymnastics.MovesController
{
    abstract class Hand : BaseJoint
    {
        public bool moveLeft(int time, Joint hand)
        {
            updateJoints(time, hand);

            if (time >= TIME_MAX)
            {
                //Sprawdzenie ruchu
                if ((jointMax.Position.X - jointZero.Position.X) < -0.7f && isJointOk()) return true;
            }

            return false;
        }

        public bool moveRight(int time, Joint hand)
        {
            updateJoints(time, hand);

            if (time >= TIME_MAX)
            {
                //Sprawdzenie ruchu
                if ((jointMax.Position.X - jointZero.Position.X) > 0.7f && isJointOk()) return true;
            }

            return false;
        }

        public bool moveUp(int time, Joint hand)
        {
            updateJoints(time, hand);

            if (time >= TIME_MAX)
            {
                //Sprawdzenie ruchu
                if ((jointMax.Position.Y - jointZero.Position.Y) > 0.7f && isJointOk()) return true;
            }

            return false;
        }

        public bool moveDown(int time, Joint hand)
        {
            updateJoints(time, hand);

            if (time >= TIME_MAX)
            {
                //Sprawdzenie ruchu
                if ((jointMax.Position.Y - jointZero.Position.Y) < -0.7f && isJointOk()) return true;
            }

            return false;
        }
    }
}
