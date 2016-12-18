using System;
using LearnAndPlayWithKinect.Other;

namespace LearnAndPlayWithKinect
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// Glowny punkt startowy aplikacji
        /// </summary>
        static void Main(string[] args)
        {
            if (Settings.PROJECT_STATE == Settings.Environment.DEVELOPMENT)
            {
                using (LearnAndPlayGame game = new LearnAndPlayGame())
                {
                    game.Run();
                }
            }
            else if (Settings.PROJECT_STATE == Settings.Environment.PRODUCTION)
            {
                //W przypadku wyrzucenia b³êdu przez LearnAndPlayGame, UnhandledExceptionReporter go ³apie i wyœwietla na ekranie
                try
                {
                    using (LearnAndPlayGame game = new LearnAndPlayGame())
                    {
                        game.Run();
                    }
                }
                catch (Exception e)
                {
                    using (UnhandledExceptionReporter.ErrorHandler handler = new UnhandledExceptionReporter.ErrorHandler())
                    {
                        handler.errorMessage = e.Message;
                        handler.Run();
                    }
                }
            }
            
        }
    }
#endif
}

