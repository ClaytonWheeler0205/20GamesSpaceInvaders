using Godot;
using System;
using Util;

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

        [Signal]
        public delegate void MothershipInactive();

        public override void _Ready()
        {
            ResetShip();
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
        }

        public void ResetShip()
        {
            _isActive = false;
            Visible = false;
            ResetShipPosition();
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
                ResetShip();
                EmitSignal(nameof(MothershipInactive));
            }
        }

        public void OnScreenExited()
        {
            ResetShip();
            EmitSignal(nameof(MothershipInactive));
        }
    }
}