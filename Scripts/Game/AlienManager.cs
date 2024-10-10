using Game.Alien;
using Game.Bullet;
using Game.Bus;
using Game.SFX;
using Godot;
using System;
using Util.ExtensionMethods;

namespace Game
{

    public class AlienManager : Node, IGameManager
    {
        private InvasionBase _alienInvaders;
        private Timer _movementTimer;
        private Timer _shootingTimer;
        private AudioPlayer _movementSoundPlayer;
        private BulletManagerBase _bulletManager;

        private float _minShootCooldown = 0.02f;
        private float _maxShootCooldown = 1.0f;
        private const int ALIEN_COUNT_MAX = 55;

        [Signal]
        public delegate void AliensCleared();

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            PlayerEventBus.Instance.Connect("PlayerShot", this, "OnPlayerShot");
            PlayerEventBus.Instance.Connect("PlayerRespawn", this, "OnPlayerRespawn");
            SetNodeReferences();
            if (_bulletManager.IsValid())
            {
                _bulletManager.SetAlienInvaders(_alienInvaders);
            }
            _alienInvaders.Visible = false;
        }

        private void SetNodeReferences()
        {
            _alienInvaders = GetNode<InvasionBase>("AlienInvaders");
            _movementTimer = GetNode<Timer>("MovementTimer");
            _shootingTimer = GetNode<Timer>("ShootingTimer");
            _movementSoundPlayer = GetNode<AudioPlayer>("MovementSoundPlayer");
            _bulletManager = GetNode<BulletManagerBase>("BulletManager");
        }

        public bool StartGame()
        {
            _alienInvaders.Visible = true;
            _movementTimer.Start(_alienInvaders.AliensCount / 60.0f);
            _shootingTimer.Start(_maxShootCooldown);
            return true;
        }

        public bool EndGame()
        {
            _alienInvaders.Visible = false;
            _alienInvaders.ResetInvasion();
            _bulletManager.ResetShots();
            _movementTimer.Stop();
            _shootingTimer.Stop();
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

        public void OnShootingTimerTimeout()
        {
            if (_alienInvaders.AliensCount > 0)
            {
                _bulletManager.Fire();
                float maximumCooldown = Mathf.Lerp(_minShootCooldown * 4, _maxShootCooldown, _alienInvaders.AliensCount / (float)ALIEN_COUNT_MAX);
                _shootingTimer.Start((float)GD.RandRange(_minShootCooldown, maximumCooldown));
            }
        }

        public void OnPlayerShot()
        {
            _movementTimer.Stop();
            _shootingTimer.Stop();
        }

        public void OnPlayerRespawn()
        {
            _movementTimer.Start(_alienInvaders.AliensCount / 60.0f);
            _shootingTimer.Start(_maxShootCooldown);
        }
    }
}