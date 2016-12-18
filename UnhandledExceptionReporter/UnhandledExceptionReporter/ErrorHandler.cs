using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace UnhandledExceptionReporter
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class ErrorHandler : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D txBackground;
        SpriteFont Calibri;

        public string errorMessage = "Nie wykryto b³êdu";

        public ErrorHandler()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            //Set window name
            this.Window.Title = "Ucz siê i baw z Kinectem!";

            this.graphics.PreferredBackBufferWidth = 1000;
            this.graphics.PreferredBackBufferHeight = 700;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            this.spriteBatch = new SpriteBatch(this.GraphicsDevice);

            txBackground = Content.Load<Texture2D>("kinect_err");
            Calibri = Content.Load<SpriteFont>("Calibri");
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState newState = Keyboard.GetState();

            // Allow the game to exit
            if (newState.IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            spriteBatch.Draw(txBackground, Vector2.Zero, Color.White);
            spriteBatch.DrawString(Calibri, errorMessage, new Vector2(100, 400), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
