using Game.Bullet;
using Game.Player;
using Godot;
using System;
using Util.ExtensionMethods;

namespace Game
{

    public class PlayerManager : Node, IGameManager
    {
        private ShipController _shipController;
        private ShipBase _playerShip;
        private Projectile _playerMissile;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            SetNodeReferences();
            CheckNodeReferences();
        }

        private void SetNodeReferences()
        {
            _shipController = GetNode<ShipController>("PlayerController");
            _playerShip = GetNode<ShipBase>("PlayerShip");
            _playerMissile = GetNode<Projectile>("PlayerMissile");
        }

        private void CheckNodeReferences()
        {
            if (!_shipController.IsValid())
            {
                GD.PrintErr("ERROR: Ship controller node is not valid!");
            }
            if (!_playerShip.IsValid())
            {
                GD.PrintErr("ERROR: Player shipt node is not valid!");
            }
            if (!_playerMissile.IsValid())
            {
                GD.PrintErr("ERROR: Player missile node is not valid!");
            }
        }

        public bool StartGame()
        {
            if (_shipController.IsValid())
            {
                _shipController.IsControllerActive = true;
                if (_playerShip.IsValid())
                {
                    _shipController.ShipToControl = _playerShip;
                    _playerShip.ResetShip();
                }
                else
                {
                    return false;
                }
                if (_playerMissile.IsValid())
                {
                    _shipController.ShipProjectile = _playerMissile;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
            return true;
        }

        public bool EndGame()
        {
            if (_shipController.IsValid())
            {
                _shipController.IsControllerActive = false;
            }
            else
            {
                return false;
            }
            return true;
        }
    }
}