using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using LearnAndPlayWithKinect.Other;
using LearnAndPlayWithKinect.Interfaces;
using LearnAndPlayWithKinect.Sounds;

namespace LearnAndPlayWithKinect.Games.EngColors
{
    class Screen : iScreen
    {
        Texture2D txBackground;
        Texture2D txColorUnknown;
        Texture2D txColorPadArea;
        Texture2D txTrash;
        Texture2D txFinish;

        Answer answer;
        SoundVault soundsColor;
        SoundVault soundsAnswer;

        Rectangle rectTrash;
        Rectangle rectAnswerArea;
        Rectangle rectBtnReply = new Rectangle(203, 441, 288, 96);
        Rectangle rectBtnExit = new Rectangle(509, 441, 288, 96);

        Fonts fonts;

        List<ColorCard> cardList;
        Dictionary<Colors, Texture2D> guessColorName;
        List<Texture2D> squareBarParts;

        bool cardPicked = false;
        bool isFinished = false;

        Colors colorToGuess;

        public Screen(Fonts fonts)
        {
            this.fonts = fonts;
            cardList = new List<ColorCard>();
            guessColorName = new Dictionary<Colors, Texture2D>();
            squareBarParts = new List<Texture2D>();

            soundsColor = new SoundVault(new List<AvailableSounds>() 
                    { 
                          AvailableSounds.GREEN, 
                          AvailableSounds.ORANGE,
                          AvailableSounds.PINK,
                          AvailableSounds.SILVER,
                          AvailableSounds.VIOLET,
                          AvailableSounds.YELLOW,
                          AvailableSounds.RED,
                          AvailableSounds.BLUE,
                    }, 10);

            soundsAnswer = new SoundVault(new List<AvailableSounds>() 
                    { 
                          AvailableSounds.BAD,
                          AvailableSounds.GOOD,
                          AvailableSounds.END_GAME,
                          AvailableSounds.FANFARE,
                          AvailableSounds.CARTOON_HOP
                    }, 2);
        }

        public void loadContent(ContentManager content)
        {
            Console.WriteLine("**************** LOAD CONTENT: English Colors Screen ****************");
            soundsColor.loadContent(content);
            soundsAnswer.loadContent(content);
            txBackground = content.Load<Texture2D>("eng_colors\\engcolorsBackground");
            txColorUnknown = content.Load<Texture2D>("eng_colors\\colUnknown");
            txColorPadArea = content.Load<Texture2D>("eng_colors\\colorPadArea");
            txTrash = content.Load<Texture2D>("eng_colors\\trash");
            txFinish = content.Load<Texture2D>("eng_colors\\colorsFinish");

            answer = new Answer(content.Load<Texture2D>("eng_colors\\answerGood"), content.Load<Texture2D>("eng_colors\\answerBad"));

            cardList.Add(new ColorCard(content.Load<Texture2D>("eng_colors\\colBlue"), new Vector2(20, 400), Colors.Blue));
            cardList.Add(new ColorCard(content.Load<Texture2D>("eng_colors\\colGreen"), new Vector2(140, 400), Colors.Green));
            cardList.Add(new ColorCard(content.Load<Texture2D>("eng_colors\\colOrange"), new Vector2(260, 400), Colors.Orange));
            cardList.Add(new ColorCard(content.Load<Texture2D>("eng_colors\\colPink"), new Vector2(380, 400), Colors.Pink));
            cardList.Add(new ColorCard(content.Load<Texture2D>("eng_colors\\colRed"), new Vector2(500, 400), Colors.Red));
            cardList.Add(new ColorCard(content.Load<Texture2D>("eng_colors\\colSilver"), new Vector2(620, 400), Colors.Silver));
            cardList.Add(new ColorCard(content.Load<Texture2D>("eng_colors\\colViolet"), new Vector2(740, 400), Colors.Violet));
            cardList.Add(new ColorCard(content.Load<Texture2D>("eng_colors\\colYellow"), new Vector2(860, 400), Colors.Yellow));

            colorToGuess = this.randomColor();
            guessColorName.Add(Colors.Blue, content.Load<Texture2D>("eng_colors\\txtBlue"));
            guessColorName.Add(Colors.Green, content.Load<Texture2D>("eng_colors\\txtGreen"));
            guessColorName.Add(Colors.Orange, content.Load<Texture2D>("eng_colors\\txtOrange"));
            guessColorName.Add(Colors.Pink, content.Load<Texture2D>("eng_colors\\txtPink"));
            guessColorName.Add(Colors.Red, content.Load<Texture2D>("eng_colors\\txtRed"));
            guessColorName.Add(Colors.Silver, content.Load<Texture2D>("eng_colors\\txtSilver"));
            guessColorName.Add(Colors.Violet, content.Load<Texture2D>("eng_colors\\txtViolet"));
            guessColorName.Add(Colors.Yellow, content.Load<Texture2D>("eng_colors\\txtYellow"));

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

            rectAnswerArea = new Rectangle(650, 65, txColorPadArea.Width, txColorPadArea.Height);
            rectTrash = new Rectangle(50, 50, txTrash.Width, txTrash.Height);
        }

