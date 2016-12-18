using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Net;
using System.Threading;
using LearnAndPlayWithKinect.Other;

namespace LearnAndPlayWithKinect.Cloud
{
    class Services
    {
        /// <summary>
        /// Obiekt posiadający w sobie listę najlepszych wyników
        /// </summary>
        public RootObject bestScores;

        string _response;

        string response
        {
            get { return _response; }
            set
            {
                _response = value;
                if (_response != null)
                {
                    try
                    {
                        bestScores = Json.Deserialize(_response);
                    }
                    catch (Exception)
                    {
                        bestScores = null;
                    }
                    
                }
            }
        }

        /// <summary>
        /// Flaga sygnalizująca wysłanie requestu
        /// </summary>
        public bool isSent;

        public Services()
        {
            response = string.Empty;
            isSent = false;
        }

        public void initCloud()
        {
            if (Settings.PROJECT_STATE == Settings.Environment.DEVELOPMENT) return;

            Thread thread = new Thread(new ThreadStart(initRequest));
            thread.Start();
        }

        public void setScore(GameType game, int score)
        {
            Dictionary<string, string> data = new Dictionary<string, string>()
            {
                { "game", game.ToString()},
                { "user", System.Security.Principal.WindowsIdentity.GetCurrent().Name.Replace('\\', ' ')},
                { "score", score.ToString()}
            };

            Thread thread = new Thread(() => setScoreRequest(Request.DictionaryToJson(data)));
            thread.Name = "SET_SCORE THREAD -- " + game.ToString();
            thread.Start();
        }

        public void getScore(GameType gameType, int limit)
        {
            Dictionary<string, string> data = new Dictionary<string, string>()
            {
                { "game", gameType.ToString()},
                { "limit", limit.ToString()}
            };

            Thread thread = new Thread(() => getScoreRequest(Request.DictionaryToJson(data)));
            thread.Name = "GET_SCORE THREAD -- " + gameType.ToString();
            thread.Start();
        }

        void initRequest()
        {
            bool connectState = Request.CheckConnection();
            Console.WriteLine("**************** CLOUD: IS AVAILABLE - " + connectState.ToString() + " ****************");

            if (!connectState) return;

            Dictionary<string, string> data = new Dictionary<string, string>()
            {
                { "action", "LearnAndPlayWithKinect LAUNCHED !"},
                { "user", System.Security.Principal.WindowsIdentity.GetCurrent().Name.Replace('\\', ' ')}
            };

            try
            {
                string response = Request.PostRequest(Settings.SERVER_ADDRESS + "api/main/login", Request.DictionaryToJson(data));
            }
            catch (Exception)
            {

            }
            
        }

        void setScoreRequest(string json)
        {
            bool connectState = Request.CheckConnection();

            if (!connectState) return;

            try
            {
                string response = Request.PostRequest(Settings.SERVER_ADDRESS + "api/main/setScore", json);
            }
            catch (Exception)
            {

            }
        }

        void getScoreRequest(string json)
        {
            bool connectState = Request.CheckConnection();

            if (!connectState) return;

            string response = "";

            try
            {
                response = Request.PostRequest(Settings.SERVER_ADDRESS + "api/main/getBestScore", json);
            }
            catch (Exception)
            {
                response = "";
            }

            this.response = response;
        }
    }
}
