/******************************************************************
 * Klasa ta stanowi reprezentacje ikon gier na ekranach
 * Kindergarten oraz School
 ******************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using LearnAndPlayWithKinect.Other;

namespace LearnAndPlayWithKinect
{
    class GamePicture
    {
        public Texture2D picTexture;
        public Vector2 picPosition;
        public Rectangle picRectangle;
        public int picProgress;
        public GameType picType;

        public GamePicture(Texture2D tex, Vector2 vect, GameType type)
        {
            this.picTexture = tex;
            this.picPosition = vect;
            this.picType = type;
            this.picProgress = 0;
            this.picRectangle = new Rectangle((int)picPosition.X, (int)picPosition.Y, picTexture.Width, picTexture.Height);
        }
    }
}
