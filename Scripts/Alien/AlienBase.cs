using Godot;
using System;

namespace Game.Alien
{

    public abstract class AlienBase : Area2D
    {
        private bool _isActive;
        public bool IsActive
        {
            get { return _isActive; }
            set { _isActive = value; }
        }

        [Signal]
        delegate void AlienDestroyed();

        public abstract void ChangeFrame();
        public abstract void SetActive(bool newActiveState);
        public abstract bool CanShoot();
    }
}