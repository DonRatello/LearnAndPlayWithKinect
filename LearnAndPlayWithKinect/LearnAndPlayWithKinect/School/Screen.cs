using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using LearnAndPlayWithKinect.Other;
using LearnAndPlayWithKinect.Interfaces;
using LearnAndPlayWithKinect.Sounds;

namespace LearnAndPlayWithKinect.School
{
    class Screen : iScreen
    {
        List<GamePicture> gameList;

        Texture2D txBackground;
        Texture2D txProgressBar;
        Texture2D txCircleBlue;
        Texture2D txCircleGray;
        Texture2D txCircleOrange;
        SoundVault sounds;

        Vector2 posProgressBar;
        Rectangle rectBarFragment_100;
        Rectangle rectBarFragment_75;
        Rectangle rectBarFragment_50;
        Rectangle rectBarFragment_25;
        Rectangle rectBarFragment_0;

        int progress;
        float circleScale;

        public Screen()
        {
            gameList = new List<GamePicture>();
            progress = 0;
            sounds = new Sounds.SoundVault(new List<AvailableSounds>() { AvailableSounds.TOY_STORE, AvailableSounds.COLORS }, 5);
            circleScale = 0.0f;
        }

        public void loadContent(ContentManager content)
        {
            Console.WriteLine("**************** LOAD CONTENT: School Screen ****************");
            sounds.loadContent(content);
            txBackground = content.Load<Texture2D>("school\\4");
            txProgressBar = content.Load<Texture2D>("kindergarten//progress_bar");
            txCircleBlue = content.Load<Texture2D>("kindergarten//circle_blue");
            txCircleGray = content.Load<Texture2D>("kindergarten//circle_gray");
            txCircleOrange = content.Load<Texture2D>("kindergarten//circle_orange");
            posProgressBar = new Vector2((Settings.WINDOW_WIDTH / 2) - 169, 100);
            rectBarFragment_100 = new Rectangle(0, 56 * 0, txProgressBar.Width, 24);
            rectBarFragment_75 = new Rectangle(0, 56 * 1, txProgressBar.Width, 24);
            rectBarFragment_50 = new Rectangle(0, 113, txProgressBar.Width, 24);
            rectBarFragment_25 = new Rectangle(0, 170, txProgressBar.Width, 24);
            rectBarFragment_0 = new Rectangle(0, 226, txProgressBar.Width, 24);

            // Games
            gameList.Add(new GamePicture(content.Load<Texture2D>("school//txToyStore"), new Vector2(380, 530), GameType.ToyStore));
            gameList.Add(new GamePicture(content.Load<Texture2D>("school//txEngColors"), new Vector2(680, 300), GameType.EngColors));
        }

        public GameType update(Rectangle cursorRect)
        {
            if (circleScale < 0.7f) circleScale += 0.008f;
            else circleScale = 0.0f;

            foreach (GamePicture picture in gameList)
            {
                if (cursorRect.Intersects(picture.picRectangle))
                {
                    picture.picProgress++;

                    switch (picture.picType)
                    {
                        case GameType.ToyStore:
                            {
                                sounds.PlaySound(AvailableSounds.TOY_STORE);
                                break;
                            }
                        case GameType.EngColors:
                            {
                                sounds.PlaySound(AvailableSounds.COLORS);
                                break;
                            }
                    }
                }
                else if ((picture.picProgress - 1) >= 0)
                {
                    picture.picProgress--;
                }

                if (picture.picProgress > 0) progress = picture.picProgress;

                if (picture.picProgress >= 215)
                {
                    picture.picProgress = 0;
                    progress = 0;
                    return picture.picType;
                } 
            }
            
            return GameType.School;
        }

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(txBackground, Vector2.Zero, null, Color.White, 0.0f, Vector2.Zero, 0.32552083333333333333333333333f, SpriteEffects.None, 0.0f);

            foreach (GamePicture picture in gameList)
            {
                spriteBatch.Draw(txCircleBlue, new Vector2(picture.picPosition.X + picture.picTexture.Width / 2, picture.picPosition.Y + picture.picTexture.Height / 2), null, Color.White * 0.5f, 0.0f,
                    new Vector2(txCircleGray.Width / 2, txCircleGray.Height / 2), circleScale, SpriteEffects.None, 0.0f);

                spriteBatch.Draw(picture.picTexture, picture.picPosition, Color.White);
            }

            if (progress >= 0 * 2 && progress < 25 * 2) spriteBatch.Draw(txProgressBar, posProgressBar, rectBarFragment_0, Color.White);
            if (progress >= 25 * 2 && progress < 50 * 2) spriteBatch.Draw(txProgressBar, posProgressBar, rectBarFragment_25, Color.White);
            if (progress >= 50 * 2 && progress < 75 * 2) spriteBatch.Draw(txProgressBar, posProgressBar, rectBarFragment_50, Color.White);
            if (progress >= 75 * 2 && progress < 100 * 2) spriteBatch.Draw(txProgressBar, posProgressBar, rectBarFragment_75, Color.White);
            if (progress >= 100 * 2) spriteBatch.Draw(txProgressBar, posProgressBar, rectBarFragment_100, Color.White);
        }
    }
}
