using Godot;
using System;

namespace Game.Bullet
{

    public class PlayerMissile : Projectile
    {

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            ProjectileDirection = -1;
            base._Ready();
        }

        public void OnScreenExit()
        {
            ResetProjectile();
        }
    }
}