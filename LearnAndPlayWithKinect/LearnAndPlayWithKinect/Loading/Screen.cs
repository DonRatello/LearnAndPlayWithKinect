using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using LearnAndPlayWithKinect.Other;
using LearnAndPlayWithKinect.Interfaces;

namespace LearnAndPlayWithKinect.Loading
{
    class Screen : iScreen
    {
        public List<Sun> suns;
        int time;
        string loadPercent;

        //Fonts
        Fonts fonts;

        //Sprite
        public Texture2D txBackground;
        public Texture2D txLogoHalf;
        public Texture2D txLoading;
        public Texture2D txLogoAuthor;

        public Screen(Fonts fonts)
        {
            this.fonts = fonts;
            loadPercent = "0%";
            time = 0;
            suns = new List<Sun>();

            int x = 100;

            for (int i = 0; i < 10; i++)
            {
                Sun sun = new Sun(x, 300);
                suns.Add(sun);
                x += 80;
            }
        }

        public void loadContent(ContentManager content)
        {
            Console.WriteLine("**************** LOAD CONTENT: Loading Screen ****************");
            txBackground = content.Load<Texture2D>("cloud_loading");
            txLogoHalf = content.Load<Texture2D>("logo_polowa");
            txLoading = content.Load<Texture2D>("ladowanie_text");
            txLogoAuthor = content.Load<Texture2D>("logo_czernecki");

            foreach (Sun sun in suns)
            {
                sun.loadContent(content);
            }
        }

        public GameType update()
        {
            time++;

            switch (time)
            {
                    //Ladowanie elementow programu
                    //Sprawdzenie sensora, jego wlaczenie, uruchomienie strumieni danych
                    //W celu zminimalizowania mozliwosci zawieszenia sie, rozkladam to w czasie
                case 100:
                    {
                        suns.ElementAt(0).drawEnabled = true;
                        loadPercent = "10%";
                        return GameType.LoadingScreen;
                    }
                case 200:
                    {
                        suns.ElementAt(1).drawEnabled = true;
                        loadPercent = "20%";
                        return GameType.LoadingScreen;
                    }
                case 300:
                    {
                        suns.ElementAt(2).drawEnabled = true;
                        loadPercent = "30%";
                        return GameType.LoadingScreen;
                    }
                case 400:
                    {
                        suns.ElementAt(3).drawEnabled = true;
                        loadPercent = "40%";
                        return GameType.LoadingScreen;
                    }
                case 500:
                    {
                        suns.ElementAt(4).drawEnabled = true;
                        loadPercent = "50%";
                        return GameType.LoadingScreen;
                    }
                case 550:
                    {
                        suns.ElementAt(5).drawEnabled = true;
                        loadPercent = "60%";
                        return GameType.LoadingScreen;
                    }
                case 600:
                    {
                        suns.ElementAt(6).drawEnabled = true;
                        loadPercent = "70%";
                        return GameType.LoadingScreen;
                    }
                case 650:
                    {
                        suns.ElementAt(7).drawEnabled = true;
                        loadPercent = "80%";
                        return GameType.LoadingScreen;
                    }
                case 700:
                    {
                        suns.ElementAt(8).drawEnabled = true;
                        loadPercent = "90%";
                        return GameType.LoadingScreen;
                    }
                case 900:
                    {
                        suns.ElementAt(9).drawEnabled = true;
                        loadPercent = "100%";
                        return GameType.LoadingScreen;
                    }
                case 1050:
                    {
                        //Ladowanie skonczone, przejscie do intra
                        //return GameType.LoadingScreen;
                        return GameType.Intro;
                    }
                default:
                    {
                        return GameType.LoadingScreen;
                    }
            }
        }

        public GameType update(Rectangle rect)
        {
            throw new NotImplementedException("Method not implemented");
        }

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(txBackground, new Vector2(0, 0), Color.White);
            spriteBatch.Draw(txLogoHalf, new Vector2(50, Settings.WINDOW_HEIGHT - 220), Color.White);
            spriteBatch.Draw(txLoading, new Vector2(100, 210), Color.White);
            spriteBatch.Draw(txLogoAuthor, new Vector2(Settings.WINDOW_WIDTH - 260, Settings.WINDOW_HEIGHT - 60), Color.White);
            spriteBatch.DrawString(fonts.Moire32, loadPercent, new Vector2(450, 400), Color.Black);
            foreach (Sun sun in suns)
            {
                sun.draw(spriteBatch);
            }
        }

        public void setTime(int time)
        {
            this.time = time;
        }

        public int getTime()
        {
            return this.time;
        }
    }
}
