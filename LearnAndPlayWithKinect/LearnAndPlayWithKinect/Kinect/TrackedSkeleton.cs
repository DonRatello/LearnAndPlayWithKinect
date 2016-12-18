using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;

namespace LearnAndPlayWithKinect.Kinect
{
    class TrackedSkeleton
    {
        Skeleton skeleton;

        public TrackedSkeleton()
        {
            skeleton = null;
        }

        public void updateSkeleton(Skeleton s)
        {
            this.skeleton = s;
        }

        public Skeleton getSkeleton()
        {
            return skeleton;
        }

        public Joint getJoint(JointType joint)
        {
            if (skeleton != null)
            {
                return this.skeleton.Joints[joint];
            }
            else
            {
                return new Joint();
            }
        }
    }
}
