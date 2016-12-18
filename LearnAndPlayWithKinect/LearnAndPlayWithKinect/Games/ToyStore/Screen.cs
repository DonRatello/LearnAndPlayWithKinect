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

namespace LearnAndPlayWithKinect.Games.ToyStore
{
    class Screen : iScreen
    {
        Texture2D txBackground;
        Texture2D txCardBackground;
        Texture2D txPayText;
        Texture2D txPayBackground;
        Texture2D txKwotaText;
        Texture2D txDragMoneyHere;
        Texture2D txMoneyHolder;
        Texture2D txArrow;
        Texture2D txPutMoneyHere;
        Texture2D txCashRegister;
        Texture2D txTrash;
        Texture2D txButtonBackground;
        Texture2D txFinish;

        Fonts fonts;
        SoundVault sounds;

        List<ToyCard> cards;
        List<MoneyCard> money;
        List<Texture2D> squareBarParts;
        List<MoneyCard> cashInBank;

        Rectangle rectBtnReply = new Rectangle(203, 441, 288, 96);
        Rectangle rectBtnExit = new Rectangle(509, 441, 288, 96);

        int payBtnProgress;
        Rectangle rectPayBtn;
        Rectangle rectDragMoneyHere;
        Rectangle rectCashRegister;
        Rectangle rectTrash;

        bool cardPicked = false;
        int toPay;
        int inBank;

        StageMode mode;

        public Screen(Fonts fonts)
        {
            this.fonts = fonts;
            cards = new List<ToyCard>();
            money = new List<MoneyCard>();
            squareBarParts = new List<Texture2D>();
            cashInBank = new List<MoneyCard>();
            toPay = 0;
            inBank = 0;
            payBtnProgress = 0;
            mode = StageMode.Normal;

            sounds = new SoundVault(new List<AvailableSounds>() 
                { 
                    AvailableSounds.END_GAME,
                    AvailableSounds.CARTOON_HOP,
                    AvailableSounds.ERROR,
                }, 1);
        }

