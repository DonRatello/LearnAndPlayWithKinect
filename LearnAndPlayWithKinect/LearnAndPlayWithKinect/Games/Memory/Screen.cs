using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using LearnAndPlayWithKinect.Other;
using LearnAndPlayWithKinect.Interfaces;
using LearnAndPlayWithKinect.Cloud;
using LearnAndPlayWithKinect.Sounds;
using Newtonsoft.Json;

namespace LearnAndPlayWithKinect.Games.Memory
{
    class Screen : iScreen, iCloudGame
    {
        //288x96

        Fonts fonts;
        Services cloud;

        Texture2D txProgressBar;
        Texture2D txBackground;
        Texture2D txCardBack;
        Texture2D txPoints;
        Texture2D txFinishBackground;
        Texture2D txNoConnection;

        SoundVault sounds;
        //----------------------------------------------
        //                TEX ZWIERZAT
        //----------------------------------------------
        Texture2D txLion;
        Texture2D txDeer;
        Texture2D txSwan;
        Texture2D txMonkey;
        Texture2D txSheep;
        Texture2D txBear;
        Texture2D txLlama;
        Texture2D txFish;
        Texture2D txMeerkat;
        Texture2D txCamel;
        //----------------------------------------------

        List<MemoryCard> cards;
        CardPositions position;

        Vector2 posProgressBar;
        Rectangle rectBarFragment_100;
        Rectangle rectBarFragment_75;
        Rectangle rectBarFragment_50;
        Rectangle rectBarFragment_25;
        Rectangle rectBarFragment_0;

        Rectangle rectBtnReply = new Rectangle(203, 441, 288, 96);
        Rectangle rectBtnExit = new Rectangle(509, 441, 288, 96);
        int replyProgress = 0;
        int exitProgress = 0;

        /// <summary>
        /// Czas trzymania kursora na karcie
        /// </summary>
        int progress = 0;

        /// <summary>
        /// Zmienna okreslajaca czas niedotepnosci planszy w momencie odkrycia zlej karty
        /// </summary>
        int wrongCardTimeout = 0;

        /// <summary>
        /// Ilosc punktow gracza
        /// </summary>
        int points = 0;

        bool isGameFinished = false;

        MemoryCard firstPickedCard;

        public Screen(Fonts fonts)
        {
            this.fonts = fonts;
            this.cloud = new Services();

            cards = new List<MemoryCard>();
            position = new CardPositions();
            firstPickedCard = cleanCard();

            sounds = new SoundVault(new List<AvailableSounds>() 
                { 
                    AvailableSounds.END_GAME, 
                    AvailableSounds.CARTOONSPLIT,
                    AvailableSounds.CARTOON_HOP,
                }, 1);
        }

        public void loadContent(ContentManager content)
        {
            Console.WriteLine("**************** LOAD CONTENT: Memory Screen ****************");
            sounds.loadContent(content);
            getScoreFromCloud();

            txBackground = content.Load<Texture2D>("memory\\background");
            txPoints = content.Load<Texture2D>("memory\\points");
            txFinishBackground = content.Load<Texture2D>("memory\\MemoryFinish");
            txProgressBar = content.Load<Texture2D>("kindergarten\\progress_bar");
            txNoConnection = content.Load<Texture2D>("BrakPolaczenia");

            txBear = content.Load<Texture2D>("memory\\bear");
            txCamel = content.Load<Texture2D>("memory\\camel");
            txDeer = content.Load<Texture2D>("memory\\deer");
            txFish = content.Load<Texture2D>("memory\\fish");
            txLion = content.Load<Texture2D>("memory\\lion");
            txLlama = content.Load<Texture2D>("memory\\llama");
            txMeerkat = content.Load<Texture2D>("memory\\meerkat");
            txMonkey = content.Load<Texture2D>("memory\\monkey");
            txSheep = content.Load<Texture2D>("memory\\sheep");
            txSwan = content.Load<Texture2D>("memory\\swan");
            txCardBack = content.Load<Texture2D>("memory\\cardBack");

            posProgressBar = new Vector2((Settings.WINDOW_WIDTH / 2) - 169, Settings.WINDOW_HEIGHT - 50);
            rectBarFragment_100 = new Rectangle(0, 56 * 0, txProgressBar.Width, 24);
            rectBarFragment_75 = new Rectangle(0, 56 * 1, txProgressBar.Width, 24);
            rectBarFragment_50 = new Rectangle(0, 113, txProgressBar.Width, 24);
            rectBarFragment_25 = new Rectangle(0, 170, txProgressBar.Width, 24);
            rectBarFragment_0 = new Rectangle(0, 226, txProgressBar.Width, 24);

            generateCards();
        }

