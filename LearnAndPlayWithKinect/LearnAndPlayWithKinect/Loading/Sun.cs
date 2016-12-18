using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace LearnAndPlayWithKinect.Loading
{
    class Sun
    {
        public Vector2 Position;
        public Texture2D txSun;
        public bool drawEnabled;

        public Sun(int x, int y)
        {
            Position.X = x;
            Position.Y = y;
            drawEnabled = false;
        }

        public void loadContent(ContentManager content)
        {
            txSun = content.Load<Texture2D>("sun_loading");
        }

        public void update()
        {

        }

        public void draw(SpriteBatch spriteBatch)
        {
            if (drawEnabled)
            {
                spriteBatch.Draw(txSun, Position, Color.White);
            }
        }
    }
}
