using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Microsoft.Kinect;
using LearnAndPlayWithKinect.Other;
using LearnAndPlayWithKinect.Interfaces;
using LearnAndPlayWithKinect.Games.Gymnastics.MovesController;
using LearnAndPlayWithKinect.Sounds;
using LearnAndPlayWithKinect.Cloud;
using Microsoft.Speech.AudioFormat;
using Microsoft.Speech.Recognition;

namespace LearnAndPlayWithKinect.Games.Gymnastics
{
    class Screen : iScreen, iCloudGame
    {
        /// <summary>
        /// Kontroler zarządzający wykrywaniem ruchu
        /// </summary>
        MainController controller;

        Time time;
        Texture2D txBackground;
        Texture2D txFinish;
        Texture2D txPointsFrame;
        Texture2D txPoints;
        Texture2D txTime;
        Texture2D txMainFrames;
        Texture2D txNoConnection;
        Texture2D txFlower;
        Texture2D txButterfly;
        Texture2D txClock;
        Texture2D txPrize;

        bool isMoveDone;
        int points;

        Fonts fonts;
        Services cloud;
        SoundVault sounds;

        Video video;
        VideoPlayer player;
        Texture2D videoTexture;
        Dictionary<Moves, MoveInfo> moveVideos;

        Mode gameMode;

        /// <summary>
        /// Viewport w którym będzie rysowany obraz video
        /// </summary>
        Rectangle screen = new Rectangle(Settings.WINDOW_WIDTH-640-6, 5, 640, 360);

        public Screen(Fonts fonts)
        {
            time = new Time(2, 30);
            isMoveDone = false;
            points = 0;
            gameMode = Mode.START;
            this.fonts = fonts;
            cloud = new Services();
            controller = new MainController();
            controller.getRandomMove();

            sounds = new SoundVault(new List<AvailableSounds>() 
                    { 
                          AvailableSounds.HEAD_LEFT,
                          AvailableSounds.HEAD_RIGHT,

                          AvailableSounds.HIP_DOWN,
                          AvailableSounds.HIP_UP,

                          AvailableSounds.LEFT_HAND_DOWN,
                          AvailableSounds.LEFT_HAND_LEFT,
                          AvailableSounds.LEFT_HAND_RIGHT,
                          AvailableSounds.LEFT_HAND_UP,

                          AvailableSounds.LEFT_LEG_LEFT,
                          AvailableSounds.LEFT_LEG_RIGHT,
                          AvailableSounds.LEFT_LEG_UP,

                          AvailableSounds.RIGHT_HAND_DOWN,
                          AvailableSounds.RIGHT_HAND_LEFT,
                          AvailableSounds.RIGHT_HAND_RIGHT,
                          AvailableSounds.RIGHT_HAND_UP,

                          AvailableSounds.RIGHT_LEG_LEFT,
                          AvailableSounds.RIGHT_LEG_RIGHT,
                          AvailableSounds.RIGHT_LEG_UP,

                          AvailableSounds.END_GAME,

                          AvailableSounds.CARTOON_HOP,
                          AvailableSounds.FANFARE,
                          AvailableSounds.BTN_ON_CLICK,
                    }, 4);
        }

