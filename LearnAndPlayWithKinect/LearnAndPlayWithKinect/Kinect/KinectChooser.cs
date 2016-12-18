//------------------------------------------------------------------------------
// <copyright file="KinectChooser.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
//     Plus moje drobne poprawki
// </copyright>
//------------------------------------------------------------------------------

namespace LearnAndPlayWithKinect.Kinect
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Microsoft.Kinect;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using LearnAndPlayWithKinect.Other;

    /// <summary>
    /// This class will pick a kinect sensor if available.
    /// </summary>
    public class KinectChooser : DrawableGameComponent
    {
        /// <summary>
        /// The status to string mapping.
        /// </summary>
        private readonly Dictionary<KinectStatus, string> statusMap = new Dictionary<KinectStatus, string>();

        /// <summary>
        /// The requested color image format.
        /// </summary>
        private readonly ColorImageFormat colorImageFormat;

        /// <summary>
        /// The requested depth image format.
        /// </summary>
        private readonly DepthImageFormat depthImageFormat;

        /// <summary>
        /// The chooser background texture.
        /// </summary>
        private Texture2D chooserBackground;
        
        /// <summary>
        /// The SpriteBatch used for rendering.
        /// </summary>
        private SpriteBatch spriteBatch;
        
        /// <summary>
        /// The font for rendering the state text.
        /// </summary>
        private SpriteFont font;

        /// <summary>
        /// Initializes a new instance of the KinectChooser class.
        /// </summary>
        /// <param name="game">The related game object.</param>
        /// <param name="colorFormat">The desired color image format.</param>
        /// <param name="depthFormat">The desired depth image format.</param>
        public KinectChooser(Game game, ColorImageFormat colorFormat, DepthImageFormat depthFormat)
            : base(game)
        {
            this.colorImageFormat = colorFormat;
            this.depthImageFormat = depthFormat;

            KinectSensor.KinectSensors.StatusChanged += this.KinectSensors_StatusChanged;
            this.DiscoverSensor();

            //Zmiana konta elewacji
            try
            {
                Console.WriteLine("\n\n******************* KINECT ELEVATION ANGLE: " + Sensor.ElevationAngle.ToString() + "***********************\n");
                if (Sensor.ElevationAngle < 0 || Sensor.ElevationAngle > 5)
                {
                    Console.WriteLine("\n\n******************* CHANGING ELEVATION TO ZERO");
                    Sensor.ElevationAngle = 0;
                }
            }
            catch (Exception)
            {
                //throw new Exception("Nie odnaleziono sensora Kinect!");
            }

            this.statusMap.Add(KinectStatus.Connected, string.Empty);
            this.statusMap.Add(KinectStatus.DeviceNotGenuine, "nie jest oryginalny");
            this.statusMap.Add(KinectStatus.DeviceNotSupported, "nie jest obsługiwany");
            this.statusMap.Add(KinectStatus.Disconnected, "nie odnaleziono");
            this.statusMap.Add(KinectStatus.Error, "błąd");
            this.statusMap.Add(KinectStatus.Initializing, "inicjalizacja...");
            this.statusMap.Add(KinectStatus.InsufficientBandwidth, "niewystarczająca przepustowość");
            this.statusMap.Add(KinectStatus.NotPowered, "brak odpowiedniego zasilania");
            this.statusMap.Add(KinectStatus.NotReady, "niegotowy");
        }

        /// <summary>
        /// Gets the selected KinectSensor.
        /// </summary>
        public KinectSensor Sensor { get; private set; }

        /// <summary>
        /// Gets the last known status of the KinectSensor.
        /// </summary>
        public KinectStatus LastStatus { get; private set; }

        /// <summary>
        /// This method initializes necessary objects.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            this.spriteBatch = new SpriteBatch(Game.GraphicsDevice);
        }

        /// <summary>
        /// This method renders the current state of the KinectChooser.
        /// </summary>
        /// <param name="gameTime">The elapsed game time.</param>
        public override void Draw(GameTime gameTime)
        {
            // If the spritebatch is null, call initialize
            if (this.spriteBatch == null)
            {
                this.Initialize();
            }

            // If the background is not loaded, load it now
            if (this.chooserBackground == null)
            {
                this.LoadContent();
            }

            // If we don't have a sensor, or the sensor we have is not connected
            // then we will display the information text
            if (this.Sensor == null || this.LastStatus != KinectStatus.Connected)
            {
                this.spriteBatch.Begin();

                // Render the background
                this.spriteBatch.Draw(
                    this.chooserBackground,
                    new Vector2(Game.GraphicsDevice.Viewport.Width / 2, Game.GraphicsDevice.Viewport.Height / 2),
                    null,
                    Color.White,
                    0,
                    new Vector2(this.chooserBackground.Width / 2, this.chooserBackground.Height / 2),
                    1,
                    SpriteEffects.None,
                    0);

                // Determine the text
                string txt = "Status sensora - ";
                txt += this.statusMap[this.LastStatus];

                // Render the text
                this.spriteBatch.DrawString(this.font, txt, new Vector2((Settings.WINDOW_WIDTH / 2) - 300, 
                                                                        Settings.WINDOW_HEIGHT / 2), Color.White);
                this.spriteBatch.End();
            }

            base.Draw(gameTime);
        }

        /// <summary>
        /// This method loads the textures and fonts.
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();

            this.chooserBackground = Game.Content.Load<Texture2D>("kinect_err");
            this.font = Game.Content.Load<SpriteFont>("fonts\\Kootenay22");
        }

        /// <summary>
        /// This method ensures that the KinectSensor is stopped before exiting.
        /// </summary>
        protected override void UnloadContent()
        {
            base.UnloadContent();

            // Always stop the sensor when closing down
            if (this.Sensor != null)
            {
                this.Sensor.Stop();
            }
        }

        /// <summary>
        /// This method will use basic logic to try to grab a sensor.
        /// Once a sensor is found, it will start the sensor with the
        /// requested options.
        /// </summary>
        private void DiscoverSensor()
        {
            // Grab any available sensor
            this.Sensor = KinectSensor.KinectSensors.FirstOrDefault();

            if (this.Sensor != null)
            {
                this.LastStatus = this.Sensor.Status;

                // If this sensor is connected, then enable it
                if (this.LastStatus == KinectStatus.Connected)
                {
                    try
                    {
                        var parameters = new TransformSmoothParameters
                        {
                            Smoothing = 0.7f,//0.999f,
                            Correction = 0.3f,//0.1f,
                            Prediction = 0.4f,//0.1f,
                            JitterRadius = 1.0f,//0.05f,
                            MaxDeviationRadius = 0.5f//0.05f
                        };

                        this.Sensor.SkeletonStream.Enable(parameters);
                        this.Sensor.ColorStream.Enable(this.colorImageFormat);
                        this.Sensor.DepthStream.Enable(this.depthImageFormat);

                        try
                        {
                            this.Sensor.Start();
                        }
                        catch (IOException)
                        {
                            // sensor is in use by another application
                            // will treat as disconnected for display purposes
                            this.Sensor = null;
                        }
                    }
                    catch (InvalidOperationException)
                    {
                        // KinectSensor might enter an invalid state while
                        // enabling/disabling streams or stream features.
                        // E.g.: sensor might be abruptly unplugged.
                        this.Sensor = null;
                    }
                }
            }
            else
            {
                this.LastStatus = KinectStatus.Disconnected;
            }
        }

        /// <summary>
        /// This wires up the status changed event to monitor for 
        /// Kinect state changes.  It automatically stops the sensor
        /// if the device is no longer available.
        /// </summary>
        /// <param name="sender">The sending object.</param>
        /// <param name="e">The event args.</param>
        private void KinectSensors_StatusChanged(object sender, StatusChangedEventArgs e)
        {
            // If the status is not connected, try to stop it
            if (e.Status != KinectStatus.Connected)
            {
                e.Sensor.Stop();
            }

            this.LastStatus = e.Status;
            this.DiscoverSensor();
        }
    }
}
