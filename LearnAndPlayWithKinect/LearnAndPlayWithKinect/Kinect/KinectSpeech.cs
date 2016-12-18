using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using Microsoft.Kinect;
using Microsoft.Speech.AudioFormat;
using Microsoft.Speech.Recognition;
using LearnAndPlayWithKinect.Other;

namespace LearnAndPlayWithKinect.Kinect
{
    class KinectSpeech
    {
        WordCommands lastSaidCommand;

        /// <summary>
        /// Obiekt KinectSensor potrzebny do rozpoznawania mowy
        /// </summary>
        KinectSensor kinect;

        /// <summary>
        /// Silnik rozpoznawania mowy wykorzystujacy dane audio z Kinecta
        /// </summary>
        SpeechRecognitionEngine speechEngine;

        /// <summary>
        /// Lokalny poziom rozpoznawania mowy
        /// </summary>
        double ConfidenceThreshold;

        Timer timer;
        bool engineReady;

        public KinectSpeech(KinectSensor kinect)
        {
            this.kinect = kinect;
            this.ConfidenceThreshold = Settings.CONFIDENCE_THRESHOLD;

            engineReady = true;
            timer = new Timer();
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            timer.Interval = 2000; //ms

            if (kinect != null) initializeSpeechRecognizer();
        }

        /// <summary>
        /// Inicjalizacja silnika rozpoznawania mowy
        /// </summary>
        void initializeSpeechRecognizer()
        {
            RecognizerInfo ri = GetKinectRecognizer();

            if (null != ri)
            {
                this.speechEngine = new SpeechRecognitionEngine(ri.Id);

                //----------------------------------------------------------
                // DEFINICJA SLOWNIKA
                var commands = new Choices();
                commands.Add(new SemanticResultValue("start", "START"));
                commands.Add(new SemanticResultValue("play", "START"));
                commands.Add(new SemanticResultValue("stop", "STOP"));
                commands.Add(new SemanticResultValue("back", "STOP"));
                //----------------------------------------------------------

                var gb = new GrammarBuilder { Culture = ri.Culture };
                gb.Append(commands);

                var g = new Grammar(gb);
                speechEngine.LoadGrammar(g);

                //------------------------------------------------------------
                // REJESTRACJA EVENTÓW
                EnableSpeechRecognition();
                //------------------------------------------------------------

                speechEngine.SetInputToAudioStream(
                    kinect.AudioSource.Start(), new SpeechAudioFormatInfo(EncodingFormat.Pcm, 16000, 16, 1, 32000, 2, null));
                speechEngine.RecognizeAsync(RecognizeMode.Multiple);
            }
            else
            {
                throw new Exception("No speech recognizer");
            }
        }

        private static RecognizerInfo GetKinectRecognizer()
        {
            foreach (RecognizerInfo recognizer in SpeechRecognitionEngine.InstalledRecognizers())
            {
                string value;
                recognizer.AdditionalInfo.TryGetValue("Kinect", out value);
                if ("True".Equals(value, StringComparison.OrdinalIgnoreCase) && "en-US".Equals(recognizer.Culture.Name, StringComparison.OrdinalIgnoreCase))
                {
                    return recognizer;
                }
            }

            return null;
        }

        /// <summary>
        /// Obsługa wyłapanych słów
        /// </summary>
        private void SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Confidence >= ConfidenceThreshold && engineReady)
            {
                switch (e.Result.Semantics.Value.ToString())
                {
                    case "START":
                        {
                            lastSaidCommand = WordCommands.START;
                            Console.WriteLine("############### Confidence - START : " + e.Result.Confidence.ToString());
                            break;
                        }

                    case "STOP":
                        {
                            lastSaidCommand = WordCommands.STOP;
                            Console.WriteLine("############### Confidence - STOP : " + e.Result.Confidence.ToString());
                            break;
                        }
                }

                engineReady = false;
                Console.WriteLine("############### SPEECH ENGINE BUSY");
                timer.Start();
            }
        }

        /// <summary>
        /// Obsługa odrzuconych słów
        /// </summary>
        private void SpeechRejected(object sender, SpeechRecognitionRejectedEventArgs e)
        {

        }

        /// <summary>
        /// Rejestracja eventow
        /// </summary>
        public void EnableSpeechRecognition()
        {
            speechEngine.SpeechRecognized += SpeechRecognized;
            speechEngine.SpeechRecognitionRejected += SpeechRejected;
        }

        /// <summary>
        /// Usuniecie eventow
        /// </summary>
        public void DisableSpeechRecognition()
        {
            lastSaidCommand = WordCommands.NO_COMMAND;
            speechEngine.SpeechRecognized -= SpeechRecognized;
            speechEngine.SpeechRecognitionRejected -= SpeechRejected;
        }

        /// <summary>
        /// Pobranie ostatnio wypowiedzianej komendy
        /// </summary>
        /// <returns></returns>
        public WordCommands getLastCommand()
        {
            return lastSaidCommand;
        }

        /// <summary>
        /// Wyczyszczenie pola LastCommand
        /// </summary>
        public void cleanLastCommand()
        {
            lastSaidCommand = WordCommands.NO_COMMAND;
        }

        /// <summary>
        /// "Awaryjne" ustawienie komendy
        /// </summary>
        /// <param name="command"></param>
        public void setLastCommand(WordCommands command)
        {
            this.lastSaidCommand = command;
        }

        /// <summary>
        /// Ustawienie poziomu słyszalności
        /// </summary>
        /// <param name="threshold"></param>
        public void setConfidenceThreshold(double threshold)
        {
            this.ConfidenceThreshold = threshold;
        }

        /// <summary>
        /// Pobranie poziomu słyszalności
        /// </summary>
        /// <returns></returns>
        public double getConfidenceThreshold()
        {
            return this.ConfidenceThreshold;
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            engineReady = true;
            timer.Stop();
            Console.WriteLine("############### SPEECH ENGINE READY");
        }
    }
}
