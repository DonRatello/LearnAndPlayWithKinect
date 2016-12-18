using System;

namespace UnhandledExceptionReporter
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (ErrorHandler game = new ErrorHandler())
            {
                game.Run();
            }
        }
    }
#endif
}

