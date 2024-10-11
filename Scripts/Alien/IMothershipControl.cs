using Godot;
using System;

namespace Game.Alien
{

    public interface IMothershipControl
    {
        void StartShip();

        void ResetShip();
    }
}