        public GameType update(Rectangle cursorRect)
        {
            if (isFinished)
            {
                soundsAnswer.PlaySoundOnce(AvailableSounds.END_GAME);

                if (cursorRect.Intersects(rectBtnExit))
                {
                    soundsAnswer.PlaySound(AvailableSounds.CARTOON_HOP);
                    return GameType.School;
                } 

                if (cursorRect.Intersects(rectBtnReply))
                {
                    soundsAnswer.PlaySound(AvailableSounds.CARTOON_HOP);
                    resetGame();
                    return GameType.EngColors;
                }
            }
            else
            {
                answer.decreaseTime();

                if (answer.responseTime > 0) return GameType.EngColors;

                foreach (ColorCard card in cardList)
                {
                    if (!this.cardPicked)
                    {
                        //Karta nie jest wzieta wiec sprawdzam tylko karty z kolorami
                        if (cursorRect.Intersects(card.cardRectangle))
                        {
                            if (card.cardProgress + 1 < 60) card.cardProgress++;
                            else card.cardProgress = 60;
                        }
                        else
                        {
                            if (card.cardProgress - 1 >= 0) card.cardProgress--;
                            else card.cardProgress = 0;
                        }

                        if (card.cardProgress == 60)
                        {
                            card.cardPicked = true;
                            this.cardPicked = true;

                            //Przesuniecie elementu z listy na koniec po to aby byl on zawsze na gorze
                            //a nie przykryty przez inne karty
                            //break na koncu bo lista z foreach sie zmienia
                            ColorCard cc = card;
                            cardList.Remove(card);
                            cardList.Add(cc);
                            break;
                        }
                    }
                    else
                    {
                        //Karta zabrana wiec sprawdzam prostokaty TRASH i ANSWER_AREA
                        if (cursorRect.Intersects(rectAnswerArea) && card.cardPicked)
                        {
                            if (card.cardColor == colorToGuess)
                            {
                                card.cardMatched = true;
                                card.cardPicked = false;
                                this.cardPicked = false;

                                answer.setAnswer(AnswerType.Good, 100);

                                colorToGuess = randomColor();
                                PlayColorSoundDelayed(4);
                            }
                            else
                            {
                                card.cardProgress = 0;
                                card.cardPicked = false;
                                card.cardPosition = card.cardPositionOriginal;
                                this.cardPicked = false;

                                answer.setAnswer(AnswerType.Bad, 100);
                                PlayColorSoundDelayed(2);
                            }
                        }
                        if (cursorRect.Intersects(rectTrash) && card.cardPicked)
                        {
                            card.cardProgress = 0;
                            card.cardPicked = false;
                            card.cardPosition = card.cardPositionOriginal;
                            this.cardPicked = false;
                        }
                    }

                    if (card.cardPicked) card.cardPosition = new Vector2(cursorRect.X - 50, cursorRect.Y - 58);

                    if (answer.responseTime > 0)
                    {
                        switch (answer.getAnswerType())
                        {
                            case AnswerType.Bad:
                                {
                                    soundsAnswer.PlaySound(AvailableSounds.BAD);
                                    break;
                                }
                            case AnswerType.Good:
                                {
                                    soundsAnswer.PlaySound(AvailableSounds.GOOD);
                                    break;
                                }
                        }
                    }
                }

                return GameType.EngColors;
            }

            return GameType.EngColors;
        }

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(txBackground, Vector2.Zero, Color.White);

