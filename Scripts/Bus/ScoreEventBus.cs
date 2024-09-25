using Godot;
using System;

namespace Game.Bus
{

    public class ScoreEventBus : Node
    {
        public static ScoreEventBus Instance { get; private set; }

        // Score signals

        [Signal]
        public delegate void AwardPoints(int pointsToGive);

        public override void _Ready()
        {
            Instance = this;
        }
    }
}