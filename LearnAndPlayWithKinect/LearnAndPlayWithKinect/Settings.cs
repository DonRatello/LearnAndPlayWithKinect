using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using LearnAndPlayWithKinect.Other;

namespace LearnAndPlayWithKinect
{
    class Settings
    {
        public enum Environment { DEVELOPMENT, PRODUCTION };

        /// <summary>
        /// Stan produkcji
        /// </summary>
        public const Environment PROJECT_STATE = Environment.PRODUCTION;

        /// <summary>
        /// Adres serwera
        /// </summary>
        public const string SERVER_ADDRESS = "http://localhost";

        /// <summary>
        /// Adres webservice'u
        /// </summary>
        public const string SERVER_IP = "127.0.0.1";

        /// <summary>
        /// Szerokość okna
        /// </summary>
        public const int WINDOW_WIDTH = 1000;

        /// <summary>
        /// Wysokość okna
        /// </summary>
        public const int WINDOW_HEIGHT = 700;

        /// <summary>
        /// Ekran startowy
        /// </summary>
        public const GameType STARTING_SCREEN = GameType.Kindergarten;

        /// <summary>
        /// Próg pewności wypowiedzi poniżej którego słowa będą traktowane jako "nieusłyszane"
        /// </summary>
        public static double CONFIDENCE_THRESHOLD
        {
            get 
            {
                try
                {
                    Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                    return Double.Parse(config.AppSettings.Settings["CONFIDENCE_THRESHOLD"].Value);
                }
                catch (Exception)
                {
                    return 0.8f;
                }
            }
            set
            { }
        }

        /// <summary>
        /// Czy odtwarzac dzwiek przy zbieraniu jablek
        /// </summary>
        public static bool PLAY_PICK_APPLE_SOUND
        {
            get
            {
                try
                {
                    Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                    return Boolean.Parse(config.AppSettings.Settings["PLAY_PICKING_APPLE_SOUND"].Value);
                }
                catch (Exception)
                {
                    return false;
                }
            }
            set
            { }
        }
    }
}
