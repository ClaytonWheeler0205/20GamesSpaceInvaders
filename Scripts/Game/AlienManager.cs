using Game.Alien;
using Godot;
using System;

namespace Game
{

    public class AlienManager : Node, IGameManager
    {
        private InvasionBase _alienInvaders;
        private Timer _movementTimer;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            SetNodeReferences();
            StartGame();
        }

        private void SetNodeReferences()
        {
            _alienInvaders = GetNode<InvasionBase>("AlienInvaders");
            _movementTimer = GetNode<Timer>("MovementTimer");
        }

        public bool StartGame()
        {
            _movementTimer.Start(_alienInvaders.AliensCount / 60.0f);
            return true;
        }

        public bool EndGame()
        {
            return true;
        }

        public void OnMovementTimerTimeout()
        {
            _alienInvaders.Move();
            _movementTimer.Start(_alienInvaders.AliensCount / 60.0f);
        }
    }
}