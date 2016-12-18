using System;
using System.Linq;
using System.Windows;
using System.Threading;
using System.Collections.Generic;
using Microsoft.Kinect;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using LearnAndPlayWithKinect.Other;
using LearnAndPlayWithKinect.Kinect;
using LearnAndPlayWithKinect.Interfaces;

namespace LearnAndPlayWithKinect
{
    public class LearnAndPlayGame : Microsoft.Xna.Framework.Game
    {
        /// <summary>
        /// GraphicsDeviceManager dostarczony przez XNA
        /// </summary>
        private readonly GraphicsDeviceManager graphics;

        /// <summary>
        /// Klasa Kinecta. Odpowiedzialna za wybranie sensora i jego wykorzystanie.
        /// </summary>
        private readonly KinectChooser chooser;

        /// <summary>
        /// Rozpoznawanie mowy
        /// </summary>
        private KinectSpeech speech;

        /// <summary>
        /// Strumien kolorowy.
        /// </summary>
        private readonly ColorStreamRenderer colorStream;

        /// <summary>
        /// Strumien glebokosci.
        /// </summary>
        private readonly DepthStreamRenderer depthStream;

        /// <summary>
        /// ViewPort obrazu w ktorym pokazywane sa strumienie.
        /// </summary>
        private readonly Rectangle viewPortRectangle;

        /// <summary>
        /// SpriteBatch uzywany do renderowania calej reszty
        /// </summary>
        private SpriteBatch spriteBatch;

        /// <summary>
        /// Aktywny ekran
        /// </summary>
        GameType activeScreen;

        /// <summary>
        /// Œledzony szkielet
        /// </summary>
        TrackedSkeleton trackedSkeleton;

        /// <summary>
        /// Klasa kursora
        /// </summary>
        Pointer pointer;

        /// <summary>
        /// Album z czcionkami
        /// </summary>
        Fonts fonts;

        /// <summary>
        /// Chmura
        /// </summary>
        Cloud.Services cloud;

        //-------------------------------------------
        //Ekrany
        InfoBox infoBox;
        Loading.Screen loadingScreen;
        MainMenu.Screen mainMenuScreen;
        Intro.Screen introScreen;
        Kindergarten.Screen kindergartenScreen;
        School.Screen schoolScreen;

        Games.Memory.Screen memoryScreen;
        Games.ToyStore.Screen toystoreScreen;
        Games.EngColors.Screen engColorsScreen;
        Games.Gymnastics.Screen gymnasticsScreen;
        Games.PickingApples.Screen pickingApplesScreen;

        //-------------------------------------------

        public LearnAndPlayGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            //----------------------------------------//
            //           PIERWSZY EKRAN               //
            //----------------------------------------//
            if (Settings.PROJECT_STATE == Settings.Environment.DEVELOPMENT)
                activeScreen = Settings.STARTING_SCREEN;
            else
                activeScreen = GameType.LoadingScreen;
            //----------------------------------------//

            //Kursor widoczny na ekranie
            pointer = new Pointer();

            //Czcionki dostepne w aplikacji
            fonts = new Fonts();

            //InfoBox wyswietlany w trakcie gry
            infoBox = new InfoBox(fonts);
            infoBox.Disable();
            infoBox.Invisible();

            //Sledzony szkielet
            trackedSkeleton = new TrackedSkeleton();

            //Chmura
            cloud = new Cloud.Services();

            //----------------------------------------------------------------------------------------------------------------------
            //                                                  USTAWIENIA OKNA
            //----------------------------------------------------------------------------------------------------------------------
            //Ustawienie nazwy okna
            this.Window.Title = "Ucz siê i baw z Kinectem!";

            this.graphics.PreferredBackBufferWidth = Settings.WINDOW_WIDTH;
            this.graphics.PreferredBackBufferHeight = Settings.WINDOW_HEIGHT;
            this.graphics.PreparingDeviceSettings += this.GraphicsDevicePreparingDeviceSettings;
            this.graphics.SynchronizeWithVerticalRetrace = true;

            //----------------------------------------------------------------------------------------------------------------------
            //                                       INICJALIZACJA KINECTA I USTAWIENIA
            //----------------------------------------------------------------------------------------------------------------------
            this.viewPortRectangle = new Rectangle((int)infoBox.getPosition().X, (int)infoBox.getPosition().Y, 130, 130);

