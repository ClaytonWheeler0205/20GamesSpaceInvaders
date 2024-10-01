using Godot;
using System;

namespace Game.Bus
{

    public class PlayerEventBus : Node
    {
        public static PlayerEventBus Instance {get; private set;}

        // Player signals
        [Signal]
        public delegate void PlayerShot();

        [Signal]
        public delegate void PlayerRespawn();

        public override void _Ready()
        {
            Instance = this;
        }
    }
}