        public void loadContent(ContentManager content)
        {
            Console.WriteLine("**************** LOAD CONTENT: ToyStore Screen ****************");
            sounds.loadContent(content);
            txBackground = content.Load<Texture2D>("toystore\\toyBackground");
            txCardBackground = content.Load<Texture2D>("toystore\\cardBackground");
            txPayText = content.Load<Texture2D>("toystore\\zaplac_title");
            txPayBackground = content.Load<Texture2D>("toystore\\payBackground");
            txKwotaText = content.Load<Texture2D>("toystore\\kwotaTitle");
            txDragMoneyHere = content.Load<Texture2D>("eng_colors\\colorPadArea");
            txMoneyHolder = content.Load<Texture2D>("toystore\\moneyHolder");
            txArrow = content.Load<Texture2D>("toystore\\arrow");
            txPutMoneyHere = content.Load<Texture2D>("toystore\\PutMoneyHereText");
            txCashRegister = content.Load<Texture2D>("toystore\\cash_register");
            txTrash = content.Load<Texture2D>("toystore\\trash");
            txButtonBackground = content.Load<Texture2D>("toystore\\buttonBackground");
            txFinish = content.Load<Texture2D>("eng_colors\\colorsFinish");

            //-------------------------------------------------------------------------------------------------------------------
            MersenneTwister mt = new MersenneTwister();

            ToyCard card = new ToyCard(content.Load<Texture2D>("toystore\\bmw"), new Vector2(130, 355), mt.Next(5, 100));
            card.cardTextureHighlighted = content.Load<Texture2D>("toystore\\bmw_highlight");
            card.setScale(0.08f);
            card.cardRectangle = new Rectangle((int)card.cardPosition.X, (int)card.cardPosition.Y+10, 199, 110);
            cards.Add(card);

            card = new ToyCard(content.Load<Texture2D>("toystore\\cysterna"), new Vector2(360, 315), mt.Next(5, 100));
            card.cardTextureHighlighted = content.Load<Texture2D>("toystore\\cysterna_highlight");
            card.setScale(0.12f);
            card.cardRectangle = new Rectangle((int)card.cardPosition.X, (int)card.cardPosition.Y+45, 299, 110);
            cards.Add(card);

            card = new ToyCard(content.Load<Texture2D>("toystore\\kangur"), new Vector2(40, 565), mt.Next(5, 100));
            card.cardTextureHighlighted = content.Load<Texture2D>("toystore\\kangur_highlight");
            card.setScale(0.085f);
            card.cardRectangle = new Rectangle((int)card.cardPosition.X+55, (int)card.cardPosition.Y, 100, 130);
            cards.Add(card);

            card = new ToyCard(content.Load<Texture2D>("toystore\\milka"), new Vector2(450, -44), mt.Next(5, 100));
            card.cardTextureHighlighted = content.Load<Texture2D>("toystore\\milka_highlight");
            card.setScale(0.11f);
            card.cardRectangle = new Rectangle((int)card.cardPosition.X + 80, (int)card.cardPosition.Y + 50, 120, 90);
            cards.Add(card);

            card = new ToyCard(content.Load<Texture2D>("toystore\\mysz"), new Vector2(130, -30), mt.Next(5, 100));
            card.cardTextureHighlighted = content.Load<Texture2D>("toystore\\mysz_highlight");
            card.setScale(0.09f);
            card.cardRectangle = new Rectangle((int)card.cardPosition.X + 65, (int)card.cardPosition.Y + 30, 100, 100);
            cards.Add(card);

            card = new ToyCard(content.Load<Texture2D>("toystore\\pokemon"), new Vector2(-10, 95), mt.Next(5, 100));
            card.cardTextureHighlighted = content.Load<Texture2D>("toystore\\pokemon_highlight");
            card.setScale(0.15f);
            card.cardRectangle = new Rectangle((int)card.cardPosition.X+130, (int)card.cardPosition.Y+90, 120, 90);
            cards.Add(card);

            card = new ToyCard(content.Load<Texture2D>("toystore\\us"), new Vector2(350, 115), mt.Next(5, 100));
            card.cardTextureHighlighted = content.Load<Texture2D>("toystore\\us_highlight");
            card.setScale(0.11f);
            card.cardRectangle = new Rectangle((int)card.cardPosition.X + 80, (int)card.cardPosition.Y + 35, 130, 130);
            cards.Add(card);

            card = new ToyCard(content.Load<Texture2D>("toystore\\wywrotka"), new Vector2(320, 495), mt.Next(5, 100));
            card.cardTextureHighlighted = content.Load<Texture2D>("toystore\\wywrotka_highlight");
            card.setScale(0.16f);
            card.cardRectangle = new Rectangle((int)card.cardPosition.X + 70, (int)card.cardPosition.Y+90, 250, 120);
            cards.Add(card);
            //-------------------------------------------------------------------------------------------------------------------

            MoneyCard cash = new MoneyCard(content.Load<Texture2D>("toystore\\1zl"), new Vector2(760, 10), 1, MoneyType.Coin);
            cash.setScale(0.6f);
            money.Add(cash);

            cash = new MoneyCard(content.Load<Texture2D>("toystore\\2zl"), new Vector2(760, 90), 2, MoneyType.Coin);
            cash.setScale(0.6f);
            money.Add(cash);

            cash = new MoneyCard(content.Load<Texture2D>("toystore\\5zl"), new Vector2(760, 170), 5, MoneyType.Coin);
            cash.setScale(0.6f);
            money.Add(cash);

            cash = new MoneyCard(content.Load<Texture2D>("toystore\\10zl"), new Vector2(860, 5), 10, MoneyType.Banknote);
            cash.setScale(0.4f);
            money.Add(cash);

            cash = new MoneyCard(content.Load<Texture2D>("toystore\\20zl"), new Vector2(860, 80), 20, MoneyType.Banknote);
            cash.setScale(0.4f);
            money.Add(cash);

            cash = new MoneyCard(content.Load<Texture2D>("toystore\\50zl"), new Vector2(860, 155), 50, MoneyType.Banknote);
            cash.setScale(0.4f);
            money.Add(cash);

            cash = new MoneyCard(content.Load<Texture2D>("toystore\\100zl"), new Vector2(860, 230), 100, MoneyType.Banknote);
            cash.setScale(0.4f);
            money.Add(cash);

            cash = new MoneyCard(content.Load<Texture2D>("toystore\\200zl"), new Vector2(860, 305), 200, MoneyType.Banknote);
            cash.setScale(0.4f);
            money.Add(cash);

            //-------------------------------------------------------------------------------------------------------------------

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

            //-------------------------------------------------------------------------------------------------------------------

            rectPayBtn = new Rectangle(865, 20, squareBarParts[0].Width, squareBarParts[0].Height);
            rectDragMoneyHere = new Rectangle(250, 200, (int)(txPutMoneyHere.Width * 0.4f), (int)(txPutMoneyHere.Height * 0.4f));
            rectCashRegister = new Rectangle(14, 150, (int)(txButtonBackground.Width * 0.35f), (int)(txButtonBackground.Height * 0.35f));
            rectTrash = new Rectangle(14, 235, (int)(txButtonBackground.Width * 0.35f), (int)(txButtonBackground.Height * 0.35f));

            //-------------------------------------------------------------------------------------------------------------------
        }

