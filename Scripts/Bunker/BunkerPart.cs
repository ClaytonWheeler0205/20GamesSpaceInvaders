using Godot;
using System;
using Util.ExtensionMethods;

namespace Game.Bunker
{

    public class BunkerPart : Area2D
    {
        [Export]
        private Godot.Collections.Array<Texture> _damageTextures;
        private int _currentBunkerDamage = 0;
        private const int MAX_BUNKER_DAMAGE = 3;
        private const int INDEX_OFFSET = 1;

        private const string MISSILE_NODE_GROUP = "Missile";
        private const string BULLET_NODE_GROUP = "Bullet";
        private const string ALIEN_NODE_GROUP = "Alien";

        private Sprite _bunkerPartVisual;

        private PackedScene _bulletExplosionVFX = GD.Load<PackedScene>("res://Scenes/BulletExplosion.tscn");

        public override void _Ready()
        {
            _bunkerPartVisual = GetNode<Sprite>("Sprite");
            if (!_bunkerPartVisual.IsValid())
            {
                GD.PrintErr("ERROR: Bunker part sprite node is not valid!");
            }
        }

        public void OnBunkerPartAreaEntered(Area2D area)
        {
            if (area.IsInGroup(MISSILE_NODE_GROUP) || area.IsInGroup(BULLET_NODE_GROUP))
            {
                PlayExplosionEffect();
                _currentBunkerDamage++;
                if (_currentBunkerDamage == MAX_BUNKER_DAMAGE)
                {
                    this.SafeQueueFree();
                }
                else
                {
                    _bunkerPartVisual.Texture = _damageTextures[_currentBunkerDamage - INDEX_OFFSET];
                }
            }
            else if (area.IsInGroup(ALIEN_NODE_GROUP))
            {
                this.SafeQueueFree();
            }
        }

        private void PlayExplosionEffect()
        {
            Node2D particleEffect = _bulletExplosionVFX.Instance<Node2D>();
            particleEffect.GlobalPosition = GlobalPosition;
            GetNode("/root").AddChild(particleEffect);
        }
    }
}