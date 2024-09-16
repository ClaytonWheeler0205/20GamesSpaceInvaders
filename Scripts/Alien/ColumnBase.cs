using Godot;
using System;

namespace Game.Alien
{

    public abstract class ColumnBase : Area2D
    {
        private bool _isActive;
        public bool IsActive
        {
            get { return _isActive; }
            set { _isActive = value; }
        }

        [Signal]
        delegate void EdgeTouched();

        [Signal]
        delegate void ColumnAlienDestroyed();

        public abstract void AnimateAliens();

        public abstract Vector2 GetShootingAlien();

        public abstract void SetActiveStatus(bool newActiveState);
    }
}