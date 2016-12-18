using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace LearnAndPlayWithKinect.Games.Memory
{
    class MemoryCard : Games.Card
    {
        public int cardId;
        public CardType cardType;
        public bool isVisible;
        public int clickProgress;

        public MemoryCard(int cardId, Vector2 cardPosition, Texture2D cardTexture, CardType cardType)
        {
            this.cardId = cardId;
            clickProgress = 0;
            isVisible = false;
            this.cardPosition = cardPosition;
            this.cardTexture = cardTexture;
            this.cardType = cardType;

            if (this.cardTexture != null) this.cardRectangle = new Rectangle((int)cardPosition.X, (int)cardPosition.Y, this.cardTexture.Width, this.cardTexture.Height);
        }
    }
}