        public void loadContent(ContentManager content)
        {
            Console.WriteLine("**************** LOAD CONTENT: Gymnastics Screen ****************");
            sounds.loadContent(content);
            getScoreFromCloud();
            txBackground = content.Load<Texture2D>("gymnastics\\txBackground");
            txFinish = content.Load<Texture2D>("gymnastics\\finish");
            txPointsFrame = content.Load<Texture2D>("gymnastics\\PointsFrame");
            txPoints = content.Load<Texture2D>("gymnastics\\txtPoints");
            txTime = content.Load<Texture2D>("gymnastics\\txtTime");
            txMainFrames = content.Load<Texture2D>("gymnastics\\mainFrames");
            txNoConnection = content.Load<Texture2D>("BrakPolaczenia");
            txButterfly = content.Load<Texture2D>("gymnastics\\Red_Butterfly");
            txFlower = content.Load<Texture2D>("gymnastics\\Red_Flower");
            txPrize = content.Load <Texture2D>("picking_apples\\prize");
            txClock = content.Load<Texture2D>("picking_apples\\clock");
            video = content.Load<Video>("gymnastics\\HDR");
            player = new VideoPlayer();

            //------------------------------------------------------
            // VIDEOS
            moveVideos = new Dictionary<Moves, MoveInfo>();
            moveVideos.Add(Moves.HEAD_LEFT, new MoveInfo(content.Load<Video>("gymnastics\\video\\head_left"), Moves.HEAD_LEFT, "Głowa w lewo"));
            moveVideos.Add(Moves.HEAD_RIGHT, new MoveInfo(content.Load<Video>("gymnastics\\video\\head_right"), Moves.HEAD_RIGHT, "Głowa w prawo"));
            moveVideos.Add(Moves.HIP_DOWN, new MoveInfo(content.Load<Video>("gymnastics\\video\\hip_down"), Moves.HIP_DOWN, "Kucnij"));
            moveVideos.Add(Moves.HIP_UP, new MoveInfo(content.Load<Video>("gymnastics\\video\\hip_up"), Moves.HIP_UP, "Skok"));
            moveVideos.Add(Moves.LEFT_HAND_DOWN, new MoveInfo(content.Load<Video>("gymnastics\\video\\left_hand_down"), Moves.LEFT_HAND_DOWN, "Lewa ręka w dół"));
            moveVideos.Add(Moves.LEFT_HAND_LEFT, new MoveInfo(content.Load<Video>("gymnastics\\video\\left_hand_left"), Moves.LEFT_HAND_LEFT, "Lewa ręka w lewo"));
            moveVideos.Add(Moves.LEFT_HAND_RIGHT, new MoveInfo(content.Load<Video>("gymnastics\\video\\left_hand_right"), Moves.LEFT_HAND_RIGHT, "Lewa ręka w prawo"));
            moveVideos.Add(Moves.LEFT_HAND_UP, new MoveInfo(content.Load<Video>("gymnastics\\video\\left_hand_up"), Moves.LEFT_HAND_UP, "Lewa ręka w górę"));
            moveVideos.Add(Moves.LEFT_LEG_LEFT, new MoveInfo(content.Load<Video>("gymnastics\\video\\left_leg_left"), Moves.LEFT_LEG_LEFT, "Lewa noga w lewo"));
            moveVideos.Add(Moves.LEFT_LEG_RIGHT, new MoveInfo(content.Load<Video>("gymnastics\\video\\left_leg_right"), Moves.LEFT_LEG_RIGHT, "Lewa noga w prawo"));
            moveVideos.Add(Moves.LEFT_LEG_UP, new MoveInfo(content.Load<Video>("gymnastics\\video\\left_leg_up"), Moves.LEFT_LEG_UP, "Lewa noga w górę"));
            moveVideos.Add(Moves.RIGHT_HAND_DOWN, new MoveInfo(content.Load<Video>("gymnastics\\video\\right_hand_down"), Moves.RIGHT_HAND_DOWN, "Prawa ręka w dół"));
            moveVideos.Add(Moves.RIGHT_HAND_LEFT, new MoveInfo(content.Load<Video>("gymnastics\\video\\right_hand_left"), Moves.RIGHT_HAND_LEFT, "Prawa ręka w lewo"));
            moveVideos.Add(Moves.RIGHT_HAND_RIGHT, new MoveInfo(content.Load<Video>("gymnastics\\video\\right_hand_right"), Moves.RIGHT_HAND_RIGHT, "Prawa ręka w prawo"));
            moveVideos.Add(Moves.RIGHT_HAND_UP, new MoveInfo(content.Load<Video>("gymnastics\\video\\right_hand_up"), Moves.RIGHT_HAND_UP, "Prawa ręka w górę"));
            moveVideos.Add(Moves.RIGHT_LEG_LEFT, new MoveInfo(content.Load<Video>("gymnastics\\video\\right_leg_left"), Moves.RIGHT_LEG_LEFT, "Prawa noga w lewo"));
            moveVideos.Add(Moves.RIGHT_LEG_RIGHT, new MoveInfo(content.Load<Video>("gymnastics\\video\\right_leg_right"), Moves.RIGHT_LEG_RIGHT, "Prawa noga w prawo"));
            moveVideos.Add(Moves.RIGHT_LEG_UP, new MoveInfo(content.Load<Video>("gymnastics\\video\\right_leg_up"), Moves.RIGHT_LEG_UP, "Prawa noga w górę"));
        }

        public GameType update(Rectangle cursorRect)
        {
            throw new NotImplementedException();
        }

