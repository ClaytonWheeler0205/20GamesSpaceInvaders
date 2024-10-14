using Game.Bus;
using Godot;
using System;
using Util;
using Util.ExtensionMethods;

namespace Game.Alien
{

    public class Mothership : Area2D, IMothershipControl
    {
        [Export]
        private Vector2 _startPositionLeft;
        [Export]
        private Vector2 _startPositionRight;

        [Export]
        private float _mothershipSpeed = 100.0f;
        private enum MovementDirection
        {
            Left,
            Right
        }
        private MovementDirection _movementDirection;

        private bool _isActive = false;

        private const string MISSILE_NODE_GROUP = "Missile";

        private AudioStreamPlayer _explosionSFX;
        private AudioStreamPlayer _movementSFX;

        private PackedScene _explosionVFX = GD.Load<PackedScene>("res://Scenes/MothershipExplosion.tscn");

        private const int MINIMUM_POINT_VALUE = 5;
        private const int MAXIMUM_POINT_VALUE = 30;
        private const int POINT_MULTIPLIER = 10;

        public override void _Ready()
        {
            SetNodeReferences();
            CheckNodeReferences();
            ResetShip();
        }

        private void SetNodeReferences()
        {
            _explosionSFX = GetNode<AudioStreamPlayer>("SFXPlayers/ExplosionPlayer");
            _movementSFX = GetNode<AudioStreamPlayer>("SFXPlayers/MovementPlayer");
        }

        private void CheckNodeReferences()
        {
            if (!_explosionSFX.IsValid())
            {
                GD.PrintErr("ERROR: Explosion audio stream player node is not valid!");
            }
            if (!_movementSFX.IsValid())
            {
                GD.PrintErr("ERROR: Movement sound player node is not valid!");
            }
        }

        public override void _Process(float delta)
        {
            if (_isActive)
            {
                int mothershipMovementDirection = 0;
                switch (_movementDirection)
                {
                    case MovementDirection.Left:
                        mothershipMovementDirection = -1;
                        break;
                    case MovementDirection.Right:
                        mothershipMovementDirection = 1;
                        break;
                }
                float newXPosition = Position.x + (_mothershipSpeed * mothershipMovementDirection * delta);
                Position = new Vector2(newXPosition, Position.y);
            }
        }

        public void StartShip()
        {
            _isActive = true;
            Visible = true;
            _movementSFX.Play();
        }

        public void ResetShip()
        {
            _isActive = false;
            Visible = false;
            ResetShipPosition();
            _movementSFX.Stop();
        }

        private void ResetShipPosition()
        {
            int randomPositionIndex = GDRandom.RandiRange(0, 1);
            switch (randomPositionIndex)
            {
                case 0:
                    Position = _startPositionLeft;
                    _movementDirection = MovementDirection.Right;
                    break;
                case 1:
                    Position = _startPositionRight;
                    _movementDirection = MovementDirection.Left;
                    break;
            }
        }

        public void OnMothershipAreaEntered(Area2D area)
        {
            if (area.IsInGroup(MISSILE_NODE_GROUP))
            {
                _explosionSFX.PitchScale = (float)GD.RandRange(0.75, 1.25);
                _explosionSFX.Play();
                PlayExplosionEffect();
                GivePoints();
                ResetShip();
            }
        }

        private void PlayExplosionEffect()
        {
            Node2D explosionEffect = _explosionVFX.Instance<Node2D>();
            explosionEffect.GlobalPosition = GlobalPosition;
            GetNode("/root").AddChild(explosionEffect);
        }

        private void GivePoints()
        {
            int points = GDRandom.RandiRange(MINIMUM_POINT_VALUE, MAXIMUM_POINT_VALUE) * POINT_MULTIPLIER;
            ScoreEventBus.Instance.EmitSignal("AwardPoints", points);
        }

        public void OnScreenExited()
        {
            ResetShip();
        }
    }
}