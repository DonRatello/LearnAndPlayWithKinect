using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using LearnAndPlayWithKinect.Other;

namespace LearnAndPlayWithKinect.Games.PickingApples
{
    class Player
    {
        public Texture2D txBasket;
        public Vector2 basketOrigin;
        public Rectangle rectBasket_Right;
        public Rectangle rectBasket_Left;
        public float scale;

        public Vector2 leftHand;
        public Vector2 rightHand;

        public int points;

        public Player(Texture2D txBasket, float scale)
        {
            leftHand = new Vector2();
            rightHand = new Vector2();
            rectBasket_Left = new Rectangle();
            rectBasket_Right = new Rectangle();
            points = 0;
            this.txBasket = txBasket;
            this.scale = scale;

            basketOrigin = new Vector2((this.txBasket.Width * scale) / 2, 0);
        }

        public void setBasketPosition(Vector2 leftHand, Vector2 rightHand)
        {
            this.leftHand = new Vector2(leftHand.X + 100, leftHand.Y + 20);
            this.rightHand = new Vector2(rightHand.X + 220, rightHand.Y + 20);

            // TODO
            // NAPRAWIC PRAWY BOUNDING BOX
            rectBasket_Right = new Rectangle((int)rightHand.X + 180, (int)rightHand.Y + 65, (int)(txBasket.Width * scale), (int)((txBasket.Height * scale) / 2));
            rectBasket_Left = new Rectangle((int)leftHand.X + (int)((txBasket.Width/2) * scale)+ 10, (int)leftHand.Y + 60, (int)(txBasket.Width * scale), (int)((txBasket.Height * scale) / 2));
        }
    }
}
