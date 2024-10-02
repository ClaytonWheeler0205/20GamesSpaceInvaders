using Godot;
using Game.Bus;
using System;
using Util.ExtensionMethods;

namespace Game
{

    public class LivesManager : Control, IGameManager
    {
        // Node References
        private Label _livesLabelReference;

        private const int STARTING_LIVES = 3;
        private int _currentLives = 0;

        [Signal]
        delegate void GameOver();

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            LivesEventBus.Instance.Connect("LoseLife", this, "OnLoseLife");
            SetNodeReferences();
            CheckNodeReferences();
        }

        private void SetNodeReferences()
        {
            _livesLabelReference = GetNode<Label>("LivesLabel");
        }

        private void CheckNodeReferences()
        {
            if (!_livesLabelReference.IsValid())
            {
                GD.PrintErr("ERROR: Lives label node is not valid!");
            }
        }

        public bool StartGame()
        {
            if (!_livesLabelReference.IsValid()) { return false; }
            _livesLabelReference.Text = $"Lives: {STARTING_LIVES}";
            _currentLives = STARTING_LIVES;
            return true;
        }

        public bool EndGame()
        {
            if (!_livesLabelReference.IsValid()) { return false; }
            _livesLabelReference.Text = "Lives: 0";
            return true;
        }

        public async void OnLoseLife()
        {
            _currentLives--;
            _livesLabelReference.Text = $"Lives: {_currentLives}";
            if (_currentLives == 0)
            {
                EmitSignal(nameof(GameOver));
            }
            else
            {
                await ToSignal(GetTree().CreateTimer(3.0f), "timeout");
                PlayerEventBus.Instance.EmitSignal("PlayerRespawn");
            }
        }
    }
}