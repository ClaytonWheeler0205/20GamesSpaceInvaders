using Game.Bus;
using Godot;
using System;
using Util.ExtensionMethods;

namespace Game.Player
{

    public class PlayerShip : ShipBase
    {
        private AudioStreamPlayer _audioStreamPlayer;

        private Vector2 _startPosition;
        private float _currentDirection;

        private const string BULLET_NODE_GROUP = "Bullet";
        private const string ALIEN_NODE_GROUP = "Alien";

        private PackedScene _playerExplosionEffect = GD.Load<PackedScene>("res://Scenes/PlayerExplosion.tscn");

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            PlayerEventBus.Instance.Connect("PlayerRespawn", this, "OnPlayerRespawn");
            SetNodeReferences();
            CheckNodeReferences();
            _startPosition = Position;
            Visible = false;
        }

        private void SetNodeReferences()
        {
            _audioStreamPlayer = GetNode<AudioStreamPlayer>("AudioStreamPlayer");
        }

        private void CheckNodeReferences()
        {
            if (!_audioStreamPlayer.IsValid())
            {
                GD.PrintErr("ERROR: Audio stream player node is not valid!");
            }
        }

        //  // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(float delta)
        {
            float newXPosition = Position.x + (_currentDirection * ShipSpeed * delta);
            newXPosition = Mathf.Clamp(newXPosition, MinXValue, MaxXValue);
            Position = new Vector2(newXPosition, Position.y);
        }

        public override void SetDirection(float direction)
        {
            _currentDirection = Mathf.Clamp(direction, -1, 1);
        }

        public override void ResetShip()
        {
            Position = _startPosition;
            Visible = true;
        }

        public void OnPlayerAreaEntered(Area2D area)
        {
            if (area.IsInGroup(BULLET_NODE_GROUP))
            {
                PlayerShot();
            }
            else if (area.IsInGroup(ALIEN_NODE_GROUP))
            {
                PlayerHitAlien();
            }
        }

        private async void PlayerShot()
        {
            ExplodePlayer();
            PlayerEventBus.Instance.EmitSignal("PlayerShot");
            await ToSignal(GetTree().CreateTimer(1.5f), "timeout");
            LivesEventBus.Instance.EmitSignal("LoseLife");
        }

        private async void PlayerHitAlien()
        {
            ExplodePlayer();
            PlayerEventBus.Instance.EmitSignal("PlayerShot");
            await ToSignal(GetTree().CreateTimer(4.5f), "timeout");
            PlayerEventBus.Instance.EmitSignal("AlienHit");
        }

        private void ExplodePlayer()
        {
            Visible = false;
            _currentDirection = 0;
            if (_audioStreamPlayer.IsValid())
            {
                _audioStreamPlayer.Play();
            }
            Node2D playerExplosion = _playerExplosionEffect.Instance<Node2D>();
            playerExplosion.GlobalPosition = GlobalPosition;
            GetNode("/root").AddChild(playerExplosion);
        }

        public void OnPlayerRespawn()
        {
            ResetShip();
        }
    }
}