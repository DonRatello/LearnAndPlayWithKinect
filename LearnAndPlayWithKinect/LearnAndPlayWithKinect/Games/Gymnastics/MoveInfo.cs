using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace LearnAndPlayWithKinect.Games.Gymnastics
{
    class MoveInfo
    {
        public Video video;
        public Moves move;
        public string name;
        public bool isDisposed;

        public MoveInfo(Video video, Moves move, string name)
        {
            this.video = video;
            this.move = move;
            this.name = name;
            this.isDisposed = false;
        }

        public void disposeVideo()
        {
            video = null;
            isDisposed = true;
        }

        public void loadVideo(Video video)
        {
            this.video = video;
            isDisposed = false;
        }
    }
}