        public GameType update(Rectangle cursorRect)
        {
            switch (mode)
            {
                case StageMode.Normal:
                    {
                        if (cursorRect.Intersects(rectPayBtn))
                        {
                            if ((payBtnProgress + 1) < 60) payBtnProgress++;
                            else payBtnProgress = 60;
                        }
                        else
                        {
                            if ((payBtnProgress - 1) >= 0) payBtnProgress--;
                            else payBtnProgress = 0;
                        }

                        if (payBtnProgress >= 60)
                        {
                            payBtnProgress = 0;
                            mode = StageMode.Paying;
                        }

                        foreach (ToyCard card in cards)
                        {
                            if (card.cardRectangle.Intersects(cursorRect) && !card.isChoosen)
                            {
                                card.isChoosen = true;
                                toPay += card.toyPrize;
                            }
                        }

                        return GameType.ToyStore;
                    }
                case StageMode.Paying:
                    {
                        foreach (MoneyCard cash in money)
                        {
                            if (cursorRect.Intersects(cash.cardRectangle) && !cardPicked)
                            {
                                if ((cash.cardProgress + 1) < 60) cash.cardProgress++;
                                else cash.cardProgress = 60;
                            }
                            else
                            {
                                if ((cash.cardProgress - 1) >= 0) cash.cardProgress--;
                                else cash.cardProgress = 0;
                            }

                            if (cash.isChoosen)
                            {
                                cash.cardPosition = new Vector2(cursorRect.X, cursorRect.Y);
                            }

                            if (cash.cardProgress >= 60 && !cardPicked)
                            {
                                cash.cardProgress = 0;
                                cash.isChoosen = true;
                                cardPicked = true;
                            }

                            if (cursorRect.Intersects(rectDragMoneyHere) && cardPicked && cash.isChoosen)
                            {
                                //Mamy pieniadz w rece i kladziemy go w polu
                                cash.cardPosition = cash.originPosition;
                                cash.cardProgress = 0;
                                cardPicked = false;
                                cash.isChoosen = false;
                                inBank += cash.value;
                                addToBank(cash);
                            }

                            if (cursorRect.Intersects(rectCashRegister) && !cardPicked)
                            {
                                if (btnCashRegisterClicked())
                                {
                                    mode = StageMode.Finish;
                                }
                                else
                                {
                                    btnTrashClicked();
                                }
                            }

                            if (cursorRect.Intersects(rectTrash) && !cardPicked)
                            {
                                btnTrashClicked();
                            }
                        }

                        return GameType.ToyStore;
                    }
                case StageMode.Finish:
                    {
                        sounds.PlaySoundOnce(AvailableSounds.END_GAME);

                        if (cursorRect.Intersects(rectBtnExit))
                        {
                            sounds.PlaySound(AvailableSounds.CARTOON_HOP);
                            return GameType.School;
                        } 

                        if (cursorRect.Intersects(rectBtnReply))
                        {
                            sounds.PlaySound(AvailableSounds.CARTOON_HOP);
                            resetGame();
                            return GameType.ToyStore;
                        }

                        return GameType.ToyStore;
                    }
                default:
                    {
                        return GameType.ToyStore;
                    }
            }            
        }

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(txBackground, Vector2.Zero, Color.White);

