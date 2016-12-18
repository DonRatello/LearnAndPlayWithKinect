using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using LearnAndPlayWithKinect.Other;
using LearnAndPlayWithKinect.Interfaces;
using Microsoft.Speech.AudioFormat;
using Microsoft.Speech.Recognition;
using LearnAndPlayWithKinect.Kinect;
using LearnAndPlayWithKinect.Cloud;
using LearnAndPlayWithKinect.Sounds;

namespace LearnAndPlayWithKinect.Games.PickingApples
{
    class Screen : iScreen
    {
        const int NEW_APPLE_TIME = 70;

        Fonts fonts;
        Texture2D txBackground;
        Texture2D txFinish;
        Texture2D txNoConnection;
        Texture2D txCloudBlue;
        Texture2D txClock;
        Texture2D txPrize;

        Time timer;
        ColorStreamRenderer colorStream;

        public bool firstEntrance;
        Mode gameMode;
        Player player;

        List<Texture2D> apples;
        List<Apple> airApple;
        Services cloud;
        SoundVault sounds;

        int time;

        public Screen(Fonts fonts, ColorStreamRenderer stream)
        {
            cloud = new Services();
            colorStream = stream;
            firstEntrance = true;
            this.fonts = fonts;
            gameMode = Mode.Start;
            time = 0;
            timer = new Time(2, 00);
            airApple = new List<Apple>();

            sounds = new SoundVault(new List<AvailableSounds>() 
                { 
                    AvailableSounds.END_GAME,
                    AvailableSounds.TICK,
                    AvailableSounds.CARTOON_HOP,

                }, 1);
        }

        public void loadContent(ContentManager content)
        {
            Console.WriteLine("**************** LOAD CONTENT: Picking Apples Screen ****************");
            sounds.loadContent(content);
            getScoreFromCloud();
            txBackground = content.Load<Texture2D>("picking_apples\\txBackground");
            txFinish = content.Load<Texture2D>("picking_apples\\finish");
            txNoConnection = content.Load<Texture2D>("BrakPolaczenia");
            txCloudBlue = content.Load<Texture2D>("picking_apples\\cloud_blue");
            txClock = content.Load<Texture2D>("picking_apples\\clock");
            txPrize = content.Load<Texture2D>("picking_apples\\prize");

            apples = new List<Texture2D>();
            apples.Add(content.Load<Texture2D>("picking_apples\\apple_1"));
            apples.Add(content.Load<Texture2D>("picking_apples\\apple_2"));
            apples.Add(content.Load<Texture2D>("picking_apples\\apple_3"));
            apples.Add(content.Load<Texture2D>("picking_apples\\apple_4"));
            apples.Add(content.Load<Texture2D>("picking_apples\\apple_5"));

            player = new Player(content.Load<Texture2D>("picking_apples\\basket"), 0.8f);
        }

        public GameType update(Rectangle cursorRect)
        {
            throw new NotImplementedException();
        }