        public GameType update(Rectangle cursorRect, Skeleton skeleton, Kinect.WordCommands command)
        {
            switch (command)
            {
                case Kinect.WordCommands.START:
                    {
                        if (gameMode == Mode.START)
                        {
                            time.isEnabled = true;
                            gameMode = Mode.PLAY;
                        }

                        if (gameMode == Mode.FINISH)
                        {
                            resetGame();
                        }
                        break;
                    }
                case Kinect.WordCommands.STOP:
                    {
                        if (player.State != MediaState.Stopped) player.Stop();

                        if (gameMode == Mode.FINISH || gameMode == Mode.START)
                        {
                            sounds.PlaySound(AvailableSounds.BTN_ON_CLICK);
                            return GameType.Kindergarten;
                        }
                        else
                        {
                            //sounds.PlaySound(AvailableSounds.FANFARE);
                            time.cleanTimer();
                            gameMode = Mode.FINISH;
                        }

                        break;
                    }
            }

            switch (gameMode)
            {
                case Mode.START:
                    {
                        return GameType.Gymnastics;
                    }
                case Mode.PLAY:
                    {
                        controller.updateSkeleton(skeleton);
                        controller.incrementTime();

                        if (isMoveDone)
                        {
                            isMoveDone = false;
                            points++;
                            controller.getRandomMove();
                            player.Stop();
                        }
                        else
                        {
                            isMoveDone = controller.isMoveDone();
                        }

                        //---------------------------------
                        // TIMER
                        if (time.isWorking && time.isEnabled)
                        {
                            time.countTimeLeft();
                        }
                        else if (time.isEnabled && !time.isWorking)
                        {
                            time.startTimer();
                        }

                        if (time.seconds < 0 && time.isEnabled)
                        {
                            //Czas sie skonczyl
                            //sounds.PlaySound(AvailableSounds.FANFARE);
                            time.cleanTimer();
                            gameMode = Mode.FINISH;
                        } 
                        //---------------------------------


                        //---------------------------------
                        // VIDEO
                        if (player.State == MediaState.Stopped)
                        {
                            player.IsLooped = true;
                            player.Play(moveVideos[controller.getMove()].video);
                        }
                        //---------------------------------
                        // AUDIO
                        PlayMoveSound(controller.getMove());
                        //---------------------------------

                        return GameType.Gymnastics;
                    }
                case Mode.FINISH:
                    {
                        sounds.PlaySoundOnce(AvailableSounds.END_GAME);

                        if (!cloud.isSent)
                        {
                            cloud.isSent = true;
                            setScoreToCloud();
                            //bestScore = Json.Deserialize(cloud.response);
                        }
                        return GameType.Gymnastics;
                    }
            }
            
            return GameType.Gymnastics;
        }

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(txBackground, Vector2.Zero, Color.White);

