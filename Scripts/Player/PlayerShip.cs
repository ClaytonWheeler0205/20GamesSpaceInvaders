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

        private PackedScene _playerExplosionEffect = GD.Load<PackedScene>("res://Scenes/PlayerExplosion.tscn");

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
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
        }

        private async void PlayerShot()
        {
            GD.Print("Player got shot!");
            Visible = false;
            _currentDirection = 0;
            PlayerEventBus.Instance.EmitSignal("PlayerShot");
            if (_audioStreamPlayer.IsValid())
            {
                _audioStreamPlayer.Play();
            }
            // Create a explosion particle effect
            Node2D playerExplosion = _playerExplosionEffect.Instance<Node2D>();
            playerExplosion.GlobalPosition = GlobalPosition;
            GetNode("/root").AddChild(playerExplosion);
            await ToSignal(GetTree().CreateTimer(4.5f), "timeout");
            ResetShip();
            PlayerEventBus.Instance.EmitSignal("PlayerRespawn");
        }
    }
}