        public GameType update(Joint leftHand, Joint rightHand, Kinect.WordCommands command)
        {
            switch (command)
            {
                case Kinect.WordCommands.START:
                    {
                        if (gameMode == Mode.Start)
                        {
                            timer.isEnabled = true;
                            gameMode = Mode.Play;
                        }
  
                        if (gameMode == Mode.Finish)
                        {
                            sounds.PlaySound(AvailableSounds.CARTOON_HOP);
                            resetGame();
                        }

                        break;
                    }
                case Kinect.WordCommands.STOP:
                    {
                        if (gameMode == Mode.Start || gameMode == Mode.Finish)
                        {
                            sounds.PlaySound(AvailableSounds.CARTOON_HOP);
                            return GameType.Kindergarten;
                        }
                        else
                        {
                            timer.cleanTimer();
                            gameMode = Mode.Finish;
                        }

                        break;
                    }
            }

            switch (gameMode)
            {
                case Mode.Start:
                    {
                        break;
                    }
                case Mode.Play:
                    {
                        time++;

                        //---------------------------------
                        // TIMER
                        if (timer.isWorking && timer.isEnabled)
                        {
                            timer.countTimeLeft();
                        }
                        else if (timer.isEnabled && !timer.isWorking)
                        {
                            timer.startTimer();
                        }

                        if (timer.seconds < 0 && timer.isEnabled)
                        {
                            //Czas sie skonczyl
                            timer.cleanTimer();
                            gameMode = Mode.Finish;
                        }
                        //---------------------------------

                        if (time > NEW_APPLE_TIME)
                        {
                            generateApple();
                            time = 0;
                        }

                        foreach (Apple apple in airApple)
                        {
                            apple.updateFall();

                            if (player.rectBasket_Left.Intersects(apple.cardRectangle))
                            {
                                if (apple.visible)
                                {
                                    if (Settings.PLAY_PICK_APPLE_SOUND) sounds.PlaySound(AvailableSounds.TICK);
                                    apple.visible = false;
                                    player.points++;
                                }
                            }
                            if (player.rectBasket_Right.Intersects(apple.cardRectangle))
                            {
                                if (apple.visible)
                                {
                                    if (Settings.PLAY_PICK_APPLE_SOUND) sounds.PlaySound(AvailableSounds.TICK);
                                    apple.visible = false;
                                    player.points++;
                                }
                            } 
                        }

                        player.setBasketPosition(colorStream.SkeletonToColorMap(leftHand.Position), colorStream.SkeletonToColorMap(rightHand.Position));

                        break;
                    }
                case Mode.Finish:
                    {
                        sounds.PlaySoundOnce(AvailableSounds.END_GAME);
                        if (!cloud.isSent)
                        {
                            cloud.isSent = true;
                            setScoreToCloud();
                        }

                        break;
                    }
            }

            return GameType.PickingApples;
        }

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(txBackground, Vector2.Zero, Color.White);