            if (isFinished)
            {
                spriteBatch.Draw(txFinish, new Vector2(0, 140), Color.White);
            }
            else
            {
                spriteBatch.Draw(txColorUnknown, new Vector2(700, 100), Color.White);
                spriteBatch.Draw(txColorPadArea, new Vector2(650, 65), Color.White);
                spriteBatch.Draw(guessColorName[colorToGuess], new Vector2(50, 110), Color.White);
                spriteBatch.Draw(txTrash, new Vector2(50, 50), Color.White);
                spriteBatch.DrawString(fonts.Kootenay22, "Anuluj", new Vector2(30, 100), Color.OrangeRed);

                foreach (ColorCard card in cardList)
                {
                    if (!card.cardMatched)
                    {
                        spriteBatch.Draw(card.cardTexture, card.cardPosition, Color.White);
                        if (card.cardProgress > 0) spriteBatch.Draw(getCardProgressTex(card.cardProgress), card.cardPosition, Color.White);
                    }
                }

                if (answer.responseTime > 0) spriteBatch.Draw(answer.getAnswer(), Vector2.Zero, Color.White * answer.getOpacity());
            }
        }

        private Colors randomColor()
        {
            MersenneTwister mt = new MersenneTwister();

            //Losuje kolor z kart pozostalych na planszy
            List<ColorCard> unusedElements = cardList.Where(c => !c.cardMatched).ToList();

            if (unusedElements.Count == 0)
            {
                soundsAnswer.PlaySound(AvailableSounds.FANFARE);
                isFinished = true;
                return Colors.Blue;
            }

            return unusedElements.ElementAt(mt.Next(0, unusedElements.Count)).cardColor;
        }

        private Texture2D getCardProgressTex(int cardProgress)
        {
            // Elementow na liscie jest 12
            if (cardProgress >= 0 && cardProgress < 5) return squareBarParts.ElementAt(0);
            if (cardProgress >= 5 && cardProgress < 10) return squareBarParts.ElementAt(1);
            if (cardProgress >= 10 && cardProgress < 15) return squareBarParts.ElementAt(2);
            if (cardProgress >= 15 && cardProgress < 20) return squareBarParts.ElementAt(3);
            if (cardProgress >= 20 && cardProgress < 25) return squareBarParts.ElementAt(4);
            if (cardProgress >= 25 && cardProgress < 30) return squareBarParts.ElementAt(5);
            if (cardProgress >= 30 && cardProgress < 35) return squareBarParts.ElementAt(6);
            if (cardProgress >= 35 && cardProgress < 40) return squareBarParts.ElementAt(7);
            if (cardProgress >= 40 && cardProgress < 45) return squareBarParts.ElementAt(8);
            if (cardProgress >= 45 && cardProgress < 50) return squareBarParts.ElementAt(9);
            if (cardProgress >= 50 && cardProgress < 55) return squareBarParts.ElementAt(10);
            if (cardProgress > 55) return squareBarParts.ElementAt(11);

            return squareBarParts.ElementAt(0);
        }

        private void resetGame()
        {
            foreach (ColorCard cc in cardList)
            {
                cc.cardPosition = cc.cardPositionOriginal;
                cc.cardProgress = 0;
                cc.cardMatched = false;
                cc.cardPicked = false;
            }

            cardPicked = false;
            isFinished = false;
            soundsAnswer.resetSounds();
        }

        private void PlayColorSoundDelayed(int seconds)
        {
            Thread thread = new Thread(() => PlayColorSound(true, 2));
            thread.Start();
        }

        private void PlayColorSound(bool notMainThread, int secods = 0)
        {
            if (notMainThread) Thread.Sleep(secods * 1000);

            switch (colorToGuess)
            {
                case Colors.Blue:
                    {
                        soundsColor.PlaySound(AvailableSounds.BLUE);
                        break;
                    }
                case Colors.Green:
                    {
                        soundsColor.PlaySound(AvailableSounds.GREEN);
                        break;
                    }
                case Colors.Orange:
                    {
                        soundsColor.PlaySound(AvailableSounds.ORANGE);
                        break;
                    }
                case Colors.Pink:
                    {
                        soundsColor.PlaySound(AvailableSounds.PINK);
                        break;
                    }
                case Colors.Red:
                    {
                        soundsColor.PlaySound(AvailableSounds.RED);
                        break;
                    }
                case Colors.Silver:
                    {
                        soundsColor.PlaySound(AvailableSounds.SILVER);
                        break;
                    }
                case Colors.Violet:
                    {
                        soundsColor.PlaySound(AvailableSounds.VIOLET);
                        break;
                    }
                case Colors.Yellow:
                    {
                        soundsColor.PlaySound(AvailableSounds.YELLOW);
                        break;
                    }
            }
        }
    }
}