        public GameType update(Rectangle cursorRect)
        {
            if (wrongCardTimeout > 0)
            {
                wrongCardTimeout--;
                return GameType.Memory;
            }

            if (!isGameFinished) isGameFinished = isFinish();

            if (isGameFinished)
            {
                sounds.PlaySoundOnce(AvailableSounds.END_GAME);
                if (!cloud.isSent)
                {
                    setScoreToCloud();
                    cloud.isSent = true;

                    //Pobranie TOP10
                    //bestScores = Json.Deserialize(cloud.response);
                }

                if (cursorRect.Intersects(rectBtnReply))
                {
                    replyProgress++;
                }
                else
                {
                    if (replyProgress - 1 > 0) replyProgress--;
                    else replyProgress = 0;
                }

                if (cursorRect.Intersects(rectBtnExit))
                {
                    exitProgress++;
                }
                else
                {
                    if (exitProgress - 1 > 0) exitProgress--;
                    else exitProgress = 0;
                }

                if (replyProgress > 100)
                {
                    sounds.PlaySound(AvailableSounds.CARTOON_HOP);
                    resetBoard();
                }

                if (exitProgress > 100)
                {
                    sounds.PlaySound(AvailableSounds.CARTOON_HOP);
                    return GameType.Kindergarten;
                } 

                return GameType.Memory;
            }

            if (firstPickedCard.cardType == CardType.NOT_PICKED) cleanUnmatchedCards();

            foreach (MemoryCard card in cards)
            {
                if (cursorRect.Intersects(card.cardRectangle) && !card.isVisible)
                {
                    card.clickProgress++;
                }
                else
                {
                    if (card.clickProgress - 1 > 0) card.clickProgress--;
                    else card.clickProgress = 0;
                }

                if (card.clickProgress > 0) progress = card.clickProgress;

                if (card.clickProgress >= 50)
                {
                    card.isVisible = true;
                    sounds.PlaySound(AvailableSounds.CARTOONSPLIT);

                    if (firstPickedCard.cardType == CardType.NOT_PICKED && firstPickedCard.cardId != card.cardId)
                    {
                        // Pierwszy ruch
                        firstPickedCard = card;
                    }
                    else
                    {
                        // Druga karta
                        if (firstPickedCard.cardId == card.cardId)
                        {
                            // Karta druga jest taka sama jak pierwsza
                        }
                        else if (firstPickedCard.cardType == card.cardType && firstPickedCard.cardId != card.cardId)
                        {
                            // Karty sie zgadzaja
                            firstPickedCard = cleanCard();
                            points += 10;
                        }
                        else
                        {
                            // Karty sie nie zgadzaja
                            wrongCardTimeout = 150;
                            firstPickedCard = cleanCard();
                            points -= 2;
                            card.clickProgress = 0;
                        }
                    }
                }
            }

            return GameType.Memory;
        }

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(txBackground, Vector2.Zero, Color.White);
            spriteBatch.Draw(txPoints, new Vector2(810, 100), Color.White);

            foreach (MemoryCard animal in cards)
            {
                if (animal.isVisible) spriteBatch.Draw(animal.cardTexture, animal.cardPosition, Color.White);
                else spriteBatch.Draw(txCardBack, animal.cardPosition, Color.White);
            }

            spriteBatch.DrawString(fonts.Motorwerk, points.ToString(), new Vector2(825, 150), Color.Black);

            if (isGameFinished)
            {
                spriteBatch.Draw(txFinishBackground, new Vector2(0, 140), Color.White);
                spriteBatch.DrawString(fonts.Motorwerk, points.ToString(), new Vector2(280, 235), Color.Tomato);

                //TOP 10
                if (cloud.bestScores != null)
                {
                    for (int i = 0; i < cloud.bestScores.data.Count; i++)
                    {
                        switch (i)
                        {
                            case 0:
                                {
                                    spriteBatch.DrawString(fonts.Kootenay22, (i + 1).ToString() + ".", new Vector2(570, 220 + i * 40), Color.White);
                                    spriteBatch.DrawString(fonts.Kootenay22, cloud.bestScores.data[i].score, new Vector2(620, 220 + i * 40), Color.Gold);
                                    spriteBatch.DrawString(fonts.Kootenay22, cloud.bestScores.data[i].user, new Vector2(700, 220 + i * 40), Color.Gold);
                                    break;
                                }
                            default:
                                {
                                    spriteBatch.DrawString(fonts.Kootenay22, (i + 1).ToString() + ".", new Vector2(570, 220 + i * 40), Color.White);
                                    spriteBatch.DrawString(fonts.Kootenay22, cloud.bestScores.data[i].score, new Vector2(620, 220 + i * 40), Color.White);
                                    spriteBatch.DrawString(fonts.Kootenay22, cloud.bestScores.data[i].user, new Vector2(700, 220 + i * 40), Color.White);
                                    break;
                                }
                        }
                    }
                }
                else
                {
                    spriteBatch.Draw(txNoConnection, new Vector2(670, 190), Color.White);
                }
            }

