using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;

namespace LearnAndPlayWithKinect.Games.Gymnastics.MovesController
{
    abstract class BaseJoint
    {
        protected Joint jointZero;
        protected Joint jointMax;
        protected int TIME_MAX;

        protected void updateJoints(int time, Joint joint)
        {
            if (time == 0)
            {
                this.jointZero = joint;
            }

            if (time == TIME_MAX)
            {
                this.jointMax = joint;
            }
        }

        public Joint getJointZero()
        {
            return this.jointZero;
        }

        public Joint getJointMax()
        {
            return this.jointMax;
        }

        public bool isJointOk()
        {
            if (jointZero.Position.X != 0 && jointZero.Position.Y != 0 && jointMax.Position.X != 0 && jointMax.Position.Y != 0)
                return true;
            else
                return false;
        }

        public void cleanJoints()
        {
            this.jointMax = new Joint();
            this.jointZero = new Joint();
        }
    }
}
