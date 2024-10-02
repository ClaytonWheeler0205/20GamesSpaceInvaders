using Godot;
using System;

namespace Game.Bus
{

    public class LivesEventBus : Node
    {
        public static LivesEventBus Instance { get; private set;}

        // Lives signals
        [Signal]
        public delegate void LoseLife();
        
        [Signal]
        public delegate void GainLife();

        public override void _Ready()
        {
            Instance = this;
        }
    }
}