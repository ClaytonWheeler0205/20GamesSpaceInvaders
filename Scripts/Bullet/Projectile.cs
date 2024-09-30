using Godot;
using System;

namespace Game.Bullet
{

    /// <summary>
    /// Base class for the projectiles in the game (i.e. the player missile and the alien bullet)
    /// Will provide base functionality such as movement, setting its starting position and making it active.
    /// </summary>
    public abstract class Projectile : Area2D
    {
        // Member variables
        [Export]
        private float _projectileSpeed = 300.0f;
        public float ProjectileSpeed
        {
            get { return _projectileSpeed; }
            set
            {
                if (value > 0)
                {
                    _projectileSpeed = value;
                }
            }
        }
        private float _projectileDirection = 0;
        protected float ProjectileDirection
        {
            get { return _projectileDirection; }
            set
            {
                _projectileDirection = Mathf.Clamp(value, -1, 1);
            }
        }
        private bool _isActive = false;
        public bool IsActive => _isActive;
        private Vector2 _startingPosition;

        public override void _Ready()
        {
            Visible = false;
            _startingPosition = Position;
        }

        // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(float delta)
        {
            if (_isActive)
            {
                float newYPosition = Position.y + (_projectileSpeed * _projectileDirection * delta);
                Position = new Vector2(Position.x, newYPosition);
            }
        }

        public void SetBulletPosition(Vector2 newPosition)
        {
            if (newPosition != null && !_isActive)
            {
                Position = newPosition;
            }
        }

        public virtual void FireProjectile()
        {
            _isActive = true;
            Visible = true;
        }

        public virtual void ResetProjectile()
        {
            _isActive = false;
            Position = _startingPosition;
            Visible = false;
        }
    }
}