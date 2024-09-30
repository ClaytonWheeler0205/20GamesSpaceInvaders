using Godot;
using System;
using System.Drawing.Imaging;

namespace Game.Bullet
{

    public class AlienBullet : Projectile
    {

        private AnimatedSprite _bulletSprite;

        private const string PLAYER_NODE_GROUP = "Player";

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            _bulletSprite = GetNode<AnimatedSprite>("AnimatedSprite");
            ProjectileDirection = 1;
            base._Ready();
        }

        public override void FireProjectile()
        {
            _bulletSprite.Play();
            base.FireProjectile();
        }

        public override void ResetProjectile()
        {
            _bulletSprite.Frame = 0;
            _bulletSprite.Playing = false;
            base.ResetProjectile();
        }

        public void OnScreenExit()
        {
            ResetProjectile();
        }

        public void OnBulletAreaEntered(Area2D area)
        {
            if (area.IsInGroup(PLAYER_NODE_GROUP))
            {
                ResetProjectile();
            }
        }
    }
}