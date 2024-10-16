using Godot;
using System;

namespace Game.UI
{

    public class TitleScreen : Control
    {
        public override void _UnhandledInput(InputEvent @event)
        {
            if (@event.IsActionPressed("fire"))
            {
                GetTree().ChangeScene("res://Scenes/Main.tscn");
            }
        }
    }
}