            // Sensor bedzie uzywal rozdzielczosci 640x480 dla obu strumieni
            this.chooser = new KinectChooser(this, ColorImageFormat.RgbResolution640x480Fps30, DepthImageFormat.Resolution640x480Fps30);
            this.Services.AddService(typeof(KinectChooser), this.chooser);

            // ColorStream - pozycja i rozmiar
            this.colorStream = new ColorStreamRenderer(this);
            this.colorStream.Size = new Vector2(this.viewPortRectangle.Width, this.viewPortRectangle.Height);
            this.colorStream.Position = new Vector2(infoBox.getPosition().X + 160, infoBox.getPosition().Y + 10);

            // DepthStream - pozycja i rozmiar
            this.depthStream = new DepthStreamRenderer(this);
            this.depthStream.Size = new Vector2(this.viewPortRectangle.Width, this.viewPortRectangle.Height);
            this.depthStream.Position = new Vector2(infoBox.getPosition().X + 150, infoBox.getPosition().Y);

            this.Components.Add(this.chooser);

            //Licznik klatek
            if (Settings.PROJECT_STATE == Settings.Environment.DEVELOPMENT) this.Components.Add(new FrameRateCounter(this));

            speech = new KinectSpeech(chooser.Sensor);
            //----------------------------------------------------------------------------------------------------------------------
        }

        protected override void Initialize()
        {
            //Inicjalizacja zmiennych ekranow
            loadingScreen = new Loading.Screen(fonts);
            mainMenuScreen = new MainMenu.Screen(fonts);
            introScreen = new Intro.Screen(fonts);
            kindergartenScreen = new Kindergarten.Screen();
            schoolScreen = new School.Screen();
            memoryScreen = new Games.Memory.Screen(fonts);
            toystoreScreen = new Games.ToyStore.Screen(fonts);
            engColorsScreen = new Games.EngColors.Screen(fonts);
            gymnasticsScreen = new Games.Gymnastics.Screen(fonts);
            pickingApplesScreen = new Games.PickingApples.Screen(fonts, colorStream);

            this.Components.Add(this.depthStream);
            this.Components.Add(this.colorStream);

            cloud.initCloud();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Stworzenie nowego spriteBatcha i dodanie go do serwisow
            this.spriteBatch = new SpriteBatch(this.GraphicsDevice);
            this.Services.AddService(typeof(SpriteBatch), this.spriteBatch);

            fonts.loadContent(this.Content);
            loadingScreen.loadContent(this.Content);
            introScreen.loadContent(this.Content);

            // LADOWANIE ELEMENTOW
            //-------------------------------------------------------------------------------------------------
            if (Settings.PROJECT_STATE == Settings.Environment.DEVELOPMENT)
            {
                mainMenuScreen.loadContent(this.Content);
                kindergartenScreen.loadContent(this.Content);
                schoolScreen.loadContent(this.Content);
                infoBox.loadContent(this.Content);
                pointer.loadContent(this.Content);
                memoryScreen.loadContent(this.Content);
                toystoreScreen.loadContent(this.Content);
                engColorsScreen.loadContent(this.Content);
                gymnasticsScreen.loadContent(this.Content);
                pickingApplesScreen.loadContent(this.Content);
            }
            //-------------------------------------------------------------------------------------------------
        }

