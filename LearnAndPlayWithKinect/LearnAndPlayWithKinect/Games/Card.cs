using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace LearnAndPlayWithKinect.Games
{
    abstract class Card
    {
        public Texture2D cardTexture;
        public Vector2 cardPosition;
        public Rectangle cardRectangle;
    }
}
