using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using LearnAndPlayWithKinect.Other;
using LearnAndPlayWithKinect.Interfaces;

namespace LearnAndPlayWithKinect.Intro
{
    class Screen : iScreen
    {
        Vector2 posLogo;
        Texture2D txLogo;
        Texture2D txBackground;

        Video video;
        VideoPlayer player;
        Texture2D videoTexture;

        Fonts fonts;
        bool wasPlayed = false;

        Rectangle screen = new Rectangle(0, 68, 1000, 563);

        public Screen(Fonts fonts)
        {
            this.fonts = fonts;
            posLogo = new Vector2((Settings.WINDOW_WIDTH / 2) - 200, (Settings.WINDOW_HEIGHT / 2) - 212);
            player = new VideoPlayer();
        }

        public void loadContent(ContentManager content)
        {
            Console.WriteLine("**************** LOAD CONTENT: Intro Screen ****************");
            txLogo = content.Load<Texture2D>("logo");
            txBackground = content.Load<Texture2D>("intro_background");

            video = content.Load<Video>("Intro");
        }

        public GameType update()
        {
            //---------------------------------
            // VIDEO
            if (player.State == MediaState.Stopped)
            {
                if (wasPlayed)
                {
                    return GameType.MainMenu;
                }
                else
                {
                    wasPlayed = true;
                    player.IsLooped = false;
                    player.Play(video);
                    
                }
            }

            return GameType.Intro;
            //---------------------------------
        }

        public GameType update(Rectangle rect)
        {
            throw new NotImplementedException("Method not implemented");
        }

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(txBackground, Vector2.Zero, Color.White);

            //-------------------------------------------------------------------------------
            // VIDEO
            if (player.State != MediaState.Stopped) videoTexture = player.GetTexture();

            if (videoTexture != null) spriteBatch.Draw(videoTexture, screen, Color.White);
            //-------------------------------------------------------------------------------
        }
    }
}
