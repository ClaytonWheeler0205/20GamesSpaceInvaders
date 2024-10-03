using Game.Alien;
using Godot;
using System;
using Util.ExtensionMethods;

namespace Game.Bullet
{

    public class AlienBulletManager : BulletManagerBase
    {
        private Projectile _squigglyShot;
        private Projectile _plungerShot;
        private Projectile _rollingShot;

        private const int STANDARD_BULLET_SPEED = 334;
        private const int FASTER_BULLET_SPEED = 417;
        private const int BULLET_COUNT = 2;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            SetNodeReferences();
            CheckNodeReferences();
        }

        private void SetNodeReferences()
        {
            _squigglyShot = GetNode<Projectile>("SquiggleShot");
            _plungerShot = GetNode<Projectile>("PlungerShot");
        }

        private void CheckNodeReferences()
        {
            if (!_squigglyShot.IsValid())
            {
                GD.PrintErr("ERROR: squiggle shot node is not valid!");
            }
            if (!_plungerShot.IsValid())
            {
                GD.PrintErr("ERROR: plunger shot node in not valid!");
            }
        }

        public override void Fire()
        {
            int randomShot = (int)(GD.Randi() % BULLET_COUNT);
            switch (randomShot)
            {
                case 0:
                    FireSquiggleShot();
                    break;
                case 1:
                    FirePlungerShot();
                    break;
            }
        }

        public override void ResetShots()
        {
            _squigglyShot.ResetProjectile();
            _plungerShot.ResetProjectile();
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
                    if (randomColumn != null)
                    {
                        _squigglyShot.SetBulletPosition(randomColumn.GetShootingAlien());
                    }
                }
                catch (InvalidOperationException ex)
                {
                    GD.Print(ex.Message);
                    return;
                }
                _squigglyShot.FireProjectile();
            }
        }

        private void FirePlungerShot()
        {
            if (alienInvaders != null)
            {
                if (alienInvaders.AliensCount == 1)
                {
                    //FireRollingShot();
                    return;
                }
                else if (alienInvaders.AliensCount > 8)
                {
                    _plungerShot.ProjectileSpeed = STANDARD_BULLET_SPEED;
                }
                else
                {
                    _plungerShot.ProjectileSpeed = FASTER_BULLET_SPEED;
                }
                ColumnBase randomColumn = alienInvaders.GetRandomActiveColumn();
                try
                {
                    if (randomColumn != null)
                    {
                        _plungerShot.SetBulletPosition(randomColumn.GetShootingAlien());
                    }
                }
                catch (InvalidOperationException ex)
                {
                    GD.Print(ex.Message);
                    return;
                }
                _plungerShot.FireProjectile();
            }
        }

        private void FireRollingShot()
        {
            if (alienInvaders != null)
            {
                if (alienInvaders.AliensCount > 8)
                {
                    _rollingShot.ProjectileSpeed = STANDARD_BULLET_SPEED;
                }
                else
                {
                    // set projectile speed to FASTER_BULLET_SPEED
                    _rollingShot.ProjectileSpeed = FASTER_BULLET_SPEED;
                }
                // Set the columnBase to be the column over the player
                ColumnBase targetColumn = alienInvaders.GetColumnOverPlayer();
                try
                {
                    // set rolling shot's position to targetColumn.GetShootingAlien()
                    _rollingShot.SetBulletPosition(targetColumn.GetShootingAlien());
                }
                catch (InvalidOperationException ex)
                {
                    GD.Print(ex.Message);
                    return;
                }
            }
        }
    }
}