            foreach (ToyCard card in cards)
            {
                if (card.isChoosen)
                {
                    spriteBatch.Draw(card.cardTextureHighlighted, card.cardPosition, null, Color.White, 0.0f, Vector2.Zero, card.scale, SpriteEffects.None, 0);
                } 

                spriteBatch.Draw(card.cardTexture, card.cardPosition, null, Color.White, 0.0f, Vector2.Zero, card.scale, SpriteEffects.None, 0);

                /*       DEBUG MODE       */
                //spriteBatch.Draw(txCardBackground, card.cardRectangle, Color.Black);
                //spriteBatch.DrawString(fontMiramonte, card.toyPrize.ToString(), new Vector2(card.cardRectangle.X + 50, card.cardRectangle.Y), Color.White);
                /* ---------------------- */
            }

            spriteBatch.Draw(txCardBackground, new Vector2(865, 20), Color.White);
            spriteBatch.Draw(txPayText, new Vector2(867,25), null, Color.White, 0.0f, Vector2.Zero, 0.15f, SpriteEffects.None, 0);

            if (payBtnProgress > 0) spriteBatch.Draw(getCardProgressTex(payBtnProgress), new Vector2(865, 20), Color.White);

            if (mode == StageMode.Paying)
            {
                spriteBatch.Draw(txPayBackground, new Vector2(0, 0), Color.White);
                spriteBatch.Draw(txKwotaText, new Vector2(10, 10), Color.White);
                spriteBatch.DrawString(fonts.Moire32, toPay.ToString() + " PLN", new Vector2(200, 10), Color.Wheat);
                //spriteBatch.Draw(txDragMoneyHere, rectDragMoneyHere, Color.Red);

                /* STRZALKI */
                spriteBatch.Draw(txArrow, new Vector2(210, 270), null, Color.White, 0.0f, Vector2.Zero, 0.1f, SpriteEffects.None, 0);   //Strzalka dolna lewa
                spriteBatch.Draw(txArrow, new Vector2(550, 270), null, Color.White, 0.0f, Vector2.Zero, 0.1f, SpriteEffects.FlipHorizontally, 0);   //Strzalka dolna prawa
                spriteBatch.Draw(txArrow, new Vector2(200, 70), null, Color.White, 0.0f, Vector2.Zero, 0.1f, SpriteEffects.FlipVertically, 0);   //Strzalka gorna lewa
                spriteBatch.Draw(txArrow, new Vector2(550, 70), null, Color.White, 0.0f, Vector2.Zero, 0.1f, SpriteEffects.FlipVertically | SpriteEffects.FlipHorizontally, 0);   //Strzalka gorna prawa
                /* END OF STRZALKI */

                spriteBatch.Draw(txPutMoneyHere, new Vector2(250, 200), null, Color.White, 0.0f, Vector2.Zero, 0.4f, SpriteEffects.None, 0);
                spriteBatch.Draw(txMoneyHolder, new Vector2(50, 400), Color.White);
                spriteBatch.Draw(txButtonBackground, new Vector2(14, 150), null, Color.White, 0.0f, Vector2.Zero, 0.35f, SpriteEffects.None, 0);
                spriteBatch.Draw(txCashRegister, new Vector2(20, 150), null, Color.White, 0.0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0);
                spriteBatch.Draw(txButtonBackground, new Vector2(14, 235), null, Color.White, 0.0f, Vector2.Zero, 0.35f, SpriteEffects.None, 0);
                spriteBatch.Draw(txTrash, new Vector2(20, 240), null, Color.White, 0.0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0);

                foreach (MoneyCard cash in money)
                {
                    spriteBatch.Draw(cash.cardTexture, cash.cardPosition, null, Color.White, 0.0f, Vector2.Zero, cash.scale, SpriteEffects.None, 0);

                    if (cash.cardProgress > 0) spriteBatch.Draw(getCardProgressTex(cash.cardProgress), cash.cardPosition, null, Color.White, 0.0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0);
                }

                foreach (MoneyCard cash in cashInBank)
                {
                    spriteBatch.Draw(cash.cardTexture, cash.cardPosition, null, Color.White, 0.0f, Vector2.Zero, cash.scale, SpriteEffects.None, 0);
                }
            }

