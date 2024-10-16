using Game.Bus;
using Godot;
using System;
using Util.ExtensionMethods;
using Util;
using System.Drawing.Printing;

namespace Game
{

    public class SpaceInvadersMain : Node
    {

        private IGameManager _alienManager;
        private IGameManager _playerManager;
        private IGameManager _livesManager;
        private IGameManager _mothershipManager;

        private Control _gameOverLabel;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            SetNodeReferences();
            CheckNodeReferences();
            PlayerEventBus.Instance.Connect("AlienHit", this, "OnGameOver");
            StartGame();
        }

        private async void StartGame()
        {
            _alienManager.StartGame();
            _mothershipManager.StartGame();
            _livesManager.StartGame();
            await ToSignal(GetTree().CreateTimer(0.5f), "timeout");
            _playerManager.StartGame();
        }

        private void SetNodeReferences()
        {
            _alienManager = GetNode<IGameManager>("AlienManager");
            _playerManager = GetNode<IGameManager>("PlayerManager");
            _livesManager = GetNode<IGameManager>("GUI/LivesManager");
            _mothershipManager = GetNode<IGameManager>("MothershipManager");
            _gameOverLabel = GetNode<Control>("GUI/GameOverLabel");
        }

        private void CheckNodeReferences()
        {
            if (!(_alienManager is Node alienNode))
            {
                GD.PrintErr("ERROR: Alien manager is not a node!");
                GetTree().Quit();
            }
            else if (!alienNode.IsValid())
            {
                GD.PrintErr("ERROR: Alien manager is not valid!");
                GetTree().Quit();
            }

            if (!(_playerManager is Node playerNode))
            {
                GD.PrintErr("ERROR: Player manager is not a node!");
                GetTree().Quit();
            }
            else if (!playerNode.IsValid())
            {
                GD.PrintErr("ERROR: Player manager is not valid!");
                GetTree().Quit();
            }

            if (!(_mothershipManager is Node mothershipNode))
            {
                GD.PrintErr("ERROR: Mothership manager is not a node!");
                GetTree().Quit();
            }
            else if (!mothershipNode.IsValid())
            {
                GD.PrintErr("ERROR: Mothership manager node is not valid!");
                GetTree().Quit();
            }

            if (!(_livesManager is Node livesNode))
            {
                GD.PrintErr("ERROR: Lives manager is not a node!");
                GetTree().Quit();
            }
            else if (!livesNode.IsValid())
            {
                GD.PrintErr("ERROR: Lives manager is not valid!");
                GetTree().Quit();
            }

            if (!_gameOverLabel.IsValid())
            {
                GD.PrintErr("ERROR: Game over label node is not valid!");
                GetTree().Quit();
            }
        }

        public async void OnAliensCleared()
        {
            _playerManager.EndGame();
            _alienManager.EndGame();
            _mothershipManager.EndGame();
            await ToSignal(GetTree().CreateTimer(3.0f), "timeout");
            _alienManager.StartGame();
            _playerManager.StartGame();
            _mothershipManager.StartGame();
        }

        public async void OnGameOver()
        {
            _gameOverLabel.Visible = true;
            _playerManager.EndGame();
            _alienManager.EndGame();
            _mothershipManager.EndGame();
            await ToSignal(GetTree().CreateTimer(3.0f), "timeout");
            GetTree().ChangeScene("res://Scenes/TitleScreen.tscn");
        }
    }
}