using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace LearnAndPlayWithKinect.Games.ToyStore
{
    class ToyCard : Games.Card
    {
        public float scale;
        public int cardProgress;
        public int toyPrize;
        public bool isChoosen;
        public Texture2D cardTextureHighlighted;

        public ToyCard(Texture2D cardTexture, Vector2 cardPosition, int toyPrize)
        {
            this.cardTexture = cardTexture;
            this.cardPosition = cardPosition;
            this.cardProgress = 0;
            this.toyPrize = toyPrize;
            this.isChoosen = false;
            this.scale = 1.0f;
            this.cardRectangle = new Rectangle((int)cardPosition.X, (int)cardPosition.Y, (int)(cardTexture.Width * scale), (int)(cardTexture.Height * scale));
        }

        public void setScale(float scale)
        {
            this.scale = scale;
            this.cardRectangle = new Rectangle((int)cardPosition.X, (int)cardPosition.Y, (int)(cardTexture.Width * scale), (int)(cardTexture.Height * scale));
        }
    }
}
