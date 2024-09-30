using Game.Alien;
using Godot;
using System;
using Util.ExtensionMethods;

namespace Game.Bullet
{

    public class AlienBulletManager : BulletManagerBase
    {
        private Projectile _squigglyShot;

        private const int STANDARD_BULLET_SPEED = 334;
        private const int FASTER_BULLET_SPEED = 417;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            SetNodeReferences();
            CheckNodeReferences();
        }

        private void SetNodeReferences()
        {
            _squigglyShot = GetNode<Projectile>("SquiggleShot");
        }

        private void CheckNodeReferences()
        {
            if (!_squigglyShot.IsValid())
            {
                GD.PrintErr("ERROR: squiggle shot node is not valid!");
            }
        }

        public override void Fire()
        {
            FireSquiggleShot();
        }

        public override void ResetShots()
        {
            _squigglyShot.ResetProjectile();
        }

        private void FireSquiggleShot()
        {
            if (alienInvaders != null)
            {
                if (alienInvaders.AliensCount > 8)
                {
                    _squigglyShot.ProjectileSpeed = STANDARD_BULLET_SPEED;
                }
                else
                {
                    _squigglyShot.ProjectileSpeed = FASTER_BULLET_SPEED;
                }
                ColumnBase randomColumn = alienInvaders.GetRandomActiveColumn();
                try
                {
                    _squigglyShot.SetBulletPosition(randomColumn.GetShootingAlien());
                }
                catch (InvalidOperationException ex)
                {
                    GD.Print(ex.Message);
                    return;
                }
                _squigglyShot.FireProjectile();
            }
        }
    }
}