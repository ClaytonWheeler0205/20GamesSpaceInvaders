using Godot;
using System;

namespace Game.Player
{

    public class PlayerShip : ShipBase
    {
        private Vector2 _startPosition;
        private float _currentDirection;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            _startPosition = Position;
            Visible = false;
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
    }
}