using Game.Bus;
using Godot;
using System;
using Util.ExtensionMethods;

namespace Game.Player
{

    public class PlayerController : ShipController
    {

        public override void _Ready()
        {
            PlayerEventBus.Instance.Connect("PlayerShot", this, "OnPlayerShot");
            PlayerEventBus.Instance.Connect("PlayerRespawn", this, "OnPlayerRespawn");
        }

        public override void _Process(float delta)
        {
            if (IsControllerActive)
            {
                float shipDirection = Input.GetAxis("move_left", "move_right");

                if (ShipToControl.IsValid())
                {
                    ShipToControl.SetDirection(shipDirection);
                }
            }
        }

        public override void _UnhandledInput(InputEvent @event)
        {
            if (IsControllerActive)
            {
                if (@event.IsActionPressed("fire") && ShipProjectile.IsValid() && !ShipProjectile.IsActive)
                {
                    ShipProjectile.SetBulletPosition(ShipToControl.Position);
                    ShipProjectile.FireProjectile();
                }
            }
        }

        public void OnPlayerShot()
        {
            IsControllerActive = false;
        }

        public void OnPlayerRespawn()
        {
            IsControllerActive = true;
        }
    }
}