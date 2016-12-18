using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LearnAndPlayWithKinect.Games
{
    class Time
    {
        public int minutes;
        public int seconds;
        public bool isWorking;
        public bool isEnabled;
        
        DateTime endDate;
        int minutesForGame;
        int secondsForGame;

        public Time(int gameMinutes, int gameSeconds)
        {
            endDate = new DateTime();
            minutes = 0;
            seconds = 0;
            isWorking = false;
            isEnabled = true;
            minutesForGame = gameMinutes;
            secondsForGame = gameSeconds;
        }

        public void cleanTimer()
        {
            minutes = 0;
            seconds = 0;
            isWorking = false;
            isEnabled = false;
        }

        public void startTimer()
        {
            endDate = DateTime.Now.AddMinutes(minutesForGame);
            endDate = endDate.AddSeconds(secondsForGame);
            isWorking = true;
        }

        public void countTimeLeft()
        {
            DateTime now = DateTime.Now;

            minutes = (endDate - now).Minutes;
            seconds = (endDate - now).Seconds;
        }

        public string getMinutesFormatted()
        {
            return this.minutes.ToString();
        }

        public string getSecondsFormatted()
        {
            if (seconds < 10)
                return 0 + seconds.ToString();
            else
                return seconds.ToString();
        }
    }
}
