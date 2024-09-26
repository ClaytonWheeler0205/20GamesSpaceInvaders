using Godot;
using System;
using Util.ExtensionMethods;

namespace Game
{

    public class SpaceInvadersMain : Node
    {

        private IGameManager _alienManager;
        private IGameManager _playerManager;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            SetNodeReferences();
            CheckNodeReferences();
            _alienManager.StartGame();
            _playerManager.StartGame();
        }

        private void SetNodeReferences()
        {
            _alienManager = GetNode<IGameManager>("AlienManager");
            _playerManager = GetNode<IGameManager>("PlayerManager");
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
        }

        public async void OnAliensCleared()
        {
            _playerManager.EndGame();
            _alienManager.EndGame();
            await ToSignal(GetTree().CreateTimer(3.0f), "timeout");
            _alienManager.StartGame();
            _playerManager.StartGame();
        }
    }
}