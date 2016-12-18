using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;

namespace LearnAndPlayWithKinect.Games.Gymnastics
{
    enum Moves
    {
        // PRAWA REKA
        RIGHT_HAND_LEFT,
        RIGHT_HAND_RIGHT,
        RIGHT_HAND_UP,
        RIGHT_HAND_DOWN,

        // LEWA REKA
        LEFT_HAND_LEFT,
        LEFT_HAND_RIGHT,
        LEFT_HAND_UP,
        LEFT_HAND_DOWN,

        // GLOWA
        HEAD_LEFT,
        HEAD_RIGHT,

        // MIEDNICA
        HIP_UP,
        HIP_DOWN,

        // PRAWA NOGA
        RIGHT_LEG_RIGHT,
        RIGHT_LEG_LEFT,
        RIGHT_LEG_UP,

        // LEWA NOGA
        LEFT_LEG_RIGHT,
        LEFT_LEG_LEFT,
        LEFT_LEG_UP,
    }
}
