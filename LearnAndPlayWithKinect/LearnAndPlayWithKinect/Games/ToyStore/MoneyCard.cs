using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace LearnAndPlayWithKinect.Games.ToyStore
{
    class MoneyCard : Games.Card
    {
        public float scale;
        public int cardProgress;
        public int value;
        public bool isChoosen;
        public Vector2 originPosition;
        public MoneyType moneyType;

        public MoneyCard(Texture2D cardTexture, Vector2 cardPosition, int moneyValue, MoneyType moneyType)
        {
            this.cardTexture = cardTexture;
            this.cardPosition = cardPosition;
            scale = 1.0f;
            cardProgress = 0;
            this.value = moneyValue;
            isChoosen = false;
            this.cardRectangle = new Rectangle((int)cardPosition.X, (int)cardPosition.Y, (int)(cardTexture.Width * scale), (int)(cardTexture.Height * scale));
            this.originPosition = this.cardPosition;
            this.moneyType = moneyType;
        }

        public void setScale(float scale)
        {
            this.scale = scale;
            this.cardRectangle = new Rectangle((int)cardPosition.X, (int)cardPosition.Y, (int)(cardTexture.Width * scale), (int)(cardTexture.Height * scale));
        }
    }
}
