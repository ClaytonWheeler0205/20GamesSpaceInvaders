using Game.Alien;
using Game.SFX;
using Godot;
using System;

namespace Game
{

    public class AlienManager : Node, IGameManager
    {
        private InvasionBase _alienInvaders;
        private Timer _movementTimer;
        private AudioPlayer _movementSoundPlayer;

        [Signal]
        public delegate void AliensCleared();

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            SetNodeReferences();
            _alienInvaders.Visible = false;
        }

        private void SetNodeReferences()
        {
            _alienInvaders = GetNode<InvasionBase>("AlienInvaders");
            _movementTimer = GetNode<Timer>("MovementTimer");
            _movementSoundPlayer = GetNode<AudioPlayer>("MovementSoundPlayer");
        }

        public bool StartGame()
        {
            _alienInvaders.Visible = true;
            _movementTimer.Start(_alienInvaders.AliensCount / 60.0f);
            return true;
        }

        public bool EndGame()
        {
            _alienInvaders.Visible = false;
            _alienInvaders.ResetInvasion();
            return true;
        }

        public void OnMovementTimerTimeout()
        {
            if (_alienInvaders.AliensCount > 0)
            {
                _alienInvaders.Move();
                _movementSoundPlayer.PlaySound("move");
                _movementTimer.Start(_alienInvaders.AliensCount / 60.0f);
            }
            else
            {
                EmitSignal(nameof(AliensCleared));
            }
        }
    }
}