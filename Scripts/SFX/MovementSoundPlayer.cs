using Godot;
using System;

namespace Game.SFX
{

    public class MovementSoundPlayer : AudioPlayer
    {
        // Audio Streams
        private Godot.Collections.Array<AudioStream> _movementAudioStreams;

        int _currentAudioStreamIndex = 0;

        public override void _Ready()
        {
            SetUpAudioArray();
        }

        private void SetUpAudioArray()
        {
            _movementAudioStreams = new Godot.Collections.Array<AudioStream>
            {
                GD.Load<AudioStream>("res://Audio/alien_move_1.wav"),
                GD.Load<AudioStream>("res://Audio/alien_move_2.wav"),
                GD.Load<AudioStream>("res://Audio/alien_move_3.wav"),
                GD.Load<AudioStream>("res://Audio/alien_move_4.wav")
            };
        }

        public override void PlaySound(string audio)
        {
            switch (audio)
            {
                case "move":
                    SoundToPlay = _movementAudioStreams[_currentAudioStreamIndex];
                    _currentAudioStreamIndex = (_currentAudioStreamIndex + 1) % _movementAudioStreams.Count;
                    break;
            }
            base.PlaySound(audio);
        }
    }
}