            switch (gameMode)
            {
                case Mode.Start:
                    {
                        Rectangle leftTxt = new Rectangle(10, 200, 410, 180);
                        Utils.DrawStringAlign(spriteBatch, fonts.SegoeScript32, "Rozpoczęcie gry", leftTxt, Utils.TextAlignment.Top, Color.Black);
                        Utils.DrawStringAlign(spriteBatch, fonts.AngryBirds32, "powiedz START", leftTxt, Utils.TextAlignment.Center, Color.Black);
                        Utils.DrawStringAlign(spriteBatch, fonts.AngryBirds32, "lub po ang. PLAY", leftTxt, Utils.TextAlignment.Bottom, Color.Black);

                        Rectangle rightTxt = new Rectangle(580, 200, 410, 180);
                        Utils.DrawStringAlign(spriteBatch, fonts.SegoeScript32, "Poprzedni ekran", rightTxt, Utils.TextAlignment.Top, Color.Black);
                        Utils.DrawStringAlign(spriteBatch, fonts.AngryBirds32, "powiedz STOP", rightTxt, Utils.TextAlignment.Center, Color.Black);
                        Utils.DrawStringAlign(spriteBatch, fonts.AngryBirds32, "lub po ang. BACK", rightTxt, Utils.TextAlignment.Bottom, Color.Black);
                        break;
                    }
                case Mode.Play:
                    {
                        //-----------------------------
                        // KINECT VIDEO
                        spriteBatch.End();
                        colorStream.drawManually(false);
                        spriteBatch.Begin();
                        //----------------------------

                        foreach (Apple apple in airApple)
                        {
                            if(Settings.PROJECT_STATE == Settings.Environment.DEVELOPMENT) spriteBatch.Draw(txBackground, apple.cardRectangle, Color.Red);
                            apple.Draw(spriteBatch);
                        }

                        if (Settings.PROJECT_STATE == Settings.Environment.DEVELOPMENT)
                        {
                            spriteBatch.Draw(txBackground, player.rectBasket_Right, Color.Green);
                            spriteBatch.Draw(txBackground, player.rectBasket_Left, Color.Green);
                        }

                        spriteBatch.Draw(player.txBasket, player.rightHand, null, Color.White, 0.0f, player.basketOrigin, player.scale, SpriteEffects.None, 0.0f);
                        spriteBatch.Draw(player.txBasket, player.leftHand, null, Color.White, 0.0f, player.basketOrigin, player.scale, SpriteEffects.None, 0.0f);

                        spriteBatch.Draw(txCloudBlue, new Vector2(210, 510), null, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
                        spriteBatch.Draw(txCloudBlue, new Vector2(Settings.WINDOW_WIDTH - (txCloudBlue.Width+210), 510), null, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
                        spriteBatch.Draw(txClock, new Vector2(230, 630), null, Color.White, 0.0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0.0f);
                        spriteBatch.Draw(txPrize, new Vector2(Settings.WINDOW_WIDTH - (txCloudBlue.Width + 190), 630), null, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
                        
                        Utils.DrawStringAlign(spriteBatch, fonts.Pericles46, player.points.ToString(), new Rectangle(Settings.WINDOW_WIDTH - (txCloudBlue.Width + 210), 510, txCloudBlue.Width, txCloudBlue.Height), Utils.TextAlignment.Center, Color.White);

                        if (timer.minutes == 0 && timer.seconds <= 10)
                            Utils.DrawStringAlign(spriteBatch, fonts.Pericles46, timer.getMinutesFormatted() + ":" + timer.getSecondsFormatted(), new Rectangle(210, 510, txCloudBlue.Width, txCloudBlue.Height), Utils.TextAlignment.Center, Color.Red);
                        else
                            Utils.DrawStringAlign(spriteBatch, fonts.Pericles46, timer.getMinutesFormatted() + ":" + timer.getSecondsFormatted(), new Rectangle(210, 510, txCloudBlue.Width, txCloudBlue.Height), Utils.TextAlignment.Center, Color.White);
                        break;
                    }
                case Mode.Finish:
                    {
                        spriteBatch.Draw(txFinish, Vector2.Zero, Color.White);
                        spriteBatch.DrawString(fonts.Pericles46, player.points.ToString(), new Vector2(170, 220), Color.OrangeRed);

                        //TOP 10
                        if (cloud.bestScores != null)
                        {
                            for (int i = 0; i < cloud.bestScores.data.Count; i++)
                            {
                                switch (i)
                                {
                                    case 0:
                                        {
                                            spriteBatch.DrawString(fonts.Kootenay22, (i + 1).ToString() + ".", new Vector2(570, 100 + i * 40), Color.White);
                                            spriteBatch.DrawString(fonts.Kootenay22, cloud.bestScores.data[i].score, new Vector2(620, 100 + i * 40), Color.Gold);
                                            spriteBatch.DrawString(fonts.Kootenay22, cloud.bestScores.data[i].user, new Vector2(700, 100 + i * 40), Color.Gold);
                                            break;
                                        }
                                    default:
                                        {
                                            spriteBatch.DrawString(fonts.Kootenay22, (i + 1).ToString() + ".", new Vector2(570, 100 + i * 40), Color.White);
                                            spriteBatch.DrawString(fonts.Kootenay22, cloud.bestScores.data[i].score, new Vector2(620, 100 + i * 40), Color.White);
                                            spriteBatch.DrawString(fonts.Kootenay22, cloud.bestScores.data[i].user, new Vector2(700, 100 + i * 40), Color.White);
                                            break;
                                        }
                                }
                            }
                        }
                        else
                        {
                            spriteBatch.Draw(txNoConnection, new Vector2(590, 70), Color.White);
                        }

                        break;
                    }
            }
        }

        public void generateApple()
        {
            MersenneTwister mt = new MersenneTwister();
            Apple apple = new Apple(apples.ElementAt(mt.Next(1, 5)), mt.Next(150, 800), 0.5f, (float)Utils.ConvertToRadians(mt.Next(0, 360)));
            airApple.Add(apple);
        }

        public void resetGame()
        {
            gameMode = Mode.Play;
            time = 0;
            timer = new Time(2, 00);
            player.points = 0;

            airApple = new List<Apple>();
            sounds.resetSounds();
            getScoreFromCloud();
        }

        public void setScoreToCloud()
        {
            cloud.setScore(GameType.PickingApples, player.points);
        }

        public void getScoreFromCloud()
        {
            cloud.getScore(GameType.PickingApples, 5);
        }
    }
}
