using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using LearnAndPlayWithKinect.Other;
using LearnAndPlayWithKinect.Sounds;

namespace LearnAndPlayWithKinect.Kinect
{
    class InfoBox
    {
        Texture2D txBackground;
        Texture2D txBtnUp;
        Texture2D txBtnDown;
        Texture2D txBtnBack;

        SoundVault sounds;
        List<Texture2D> squareBarParts;
        
        Vector2 posArrowDown;
        Vector2 posArrowUp;
        Vector2 posArrowBack;
        Vector2 posBackground;
        
        Fonts fonts;

        Rectangle rectArrow;
        Rectangle rectPointer;
        Rectangle rectArrowBack;

        Joint joint;
        double SpeechConfidenceThreshold;

        bool isEnabled;
        bool isVisible;
        bool cursorOnArrow;

        int backProgress;

        public InfoBox(Fonts fonts)
        {
            this.fonts = fonts;
            backProgress = 0;
            cursorOnArrow = false;
            this.SpeechConfidenceThreshold = 0.0f;
            isEnabled = false;
            isVisible = false;
            posBackground = new Vector2(Settings.WINDOW_WIDTH - 300, Settings.WINDOW_HEIGHT - 150);
            posArrowDown = new Vector2(Settings.WINDOW_WIDTH - 187, Settings.WINDOW_HEIGHT - 57);
            posArrowUp = new Vector2(Settings.WINDOW_WIDTH - 187, Settings.WINDOW_HEIGHT - 57);
            posArrowBack = new Vector2(5, Settings.WINDOW_HEIGHT - 53);

            sounds = new SoundVault(new List<AvailableSounds>() { AvailableSounds.BTN_ON_CLICK }, 1);
            squareBarParts = new List<Texture2D>();
        }

        public void loadContent(ContentManager content)
        {
            Console.WriteLine("**************** LOAD CONTENT: InfoBox ****************");
            sounds.loadContent(content);
            txBackground = content.Load<Texture2D>("infoBox");
            txBtnUp = content.Load<Texture2D>("arrow_up");
            txBtnDown = content.Load<Texture2D>("arrow_down");
            txBtnBack = content.Load<Texture2D>("arrow_back");

            squareBarParts.Add(content.Load<Texture2D>("eng_colors\\squareBar1"));
            squareBarParts.Add(content.Load<Texture2D>("eng_colors\\squareBar2"));
            squareBarParts.Add(content.Load<Texture2D>("eng_colors\\squareBar3"));
            squareBarParts.Add(content.Load<Texture2D>("eng_colors\\squareBar4"));
            squareBarParts.Add(content.Load<Texture2D>("eng_colors\\squareBar5"));
            squareBarParts.Add(content.Load<Texture2D>("eng_colors\\squareBar6"));
            squareBarParts.Add(content.Load<Texture2D>("eng_colors\\squareBar7"));
            squareBarParts.Add(content.Load<Texture2D>("eng_colors\\squareBar8"));
            squareBarParts.Add(content.Load<Texture2D>("eng_colors\\squareBar9"));
            squareBarParts.Add(content.Load<Texture2D>("eng_colors\\squareBar10"));
            squareBarParts.Add(content.Load<Texture2D>("eng_colors\\squareBar11"));
            squareBarParts.Add(content.Load<Texture2D>("eng_colors\\squareBar12"));

            rectArrow = new Rectangle((int)posArrowUp.X, (int)posArrowUp.Y, txBtnUp.Width, txBtnUp.Height);
            rectArrowBack = new Rectangle((int)posArrowBack.X, (int)posArrowBack.Y, txBtnBack.Width, txBtnBack.Height);
        }

        public GameType update(Joint joint, Rectangle rectPointer, GameType gameType, double SpeechConfidenceThreshold)
        {
            this.joint = joint;
            this.rectPointer = rectPointer;
            this.SpeechConfidenceThreshold = SpeechConfidenceThreshold;

            if (rectPointer.Intersects(rectArrow))
            {
                if (!cursorOnArrow)
                {
                    if (isEnabled) isEnabled = false;
                    else isEnabled = true;
                    cursorOnArrow = true;
                }
            }
            else
            {
                cursorOnArrow = false;
            }

            if (rectPointer.Intersects(rectArrowBack))
            {
                backProgress++;

                if (backProgress >= 60)
                {
                    backProgress = 0;
                    sounds.PlaySound(AvailableSounds.BTN_ON_CLICK);
                    return this.getPreviousScreen(gameType);
                }
            }
            else
            {
                if (backProgress - 1 >= 0) backProgress--;
                else backProgress = 0;
            }

            return gameType;
        }

