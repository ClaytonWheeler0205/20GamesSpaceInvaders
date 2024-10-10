using Godot;
using System;

namespace Game.SFX
{

    public class MissileSoundPlayer : AudioPlayer
    {
        // Audio Streams
        private AudioStream _missileFireSFX = GD.Load<AudioStream>("res://Audio/missile_fire.wav");

        public override void PlaySound(string audio)
        {
            switch (audio)
            {
                case "missile_fire":
                    SoundToPlay = _missileFireSFX;
                    PitchScale = (float)GD.RandRange(0.75, 1.25);
                    break;
                default:
                    PitchScale = 1.0f;
                    break;
            }
            base.PlaySound(audio);
        }
    }
}