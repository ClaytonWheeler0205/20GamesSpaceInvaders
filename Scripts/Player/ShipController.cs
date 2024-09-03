using Godot;
using System;

namespace Game.Player
{

    /// <summary>
    /// Base class that stores information about the ship that it is controlling. Concrete implementations will determine exactly
    /// how the ship is controlled.
    /// </summary>
    public abstract class ShipController : Node
    {
        private ShipBase _shipToControl;
        public ShipBase ShipToControl
        {
            get { return _shipToControl; }
            set
            {
                if (value != null)
                {
                    _shipToControl = value;
                }
            }
        }
        private bool _isControllerActive;
        public bool IsControllerActive
        {
            get { return _isControllerActive;}
            set { _isControllerActive = value;}
        }
    }
}