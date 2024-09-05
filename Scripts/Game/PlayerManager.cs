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
            _shipController = GetNode<ShipController>("PlayerController");
            _playerShip = GetNode<ShipBase>("PlayerShip");
            _playerMissile = GetNode<Projectile>("PlayerMissile");
            StartGame();
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