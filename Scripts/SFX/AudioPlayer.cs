using Godot;
using System;

namespace Game.SFX
{

    public abstract class AudioPlayer : AudioStreamPlayer
    {
        private AudioStream _soundToPlay = null;
        protected AudioStream SoundToPlay
        {
            get { return _soundToPlay; }
            set
            {
                if (value != null)
                {
                    _soundToPlay = value;
                }
            }
        }

        /// <summary>
        /// Plays an audio stream based on the string parameter given. The concrete implementation will determine what sound is to be played.
        /// </summary>
        /// <param name="audio">The string name of the audio sound to play</param>
        public virtual void PlaySound(string audio)
        {
            if (_soundToPlay != null) // sound to be played will be decided in the concrete implementation
            {
                Stream = _soundToPlay;
                Play();
            }
        }
    }
}