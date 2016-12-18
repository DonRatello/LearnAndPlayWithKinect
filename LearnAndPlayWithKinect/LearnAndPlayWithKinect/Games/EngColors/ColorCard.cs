using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using LearnAndPlayWithKinect.Other;

namespace LearnAndPlayWithKinect.Games.EngColors
{
    class ColorCard : Games.Card
    {
        public Vector2 cardPositionOriginal;
        public Colors cardColor;
        public int cardProgress;
        public bool cardPicked;
        public bool cardMatched;

        public ColorCard()
        {
            
        }

        public ColorCard(Texture2D cardTex, Vector2 cardPosition, Colors cardColor)
        {
            this.cardMatched = false;
            this.cardPicked = false; 
            this.cardColor = cardColor;
            this.cardPosition = cardPosition;
            this.cardPositionOriginal = cardPosition;
            this.cardProgress = 0;
            this.cardRectangle = new Rectangle((int)cardPosition.X, (int)cardPosition.Y, cardTex.Width, cardTex.Height);
            this.cardTexture = cardTex;
        }
    }
}
