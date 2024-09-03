using Godot;
using System;

namespace Game.Player
{

    public abstract class ShipBase : Area2D
    {
        [Export]
        private float _shipSpeed = 300.0f;
        protected float ShipSpeed => _shipSpeed;
        [Export]
        private float _minXValue;
        protected float MinXValue => _minXValue;
        [Export]
        private float _maxXValue;
        protected float MaxXValue => _maxXValue;

        /// <summary>
        /// Sets the current direction the ship should move. Determined by user input from a controller script
        /// </summary>
        /// <param name="direction">The new direction the ship should move. Either -1, 0, or 1</param>
        public abstract void SetDirection(float direction);

        /// <summary>
        /// Resets the ship to its starting properties.
        /// </summary>
        public abstract void ResetShip();
    }
}