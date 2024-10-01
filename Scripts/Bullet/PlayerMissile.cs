using Game.Bus;
using Game.SFX;
using Godot;
using System;
using Util.ExtensionMethods;

namespace Game.Bullet
{

    public class PlayerMissile : Projectile
    {
        private AudioPlayer _missileSoundPlayer;

        private const string ALIEN_NODE_GROUP = "Alien";
        private const string BULLET_NODE_GROUP = "Bullet";

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            PlayerEventBus.Instance.Connect("PlayerShot", this, "OnPlayerShot");
            _missileSoundPlayer = GetNode<AudioPlayer>("AudioStreamPlayer");
            ProjectileDirection = -1;
            base._Ready();
        }

        public override void FireProjectile()
        {
            if (_missileSoundPlayer.IsValid())
            {
                _missileSoundPlayer.PlaySound("missile_fire");
            }
            base.FireProjectile();
        }

        public void OnScreenExit()
        {
            ResetProjectile();
        }

        public void OnMissileAreaEntered(Area2D area)
        {
            if (area.IsInGroup(ALIEN_NODE_GROUP) || area.IsInGroup(BULLET_NODE_GROUP))
            {
                ResetProjectile();
            }
        }

        public void OnPlayerShot()
        {
            ResetProjectile();
        }
    }
}