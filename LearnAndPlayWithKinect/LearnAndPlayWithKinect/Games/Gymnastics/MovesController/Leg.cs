using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;

namespace LearnAndPlayWithKinect.Games.Gymnastics.MovesController
{
    abstract class Leg : BaseJoint
    {
        public bool moveLeft(int time, Joint leg)
        {
            updateJoints(time, leg);

            if (time >= TIME_MAX)
            {
                //Sprawdzenie ruchu
                if ((jointMax.Position.X - jointZero.Position.X) < -0.5f && isJointOk()) return true;
            }

            return false;
        }

        public bool moveRight(int time, Joint leg)
        {
            updateJoints(time, leg);

            if (time >= TIME_MAX)
            {
                //Sprawdzenie ruchu
                if ((jointMax.Position.X - jointZero.Position.X) > 0.5f && isJointOk()) return true;
            }

            return false;
        }

        public bool moveUp(int time, Joint leg)
        {
            updateJoints(time, leg);

            if (time >= TIME_MAX)
            {
                //Sprawdzenie ruchu
                if ((jointMax.Position.Y - jointZero.Position.Y) > 0.3f && isJointOk()) return true;
            }

            return false;
        }
    }
}