            // 0% - 0
            // 25% - 12
            // 50% - 25
            // 75% - 37
            // 100% - 50
            if (progress >= 0 && progress < 12) spriteBatch.Draw(txProgressBar, posProgressBar, rectBarFragment_0, Color.White);
            if (progress >= 12 && progress < 25) spriteBatch.Draw(txProgressBar, posProgressBar, rectBarFragment_25, Color.White);
            if (progress >= 25 && progress < 37) spriteBatch.Draw(txProgressBar, posProgressBar, rectBarFragment_50, Color.White);
            if (progress >= 37 && progress < 50) spriteBatch.Draw(txProgressBar, posProgressBar, rectBarFragment_75, Color.White);
            if (progress >= 50) spriteBatch.Draw(txProgressBar, posProgressBar, rectBarFragment_100, Color.White);
        }

        void generateCards()
        {
            cards.Add(new MemoryCard(0, position.getRandomPosition(), txBear, CardType.Bear));
            cards.Add(new MemoryCard(1, position.getRandomPosition(), txBear, CardType.Bear));

            cards.Add(new MemoryCard(2, position.getRandomPosition(), txCamel, CardType.Camel));
            cards.Add(new MemoryCard(3, position.getRandomPosition(), txCamel, CardType.Camel));

            cards.Add(new MemoryCard(4, position.getRandomPosition(), txDeer, CardType.Deer));
            cards.Add(new MemoryCard(5, position.getRandomPosition(), txDeer, CardType.Deer));

            cards.Add(new MemoryCard(6, position.getRandomPosition(), txFish, CardType.Fish));
            cards.Add(new MemoryCard(7, position.getRandomPosition(), txFish, CardType.Fish));

            cards.Add(new MemoryCard(8, position.getRandomPosition(), txLion, CardType.Lion));
            cards.Add(new MemoryCard(9, position.getRandomPosition(), txLion, CardType.Lion));

            cards.Add(new MemoryCard(10, position.getRandomPosition(), txLlama, CardType.Llama));
            cards.Add(new MemoryCard(11, position.getRandomPosition(), txLlama, CardType.Llama));

            cards.Add(new MemoryCard(12, position.getRandomPosition(), txMeerkat, CardType.Meerkat));
            cards.Add(new MemoryCard(13, position.getRandomPosition(), txMeerkat, CardType.Meerkat));

            cards.Add(new MemoryCard(14, position.getRandomPosition(), txMonkey, CardType.Monkey));
            cards.Add(new MemoryCard(15, position.getRandomPosition(), txMonkey, CardType.Monkey));

            cards.Add(new MemoryCard(16, position.getRandomPosition(), txSheep, CardType.Sheep));
            cards.Add(new MemoryCard(17, position.getRandomPosition(), txSheep, CardType.Sheep));

            cards.Add(new MemoryCard(18, position.getRandomPosition(), txSwan, CardType.Swan));
            cards.Add(new MemoryCard(19, position.getRandomPosition(), txSwan, CardType.Swan));
        }

        void cleanUnmatchedCards()
        {
            bool hasPair = false;

            for (int i = 0; i < cards.Count; i++)
            {
                if (cards.ElementAt(i).isVisible)
                {
                    for (int j = 0; j < cards.Count; j++)
                    {
                        if (i != j)
                        {
                            if (cards.ElementAt(i).cardType == cards.ElementAt(j).cardType && cards.ElementAt(j).isVisible)
                            {
                                hasPair = true;
                                break;
                            }
                        }
                    }
                }

                if (!hasPair)
                {
                    cards.ElementAt(i).isVisible = false;
                }

                hasPair = false;
            }
        }

        MemoryCard cleanCard()
        {
            return new MemoryCard(100, Vector2.Zero, null, CardType.NOT_PICKED); 
        }

        bool isFinish()
        {
            bool isFinish = true;

            foreach (MemoryCard card in cards)
            {
                if (!card.isVisible)
                {
                    isFinish = false;
                    break;
                }
            }

            return isFinish;
        }

        void resetBoard()
        {
            isGameFinished = false;
            cloud.isSent = false;
            cards = new List<MemoryCard>();
            position = new CardPositions();
            firstPickedCard = cleanCard();
            generateCards();

            replyProgress = 0;
            exitProgress = 0;
            points = 0;

            getScoreFromCloud();
            sounds.resetSounds();
        }

        public void setScoreToCloud()
        {
            cloud.setScore(GameType.Memory, points);
        }

        public void getScoreFromCloud()
        {
            cloud.getScore(GameType.Memory, 5);
        }
    }
}
