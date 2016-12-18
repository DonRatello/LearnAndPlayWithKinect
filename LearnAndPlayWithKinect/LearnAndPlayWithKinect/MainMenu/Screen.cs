using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using LearnAndPlayWithKinect.Other;
using LearnAndPlayWithKinect.Interfaces;
using LearnAndPlayWithKinect.Sounds;

namespace LearnAndPlayWithKinect.MainMenu
{
    class Screen : iScreen
    {
        Texture2D txBackground;

        int redProgress;
        int blueProgress;
        Rectangle redCircleRectangle;
        Rectangle blueCircleRectangle;

        Vector2 posBlueCircle;
        Vector2 posRedCircle;
   
        Fonts fonts;
        Sounds.SoundVault sounds;

        //--------------------------
        // Czerwone kolo tekstura
        Texture2D txRedCircle10;
        Texture2D txRedCircle20;
        Texture2D txRedCircle30;
        Texture2D txRedCircle40;
        Texture2D txRedCircle50;
        Texture2D txRedCircle60;
        Texture2D txRedCircle70;
        Texture2D txRedCircle80;
        Texture2D txRedCircle90;
        Texture2D txRedCircle100;
        //--------------------------
        // Niebieskie kolo tekstura
        Texture2D txBlueCircle10;
        Texture2D txBlueCircle20;
        Texture2D txBlueCircle30;
        Texture2D txBlueCircle40;
        Texture2D txBlueCircle50;
        Texture2D txBlueCircle60;
        Texture2D txBlueCircle70;
        Texture2D txBlueCircle80;
        Texture2D txBlueCircle90;
        Texture2D txBlueCircle100;
        //--------------------------

        public Screen(Fonts fonts)
        {
            redProgress = 0;
            blueProgress = 0;
            this.fonts = fonts;
            sounds = new Sounds.SoundVault(new List<Sounds.AvailableSounds>() { AvailableSounds.KINDERGARTEN, AvailableSounds.SCHOOL }, 6);
        }

        public void loadContent(ContentManager content)
        {
            Console.WriteLine("**************** LOAD CONTENT: MainMenu Screen ****************");
            sounds.loadContent(content);
            txBackground = content.Load<Texture2D>("mainMenu_background");

            txRedCircle10 = content.Load<Texture2D>("progress_bar//red_circle_10");
            txRedCircle20 = content.Load<Texture2D>("progress_bar//red_circle_20");
            txRedCircle30 = content.Load<Texture2D>("progress_bar//red_circle_30");
            txRedCircle40 = content.Load<Texture2D>("progress_bar//red_circle_40");
            txRedCircle50 = content.Load<Texture2D>("progress_bar//red_circle_50");
            txRedCircle60 = content.Load<Texture2D>("progress_bar//red_circle_60");
            txRedCircle70 = content.Load<Texture2D>("progress_bar//red_circle_70");
            txRedCircle80 = content.Load<Texture2D>("progress_bar//red_circle_80");
            txRedCircle90 = content.Load<Texture2D>("progress_bar//red_circle_90");
            txRedCircle100 = content.Load<Texture2D>("progress_bar//red_circle_100");

            txBlueCircle10 = content.Load<Texture2D>("progress_bar//blue_circle_10");
            txBlueCircle20 = content.Load<Texture2D>("progress_bar//blue_circle_20");
            txBlueCircle30 = content.Load<Texture2D>("progress_bar//blue_circle_30");
            txBlueCircle40 = content.Load<Texture2D>("progress_bar//blue_circle_40");
            txBlueCircle50 = content.Load<Texture2D>("progress_bar//blue_circle_50");
            txBlueCircle60 = content.Load<Texture2D>("progress_bar//blue_circle_60");
            txBlueCircle70 = content.Load<Texture2D>("progress_bar//blue_circle_70");
            txBlueCircle80 = content.Load<Texture2D>("progress_bar//blue_circle_80");
            txBlueCircle90 = content.Load<Texture2D>("progress_bar//blue_circle_90");
            txBlueCircle100 = content.Load<Texture2D>("progress_bar//blue_circle_100");

            posBlueCircle = new Vector2(Settings.WINDOW_WIDTH - txBlueCircle100.Width - 30, 180);
            posRedCircle = new Vector2(300, 100);

            redCircleRectangle = new Rectangle((int)posRedCircle.X, (int)posRedCircle.Y, txRedCircle100.Width, txRedCircle100.Height);
            blueCircleRectangle = new Rectangle((int)posBlueCircle.X, (int)posBlueCircle.Y, txBlueCircle100.Width, txBlueCircle100.Height);
        }

