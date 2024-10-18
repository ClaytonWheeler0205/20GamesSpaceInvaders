using Godot;
using Util.ExtensionMethods;

namespace Game.Bunker
{

    public class BunkerBuilder : Node, IBunkerBuilder
    {
        private const float START_X = -290f;
        private const float START_Y = 140f;
        private const float DELTA_X = 195f;

        private PackedScene _bunker = GD.Load<PackedScene>("res://Scenes/Bunker.tscn");
        private const int BUNKER_COUNT = 4;

        public override void _Ready()
        {
            BuildBunkers();
        }

        public void RebuildBunkers()
        {
            DestroyBunkers();
            BuildBunkers();
        }

        private void BuildBunkers()
        {
            for (int i = 0; i < BUNKER_COUNT; i++)
            {
                Node2D bunker = _bunker.Instance<Node2D>();
                bunker.GlobalPosition = new Vector2(START_X + (DELTA_X * i), START_Y);
                AddChild(bunker);
            }
        }

        private void DestroyBunkers()
        {
            int numberChildren = GetChildCount();
            for (int i = 0; i < numberChildren; i++)
            {
                GetChild(i).SafeQueueFree();
            }
        }
    }
}