        public void draw(SpriteBatch spriteBatch)
        {
            if (!isVisible) return;

            spriteBatch.Draw(txBtnBack, posArrowBack, Color.White);

            if (backProgress > 0)
            {
                float scale = 0.5f; //50% mniejszy
                spriteBatch.Draw(getCardProgressTex(backProgress), new Vector2(posArrowBack.X, posArrowBack.Y-5), null, Color.White, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0.0f);
            }

            if (!isEnabled)
            {
                spriteBatch.Draw(txBtnUp, posArrowUp, Color.White);
            }
            else
            {
                //spriteBatch.Draw(txBackground, posBackground, Color.White);
                spriteBatch.Draw(txBackground, posBackground, null, Color.White, 0.0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0.0f);
                //spriteBatch.Draw(txBtnClose, posBtnClose, Color.White);
                spriteBatch.DrawString(fonts.Miramonte, "Mowa: " + (SpeechConfidenceThreshold * 100).ToString() + "%", new Vector2(Settings.WINDOW_WIDTH - 290, Settings.WINDOW_HEIGHT - 40), Color.Black);

                if (joint != null)
                {
                    spriteBatch.DrawString(fonts.Miramonte, "X: " + joint.Position.X.ToString(), new Vector2(Settings.WINDOW_WIDTH - 280, Settings.WINDOW_HEIGHT - 120), Color.Black);
                    spriteBatch.DrawString(fonts.Miramonte, "Y: " + joint.Position.Y.ToString(), new Vector2(Settings.WINDOW_WIDTH - 280, Settings.WINDOW_HEIGHT - 100), Color.Black);
                    spriteBatch.DrawString(fonts.Miramonte, "Z: " + joint.Position.Z.ToString(), new Vector2(Settings.WINDOW_WIDTH - 280, Settings.WINDOW_HEIGHT - 80), Color.Black);
                    spriteBatch.DrawString(fonts.Miramonte, translateTrackingState(joint.TrackingState), new Vector2(Settings.WINDOW_WIDTH - 290, Settings.WINDOW_HEIGHT - 140), Color.Black);
                    spriteBatch.Draw(txBtnDown, posArrowDown, Color.White);
                }
            }

        }

        /// <summary>
        /// InfoBox jest aktywny i pokazuje informacje
        /// </summary>
        public void Enable()
        {
            isEnabled = true;
        }

        /// <summary>
        /// InfoBox jest schowany i jest pokazana tylko strzalka zeby go aktywowac
        /// </summary>
        public void Disable()
        {
            isEnabled = false;
        }

        /// <summary>
        /// Wlaczenie ikony infoBoxa lub samego infoBoxa
        /// </summary>
        public void Visible()
        {
            this.isVisible = true;
        }

        /// <summary>
        /// Wylaczenie ikony infoBoxa lub samego infoBoxa
        /// </summary>
        public void Invisible()
        {
            this.isVisible = false;
        }

        public Vector2 getPosition()
        {
            return this.posBackground;
        }

        private GameType getPreviousScreen(GameType type)
        {
            switch (type)
            {
                case GameType.Kindergarten:
                    {
                        return GameType.MainMenu;
                    }
                case GameType.School:
                    {
                        return GameType.MainMenu;
                    }
                case GameType.EngColors:
                    {
                        return GameType.School;
                    }
                case GameType.ToyStore:
                    {
                        return GameType.School;
                    }
                case GameType.Memory:
                    {
                        return GameType.Kindergarten;
                    }
                case GameType.Gymnastics:
                    {
                        return GameType.Kindergarten;
                    }
                case GameType.PickingApples:
                    {
                        return GameType.Kindergarten;
                    }
                default:
                    {
                        return GameType.MainMenu;
                    }
            }
        }

        private Texture2D getCardProgressTex(int progress)
        {
            // Elementow na liscie jest 12
            if (progress >= 0 && progress < 5) return squareBarParts.ElementAt(0);
            if (progress >= 5 && progress < 10) return squareBarParts.ElementAt(1);
            if (progress >= 10 && progress < 15) return squareBarParts.ElementAt(2);
            if (progress >= 15 && progress < 20) return squareBarParts.ElementAt(3);
            if (progress >= 20 && progress < 25) return squareBarParts.ElementAt(4);
            if (progress >= 25 && progress < 30) return squareBarParts.ElementAt(5);
            if (progress >= 30 && progress < 35) return squareBarParts.ElementAt(6);
            if (progress >= 35 && progress < 40) return squareBarParts.ElementAt(7);
            if (progress >= 40 && progress < 45) return squareBarParts.ElementAt(8);
            if (progress >= 45 && progress < 50) return squareBarParts.ElementAt(9);
            if (progress >= 50 && progress < 55) return squareBarParts.ElementAt(10);
            if (progress > 55) return squareBarParts.ElementAt(11);

            return squareBarParts.ElementAt(0);
        }

        private string translateTrackingState(JointTrackingState state)
        {
            switch (state)
            {
                case JointTrackingState.Tracked: return "Sledzony";
                case JointTrackingState.NotTracked: return "Szkielet niewykryty";
                case JointTrackingState.Inferred: return "Szkielet zaklocony";
                default: return state.ToString();
            }
        }
    }
}