        public GameType update(Rectangle cursorRect)
        {
            if (cursorRect.Intersects(redCircleRectangle))
            {
                sounds.PlaySound(AvailableSounds.KINDERGARTEN);
                redProgress++;
            }
            else
            {
                if (redProgress - 1 > 0)
                {
                    redProgress--;
                }
                else
                {
                    redProgress = 0;
                }
            }

            if (cursorRect.Intersects(blueCircleRectangle))
            {
                sounds.PlaySound(AvailableSounds.SCHOOL);
                blueProgress++;
            }
            else
            {
                if (blueProgress - 1 > 0)
                {
                    blueProgress--;
                }
                else
                {
                    blueProgress = 0;
                }
            }

            //Sprawdzenie czy nie nastapila zmiana ekranu
            if (redProgress > 230)
            {
                redProgress = 0;
                return GameType.Kindergarten;
            }

            if (blueProgress > 230)
            {
                blueProgress = 0;
                return GameType.School;
            }

            return GameType.MainMenu;
        }

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(txBackground, Vector2.Zero, Color.White);

            if (redProgress == 0)
            {
                spriteBatch.Draw(txRedCircle100, posRedCircle, Color.White * 0.3f);
                spriteBatch.DrawString(fonts.Kootenay16, "Wejdź do przedszkola", new Vector2(posRedCircle.X + 10, posRedCircle.Y + 100), Color.OrangeRed);
            }
            if (blueProgress == 0)
            {
                spriteBatch.Draw(txBlueCircle100, posBlueCircle, Color.White * 0.3f);
                spriteBatch.DrawString(fonts.Kootenay16, "Wejdź do szkoły", new Vector2(posBlueCircle.X + 35, posBlueCircle.Y + 100), Color.CadetBlue);
            }

            if (redProgress > 0 && redProgress < 25) spriteBatch.Draw(txRedCircle10, posRedCircle, Color.White);
            if (redProgress > 25 && redProgress < 50) spriteBatch.Draw(txRedCircle20, posRedCircle, Color.White);
            if (redProgress > 50 && redProgress < 75) spriteBatch.Draw(txRedCircle30, posRedCircle, Color.White);
            if (redProgress > 75 && redProgress < 100) spriteBatch.Draw(txRedCircle40, posRedCircle, Color.White);
            if (redProgress > 100 && redProgress < 125) spriteBatch.Draw(txRedCircle50, posRedCircle, Color.White);
            if (redProgress > 125 && redProgress < 150) spriteBatch.Draw(txRedCircle60, posRedCircle, Color.White);
            if (redProgress > 150 && redProgress < 175) spriteBatch.Draw(txRedCircle70, posRedCircle, Color.White);
            if (redProgress > 175 && redProgress < 200) spriteBatch.Draw(txRedCircle80, posRedCircle, Color.White);
            if (redProgress > 200 && redProgress < 225) spriteBatch.Draw(txRedCircle90, posRedCircle, Color.White);
            if (redProgress > 225) spriteBatch.Draw(txRedCircle100, posRedCircle, Color.White);

            if (blueProgress > 0 && redProgress < 25) spriteBatch.Draw(txBlueCircle10, posBlueCircle, Color.White);
            if (blueProgress > 25 && redProgress < 50) spriteBatch.Draw(txBlueCircle20, posBlueCircle, Color.White);
            if (blueProgress > 50 && redProgress < 75) spriteBatch.Draw(txBlueCircle30, posBlueCircle, Color.White);
            if (blueProgress > 75 && redProgress < 100) spriteBatch.Draw(txBlueCircle40, posBlueCircle, Color.White);
            if (blueProgress > 100 && redProgress < 125) spriteBatch.Draw(txBlueCircle50, posBlueCircle, Color.White);
            if (blueProgress > 125 && redProgress < 150) spriteBatch.Draw(txBlueCircle60, posBlueCircle, Color.White);
            if (blueProgress > 150 && redProgress < 175) spriteBatch.Draw(txBlueCircle70, posBlueCircle, Color.White);
            if (blueProgress > 175 && redProgress < 200) spriteBatch.Draw(txBlueCircle80, posBlueCircle, Color.White);
            if (blueProgress > 200 && redProgress < 225) spriteBatch.Draw(txBlueCircle90, posBlueCircle, Color.White);
            if (blueProgress > 225) spriteBatch.Draw(txBlueCircle100, posBlueCircle, Color.White);
        }
    }
}
