using Game.Bus;
using Godot;
using System;
using System.Drawing.Imaging;

namespace Game.Bullet
{

    public class AlienBullet : Projectile
    {

        private AnimatedSprite _bulletSprite;

        private const string PLAYER_NODE_GROUP = "Player";
        private const string MISSILE_NODE_GROUP = "Missile";

        private PackedScene _bulletExplosionVFX = GD.Load<PackedScene>("res://Scenes/BulletExplosion.tscn");

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            PlayerEventBus.Instance.Connect("PlayerShot", this, "OnPlayerShot");
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
            else if (area.IsInGroup(MISSILE_NODE_GROUP))
            {
                Node2D particleEffect = _bulletExplosionVFX.Instance<Node2D>();
                particleEffect.GlobalPosition = GlobalPosition;
                GetNode("/root").AddChild(particleEffect);
                ResetProjectile();
            }
        }

        public void OnPlayerShot()
        {
            ResetProjectile();
        }
    }
}