            switch (gameMode)
            {
                case Mode.START:
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
                case Mode.PLAY:
                    {
                        Utils.DrawStringAlign(spriteBatch, fonts.Calibri40, moveVideos[controller.getMove()].name, new Rectangle(152, 343, 490, 283), Utils.TextAlignment.Center, Color.Black);
                        
                        spriteBatch.Draw(txPointsFrame, new Vector2(25, 10), Color.White);
                        spriteBatch.Draw(txPointsFrame, new Vector2(25, 170), Color.White);
                        
                        Utils.DrawStringAlign(spriteBatch, fonts.Roboto60, getPointsFormatted(), new Rectangle(25, 170, txPointsFrame.Width, txPointsFrame.Height), Utils.TextAlignment.Center, Color.DimGray);

                        spriteBatch.Draw(txClock, new Vector2(30, (10+160)-48-10), null, Color.White, 0.0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0.0f);
                        spriteBatch.Draw(txPrize, new Vector2(30, (170+160)-48-10), null, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
                        //spriteBatch.Draw(txTime, new Vector2(27, 10), Color.White);
                        //spriteBatch.Draw(txPoints, new Vector2(27, 170), Color.White);
                        
                        if (time.minutes == 0 && time.seconds <= 10)
                            Utils.DrawStringAlign(spriteBatch, fonts.Roboto60, time.getMinutesFormatted() + ":" + time.getSecondsFormatted(), new Rectangle(25, 10, txPointsFrame.Width, txPointsFrame.Height), Utils.TextAlignment.Center, Color.Red);
                        else
                            Utils.DrawStringAlign(spriteBatch, fonts.Roboto60, time.getMinutesFormatted() + ":" + time.getSecondsFormatted(), new Rectangle(25, 10, txPointsFrame.Width, txPointsFrame.Height), Utils.TextAlignment.Center, Color.Black);

                        //-------------------------------------------------------------------------------
                        // VIDEO
                        if (player.State != MediaState.Stopped) videoTexture = player.GetTexture();

                        if (videoTexture != null) spriteBatch.Draw(videoTexture, screen, Color.White);
                        //-------------------------------------------------------------------------------

                        spriteBatch.Draw(txMainFrames, Vector2.Zero, Color.White);
                        spriteBatch.Draw(txFlower, new Vector2(300, 240), null, Color.White, 0.0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0.0f);
                        spriteBatch.Draw(txButterfly, new Vector2(Settings.WINDOW_WIDTH-90, 10), null, Color.White, 0.0f, Vector2.Zero, 0.3f, SpriteEffects.None, 0.0f);

                        break;
                    }
                case Mode.FINISH:
                    {
                        spriteBatch.Draw(txFinish, Vector2.Zero, Color.White);
                        spriteBatch.DrawString(fonts.Pericles46, points.ToString(), new Vector2(170, 220), Color.OrangeRed);

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

        void resetGame()
        {
            getScoreFromCloud();
            time.cleanTimer();
            time.isEnabled = true;
            isMoveDone = false;
            points = 0;
            gameMode = Mode.PLAY;
            sounds.resetSounds();
        }

        string getPointsFormatted()
        {
            if (points >= 10)
                return points.ToString();
            else
                return "0" + points.ToString();
        }

        public void getScoreFromCloud()
        {
            cloud.getScore(GameType.Gymnastics, 5);
        }

        public void setScoreToCloud()
        {
            cloud.setScore(GameType.Gymnastics, points);
        }

        void PlayMoveSound(Moves move)
        {
            switch (move)
            {
                case Moves.HEAD_LEFT:
                    {
                        sounds.PlaySound(AvailableSounds.HEAD_LEFT);
                        break;
                    }
                case Moves.HEAD_RIGHT:
                    {
                        sounds.PlaySound(AvailableSounds.HEAD_RIGHT);
                        break;
                    }
                case Moves.HIP_DOWN:
                    {
                        sounds.PlaySound(AvailableSounds.HIP_DOWN);
                        break;
                    }
                case Moves.HIP_UP:
                    {
                        sounds.PlaySound(AvailableSounds.HIP_UP);
                        break;
                    }
                case Moves.LEFT_HAND_DOWN:
                    {
                        sounds.PlaySound(AvailableSounds.LEFT_HAND_DOWN);
                        break;
                    }
                case Moves.LEFT_HAND_LEFT:
                    {
                        sounds.PlaySound(AvailableSounds.LEFT_HAND_LEFT);
                        break;
                    }
                case Moves.LEFT_HAND_RIGHT:
                    {
                        sounds.PlaySound(AvailableSounds.LEFT_HAND_RIGHT);
                        break;
                    }
                case Moves.LEFT_HAND_UP:
                    {
                        sounds.PlaySound(AvailableSounds.LEFT_HAND_UP);
                        break;
                    }
                case Moves.LEFT_LEG_LEFT:
                    {
                        sounds.PlaySound(AvailableSounds.LEFT_LEG_LEFT);
                        break;
                    }
                case Moves.LEFT_LEG_RIGHT:
                    {
                        sounds.PlaySound(AvailableSounds.LEFT_LEG_RIGHT);
                        break;
                    }
                case Moves.LEFT_LEG_UP:
                    {
                        sounds.PlaySound(AvailableSounds.LEFT_LEG_UP);
                        break;
                    }
                case Moves.RIGHT_HAND_DOWN:
                    {
                        sounds.PlaySound(AvailableSounds.RIGHT_HAND_DOWN);
                        break;
                    }
                case Moves.RIGHT_HAND_LEFT:
                    {
                        sounds.PlaySound(AvailableSounds.RIGHT_HAND_LEFT);
                        break;
                    }
                case Moves.RIGHT_HAND_RIGHT:
                    {
                        sounds.PlaySound(AvailableSounds.RIGHT_HAND_RIGHT);
                        break;
                    }
                case Moves.RIGHT_HAND_UP:
                    {
                        sounds.PlaySound(AvailableSounds.RIGHT_HAND_UP);
                        break;
                    }
                case Moves.RIGHT_LEG_LEFT:
                    {
                        sounds.PlaySound(AvailableSounds.RIGHT_LEG_LEFT);
                        break;
                    }
                case Moves.RIGHT_LEG_RIGHT:
                    {
                        sounds.PlaySound(AvailableSounds.RIGHT_LEG_RIGHT);
                        break;
                    }
                case Moves.RIGHT_LEG_UP:
                    {
                        sounds.PlaySound(AvailableSounds.RIGHT_LEG_UP);
                        break;
                    }
            }
        }
    }
}
