using Game.Alien;
using Godot;
using System;

namespace Game.Bullet
{

    public abstract class BulletManagerBase : Node
    {
        protected InvasionBase alienInvaders = null;
        
        public void SetAlienInvaders(InvasionBase invaders)
        {
            alienInvaders = invaders;
        }

        public abstract void Fire();

        public abstract void ResetShots();

    }
}