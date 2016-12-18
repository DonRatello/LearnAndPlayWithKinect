using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace LearnAndPlayWithKinect.Sounds
{
    class SoundVault
    {
        List<Sound> sounds;
        List<AvailableSounds> availableSounds;
        int secondsInterval;

        public SoundVault(List<AvailableSounds> sounds, int SecondsInterval)
        {
            this.sounds = new List<Sound>();
            availableSounds = sounds;
            this.secondsInterval = SecondsInterval;
        }

        public void loadContent(ContentManager content)
        {
            if (availableSounds.Count != 0)
            {
                foreach (AvailableSounds s in availableSounds)
                {
                    Sound newSound = new Sound(content.Load<SoundEffect>("audio\\" + s.ToString().ToLower()), s, secondsInterval);
                    sounds.Add(newSound);
                }
            }
        }

        Sound getSound(AvailableSounds s)
        {
            foreach (Sound sound in sounds)
            {
                if (sound.type == s) return sound;
            }

            throw new Exception("SOUND IS NOT AVAILABLE");
        }

        public void PlaySound(AvailableSounds sound)
        {
            Sound s = getSound(sound);

            if (!s.wasPlayed)
            {
                s.PlaySound();
            }
        }

        public void PlaySoundOnce(AvailableSounds sound)
        {
            Sound s = getSound(sound);

            if (!s.wasPlayed)
            {
                s.PlaySoundOnce();
            }
        }

        /// <summary>
        /// Funkcja ustawia flagi wasPlayed na FALSE we wszystkich dźwiękach
        /// </summary>
        public void resetSounds()
        {
            foreach (Sound s in sounds)
            {
                s.wasPlayed = false;
            }
        }
    }
}