        protected override void UnloadContent()
        {
            Content.Unload();
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState newState = Keyboard.GetState();

            // Wyjscie z gry
            if (newState.IsKeyDown(Keys.Escape))
                this.Exit();

            // Podanie komendy play
            if (newState.IsKeyDown(Keys.P))
                speech.setLastCommand(WordCommands.START);

            if (newState.IsKeyDown(Keys.A))
            {
                if (speech.getConfidenceThreshold() + 0.01f < 1) speech.setConfidenceThreshold(Math.Round(speech.getConfidenceThreshold() + 0.01f, 2));
                else speech.setConfidenceThreshold(1.0f);
            }

            if (newState.IsKeyDown(Keys.Z))
            {
                if (speech.getConfidenceThreshold() - 0.01f > 0) speech.setConfidenceThreshold(Math.Round(speech.getConfidenceThreshold() - 0.01f, 2));
                else speech.setConfidenceThreshold(0.0f);
            }

            //Infobox jest update'owany niezaleznie od ekranu
            trackedSkeleton.updateSkeleton(depthStream.skeletonStream.getLastSkeleton());
            activeScreen = infoBox.update(trackedSkeleton.getJoint(JointType.WristRight), pointer.rectanglePointer, activeScreen, speech.getConfidenceThreshold());

            //Stan myszy
            MouseState mouse = Mouse.GetState();
            pointer.update(mouse.X, mouse.Y);

            //Ruch reka
            pointer.updateByKinect(trackedSkeleton);

            depthStream.Visible = false;
            depthStream.Enabled = true;
            colorStream.Visible = true;
            colorStream.Enabled = true;

            //Update screenu ze wzgledu na aktywny ekran
            switch (activeScreen)
            {
                case GameType.LoadingScreen:
                    {
                        infoBox.Invisible();
                        colorStream.Visible = false;
                        colorStream.Enabled = false;
                        depthStream.Visible = false;
                        depthStream.Enabled = false;
                        activeScreen = loadingScreen.update();
                        pointer.Disable();
                        LoadContentInTime(loadingScreen.getTime());
                        break;
                    }
                case GameType.Intro:
                    {
                        infoBox.Invisible();
                        colorStream.Visible = false;
                        colorStream.Enabled = false;
                        depthStream.Visible = false;
                        depthStream.Enabled = false;
                        activeScreen = introScreen.update();
                        pointer.Disable();
                        break;
                    }
                case GameType.MainMenu:
                    {
                        infoBox.Visible();
                        activeScreen = mainMenuScreen.update(pointer.rectanglePointer);
                        pointer.Enable();
                        break;
                    }
                case GameType.Kindergarten:
                    {
                        infoBox.Visible();
                        activeScreen = kindergartenScreen.update(pointer.rectanglePointer);
                        pointer.Enable();
                        break;
                    }
                case GameType.School:
                    {
                        infoBox.Visible();
                        activeScreen = schoolScreen.update(pointer.rectanglePointer);
                        pointer.Enable();
                        break;
                    }
                case GameType.Memory:
                    {
                        infoBox.Visible();
                        activeScreen = memoryScreen.update(pointer.rectanglePointer);
                        pointer.Enable();
                        break;
                    }
                case GameType.ToyStore:
                    {
                        infoBox.Visible();
                        activeScreen = toystoreScreen.update(pointer.rectanglePointer);
                        pointer.Enable();
                        break;
                    }
                case GameType.EngColors:
                    {
                        infoBox.Visible();
                        activeScreen = engColorsScreen.update(pointer.rectanglePointer);
                        pointer.Enable();
                        break;
                    }
                case GameType.Gymnastics:
                    {
                        infoBox.Invisible();
                        speech.EnableSpeechRecognition();
                        GameType newMode = gymnasticsScreen.update(pointer.rectanglePointer, trackedSkeleton.getSkeleton(), speech.getLastCommand());

                        if (newMode != activeScreen)
                            speech.DisableSpeechRecognition();

                        speech.cleanLastCommand();
                        pointer.Disable();
                        activeScreen = newMode;
                        break;
                    }
                case GameType.PickingApples:
                    {
                        infoBox.Invisible();
                        colorStream.Visible = false;
                        GameType newMode = pickingApplesScreen.update(trackedSkeleton.getJoint(JointType.HandLeft), trackedSkeleton.getJoint(JointType.HandRight), speech.getLastCommand());

                        if (newMode == activeScreen)
                        {
                            if (pickingApplesScreen.firstEntrance)
                            {
                                //Tryb uruchomiony pierwszy raz, trzeba zmienic viewport
                                pickingApplesScreen.firstEntrance = false;
                                speech.EnableSpeechRecognition();
                                this.setKinectViewport(KinectViewportModes.PICKING_APPLES);
                            }
                        }
                        else
                        {
                            //Zmiana trybu
                            pickingApplesScreen.firstEntrance = true;
                            this.setKinectViewport(KinectViewportModes.NORMAL);
                            speech.DisableSpeechRecognition();
                        }

                        speech.cleanLastCommand();
                        pointer.Disable();
                        activeScreen = newMode;
                        break;
                    }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            //Malowanie grafik ze wzgledu na aktywny ekran
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

            switch (activeScreen)
            {
                case GameType.LoadingScreen:
                    {
                        loadingScreen.draw(spriteBatch);
                        break;
                    }
                case GameType.Intro:
                    {
                        introScreen.draw(spriteBatch);
                        break;
                    }
                case GameType.MainMenu:
                    {
                        mainMenuScreen.draw(spriteBatch);
                        break;
                    }
                case GameType.Kindergarten:
                    {
                        kindergartenScreen.draw(spriteBatch);
                        break;
                    }
                case GameType.School:
                    {
                        schoolScreen.draw(spriteBatch);
                        break;
                    }
                case GameType.Memory:
                    {
                        memoryScreen.draw(spriteBatch);
                        break;
                    }
                case GameType.ToyStore:
                    {
                        toystoreScreen.draw(spriteBatch);
                        break;
                    }
                case GameType.EngColors:
                    {
                        engColorsScreen.draw(spriteBatch);
                        break;
                    }
                case GameType.Gymnastics:
                    {
                        gymnasticsScreen.draw(spriteBatch);
                        break;
                    }
                case GameType.PickingApples:
                    {
                        pickingApplesScreen.draw(spriteBatch);
                        break;
                    }
            }

            //Infobox jest malowany niezaleznie od ekranu
            infoBox.draw(spriteBatch);

            //Kursor
            pointer.draw(spriteBatch);

            spriteBatch.End();
            base.Draw(gameTime);
        }

        public void setActiveScreen(GameType newType)
        {
            this.activeScreen = newType;
        }

        /// <summary>
        /// Metoda ta zapewnia, ze mozna renderowac do back-buffera bez straty danych juz posiadanych.
        /// Wykorzystywana przez SkeletonStreamRenderer
        /// </summary>
        private void GraphicsDevicePreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            e.GraphicsDeviceInformation.PresentationParameters.RenderTargetUsage = RenderTargetUsage.PreserveContents;
        }

