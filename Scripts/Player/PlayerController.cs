using Godot;
using System;
using Util.ExtensionMethods;

namespace Game.Player
{

    public class PlayerController : ShipController
    {

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
    }
}