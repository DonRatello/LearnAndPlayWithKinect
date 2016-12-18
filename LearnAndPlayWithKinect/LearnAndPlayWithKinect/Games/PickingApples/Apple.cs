using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using LearnAndPlayWithKinect.Other;

/*************************************************************/
/* W tej klasie był problem z tzw. boxem kolizyjnym          */
/* Przy rotacji tekstury box się rotował ale w dziwny sposób */
/* Sposobem na to jest ustawienie pivota w środku obiektu    */
/* a następnie przesunięcie go w górę i w lewo               */
/*************************************************************/
namespace LearnAndPlayWithKinect.Games.PickingApples
{
    class Apple : Card
    {
        public float scale = 0.0f;
        public float rotation = 0.0f;
        public bool visible = true;
        public Vector2 origin;

        public Apple(Texture2D texture, int startingX, float scale, float rotation)
        {
            this.rotation = rotation;
            this.scale = scale;
            this.cardTexture = texture;
            this.cardPosition = new Vector2(startingX, 0);
            this.cardRectangle = new Rectangle((int)cardPosition.X, (int)cardPosition.Y, (int)(cardTexture.Width * scale), (int)(cardTexture.Height * scale));
            this.origin = new Vector2((cardTexture.Width / 2), (cardTexture.Height / 2));
        }

        public void changePosition(int x, int y)
        {
            cardPosition = new Vector2(x, y);
            cardRectangle = new Rectangle((int)cardPosition.X, (int)cardPosition.Y, (int)(cardTexture.Width * scale), (int)(cardTexture.Height * scale));
        }

        public void updateFall()
        {
            //this.cardPosition.Y++;
            this.cardPosition.Y += 2;
            cardRectangle = new Rectangle((int)cardPosition.X - (int)(cardTexture.Width / 4), (int)cardPosition.Y - (int)(cardTexture.Height / 4), (int)(cardTexture.Width * scale), (int)(cardTexture.Height * scale));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if ((cardPosition.Y < Settings.WINDOW_HEIGHT) && visible)
            {
                spriteBatch.Draw(cardTexture, cardPosition, null, Color.White, rotation, origin, scale, SpriteEffects.None, 0);
            }
        }
    }
}
