using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using System.Timers;

namespace LearnAndPlayWithKinect.Sounds
{
    class Sound
    {
        Timer timer;
        public SoundEffect sound;
        public bool wasPlayed;
        public AvailableSounds type;
        int interval;

        public Sound(SoundEffect sound, AvailableSounds type, int secondsInterval)
        {
            this.sound = sound;
            this.type = type;
            this.wasPlayed = false;
            this.interval = secondsInterval * 1000;

            timer = new Timer();
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            timer.Interval = interval;
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            wasPlayed = false;
            timer.Stop();
        }

        public void PlaySound()
        {
            sound.Play();
            wasPlayed = true;
            timer.Start();
        }

        public void PlaySoundOnce()
        {
            sound.Play();
            wasPlayed = true;
        }
    }
}
