using Game.Bus;
using Godot;
using System;
using Util.ExtensionMethods;

namespace Game.Alien
{

    public class AlienInvader : AlienBase
    {
        [Export]
        private int _pointValue;

        private AnimatedSprite _alienSprite;
        private CollisionShape2D _alienCollision;
        private RayCast2D _alienDetector;
        private AudioStreamPlayer _audioStreamPlayer;

        private const string ALIEN_NODE_GROUP = "Alien";
        private const string MISSILE_NODE_GROUP = "Missile";

        private PackedScene _alienExplosionVFX = GD.Load<PackedScene>("res://Scenes/AlienExplosion.tscn");

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            SetNodeReferences();
            CheckNodeReferences();
            IsActive = true;
        }

        private void SetNodeReferences()
        {
            _alienSprite = GetNode<AnimatedSprite>("AnimatedSprite");
            _alienCollision = GetNode<CollisionShape2D>("CollisionShape2D");
            _alienDetector = GetNode<RayCast2D>("RayCast2D");
            _audioStreamPlayer = GetNode<AudioStreamPlayer>("AudioStreamPlayer");
        }

        private void CheckNodeReferences()
        {
            if (!_alienSprite.IsValid())
            {
                GD.PrintErr("ERROR: Alien sprite node is not valid!");
            }
            if (!_alienCollision.IsValid())
            {
                GD.PrintErr("ERROR: Alien collision node is not valid!");
            }
            if (!_alienDetector.IsValid())
            {
                GD.PrintErr("ERROR: Alien detector node is not valid!");
            }
            if (!_audioStreamPlayer.IsValid())
            {
                GD.PrintErr("ERROR: Audio stream player node is not valid!");
            }
        }

        public override void ChangeFrame()
        {
            if (_alienSprite.Frame == 0)
            {
                _alienSprite.Frame = 1;
            }
            else
            {
                _alienSprite.Frame = 0;
            }
        }

        /// <summary>
        /// Sets the alien's active state, which includes the collision and visibility.
        /// </summary>
        /// <param name="newActiveState">True if the alien is to be visible and have active collision. False if otherwise.</param>
        public override void SetActive(bool newActiveState)
        {
            IsActive = newActiveState;
            if (IsActive)
            {
                _alienSprite.Visible = true;
                _alienSprite.Frame = 0;

                _alienCollision.SetDeferred("disabled", false);
            }
            else
            {
                _alienSprite.Visible = false;

                _alienCollision.SetDeferred("disabled", true);
            }
        }

        public override bool CanShoot()
        {
            // If the node isn't active, it can't shoot
            if (!IsActive)
            {
                return false;
            }

            Node collisionNode = _alienDetector.GetCollider() as Node;
            // If the ray cast finds nothing, it can shoot
            if (collisionNode == null)
            {
                return true;
            }
            // If the raycast finds something, check that it is not an alien
            else
            {
                return !collisionNode.IsInGroup(ALIEN_NODE_GROUP);
            }
        }

        public void OnAlienAreaEntered(Area2D area)
        {
            if (area.IsInGroup(MISSILE_NODE_GROUP))
            {
                SetActive(false);
                PlayExplosionEffect();
                _audioStreamPlayer.Play();
                EmitSignal("AlienDestroyed");
                ScoreEventBus.Instance.EmitSignal("AwardPoints", _pointValue);
            }
        }

        private void PlayExplosionEffect()
        {
            Node2D particleEffect = _alienExplosionVFX.Instance<Node2D>();
            particleEffect.GlobalPosition = GlobalPosition;
            GetNode("/root").AddChild(particleEffect);
        }
    }
}