            if (mode == StageMode.Finish)
            {
                spriteBatch.Draw(txFinish, new Vector2(0, 140), Color.White);
            }
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

        private void addToBank(MoneyCard m)
        {
            MoneyCard kasiora = new MoneyCard(m.cardTexture, m.cardPosition, m.value, m.moneyType);
            float banknoteScale = 0.2f;
            float coinScale = 0.4f;

            // SKALA
            if (kasiora.moneyType == MoneyType.Banknote)
            {
                kasiora.scale = banknoteScale;
            }
            else if (kasiora.moneyType == MoneyType.Coin)
            {
                kasiora.scale = coinScale;
            }

            // POZYCJA
            if (cashInBank.Count > 0)
            {
                Vector2 previousPosition = cashInBank.Last().cardPosition;

                if (kasiora.moneyType == MoneyType.Banknote)
                {
                    if (cashInBank.Last().moneyType == MoneyType.Banknote)
                        kasiora.cardPosition = new Vector2(previousPosition.X + (cashInBank.Last().cardTexture.Width * banknoteScale) + 10, previousPosition.Y);
                    else if (cashInBank.Last().moneyType == MoneyType.Coin)
                        kasiora.cardPosition = new Vector2(previousPosition.X + (cashInBank.Last().cardTexture.Width * banknoteScale) + 30, previousPosition.Y);
                }
                else if (kasiora.moneyType == MoneyType.Coin)
                {
                    if (cashInBank.Last().moneyType == MoneyType.Banknote)
                        kasiora.cardPosition = new Vector2(previousPosition.X + (cashInBank.Last().cardTexture.Width * coinScale) - 55, previousPosition.Y);
                    else if (cashInBank.Last().moneyType == MoneyType.Coin)
                        kasiora.cardPosition = new Vector2(previousPosition.X + (cashInBank.Last().cardTexture.Width * coinScale) + 10, previousPosition.Y);
                }

                if (kasiora.cardPosition.X > 850)
                {
                    kasiora.cardPosition.Y += 45;
                    kasiora.cardPosition.X = 90;
                }

                if (kasiora.cardPosition.Y > (410 + 2*45))
                {
                    //410 to pozycja startowa a sa 3 rzedy po 45
                    return;
                }
            }
            else
            { 
                kasiora.cardPosition = new Vector2(90, 410);
            }

            // DODANIE
            cashInBank.Add(kasiora);
        }

        private void btnTrashClicked()
        {
            sounds.PlaySound(AvailableSounds.ERROR);
            cashInBank.RemoveAll(c => c.value > 0);
            inBank = 0;
        }

        /// <summary>
        /// Akcja sprawdzenia czy kwota jest poprawna
        /// </summary>
        /// <returns>
        /// True jeśli wartość jest poprawna, false jeśli błąd
        /// </returns>
        private bool btnCashRegisterClicked()
        {
            if (inBank == toPay)
                return true;
            else
                return false;
        }

        private void resetGame()
        {
            cashInBank = new List<MoneyCard>();
            toPay = 0;
            inBank = 0;
            payBtnProgress = 0;
            mode = StageMode.Normal;

            foreach (ToyCard card in cards)
            {
                card.isChoosen = false;
            }

            sounds.resetSounds();
        }
    }
}