        private void LoadContentInTime(int time)
        {
            switch (time)
            {
                //Ladowanie elementow programu
                //W celu zminimalizowania mozliwosci zawieszenia sie, rozkladam to w czasie
                case 100:
                    {
                        //Ladowanie ekranu startowego i menu
                        //introScreen.loadContent(this.Content);
                        mainMenuScreen.loadContent(this.Content);
                        break;
                    }
                case 200:
                    {
                        //Ladowanie ekranu przedszkola i szkoly
                        kindergartenScreen.loadContent(this.Content);
                        schoolScreen.loadContent(this.Content);
                        break;
                    }
                case 300:
                    {
                        //InfoBox
                        infoBox.loadContent(this.Content);
                        break;
                    }
                case 400:
                    {
                        //Wskaznik
                        pointer.loadContent(this.Content);
                        break;
                    }
                case 500:
                    {
                        memoryScreen.loadContent(this.Content);
                        break;
                    }
                case 550:
                    {
                        pickingApplesScreen.loadContent(this.Content);
                        break;
                    }
                case 600:
                    {
                        engColorsScreen.loadContent(this.Content);
                        break;
                    }
                case 650:
                    {
                        gymnasticsScreen.loadContent(this.Content);
                        break;
                    }
                case 700:
                    {
                        break;
                    }
                case 900:
                    {
                        toystoreScreen.loadContent(this.Content);
                        break;
                    }
            }
        }

        /// <summary>
        /// Niektóre gry wymagaj¹ innych rozmiarów obrazu z kinecta. Ta funkcja pozwala na zmianê trybów wyœwietlania
        /// </summary>
        /// <param name="viewport"></param>
        private void setKinectViewport(KinectViewportModes viewport)
        {
            // DepthStream - pozycja i rozmiar
            this.depthStream.Size = new Vector2(this.viewPortRectangle.Width, this.viewPortRectangle.Height);
            this.depthStream.Position = new Vector2(infoBox.getPosition().X + 150, infoBox.getPosition().Y);

            switch (viewport)
            {
                case KinectViewportModes.NORMAL:
                    {
                        this.colorStream.Visible = true;
                        this.colorStream.Size = new Vector2(this.viewPortRectangle.Width, this.viewPortRectangle.Height);
                        this.colorStream.Position = new Vector2(infoBox.getPosition().X + 160, infoBox.getPosition().Y + 10);

                        break;
                    }
                case KinectViewportModes.PICKING_APPLES:
                    {
                        this.colorStream.Size = new Vector2(800, 600);
                        this.colorStream.Position = new Vector2(100, 0);

                        break;
                    }
            }
        }
    }
}

