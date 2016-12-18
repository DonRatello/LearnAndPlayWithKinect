using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LearnAndPlayWithKinect.Other
{
    class Utils
    {
        public enum TextAlignment
        {
            Center, 
            Left, 
            Right, 
            Top, 
            Bottom
        }

        public static void DrawStringAlign(SpriteBatch spriteBatch, SpriteFont font, string text, Rectangle bounds, TextAlignment align, Color color)
        {
            Vector2 size = font.MeasureString(text);
            Vector2 pos = new Vector2(bounds.Center.X, bounds.Center.Y);
            Vector2 origin = size * 0.5f;

            switch (align)
            {
                case TextAlignment.Left:
                    {
                        origin.X += bounds.Width / 2 - size.X / 2;
                        break;
                    }
                case TextAlignment.Right:
                    {
                        origin.X -= bounds.Width / 2 - size.X / 2;
                        break;
                    }
                case TextAlignment.Top:
                    {
                        origin.Y += bounds.Height / 2 - size.Y / 2;
                        break;
                    }
                case TextAlignment.Bottom:
                    {
                        origin.Y -= bounds.Height / 2 - size.Y / 2;
                        break;
                    }
            }

            spriteBatch.DrawString(font, text, pos, color, 0, origin, 1, SpriteEffects.None, 0);
        }

        public static double ConvertToRadians(double angle)
        {
            return (Math.PI / 180) * angle;
        }
    }
}
