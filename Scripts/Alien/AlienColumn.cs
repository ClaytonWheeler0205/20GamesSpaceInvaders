using Godot;
using System;
using Util.ExtensionMethods;

namespace Game.Alien
{

    public class AlienColumn : ColumnBase
    {
        private Godot.Collections.Array<AlienBase> _aliens;
        private CollisionShape2D _columnCollision;
        private RayCast2D _playerDetector;

        private int _aliensCount = 0;

        private const string EDGE_NODE_GROUP = "Edge";
        private const string PLAYER_NODE_GROUP = "Player";

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            _aliens = new Godot.Collections.Array<AlienBase>();
            ConstructAliensArray();
            SetAlienConnections();

            SetNodeReferences();
            CheckNodeReferences();
            IsActive = true;
        }

        private void ConstructAliensArray()
        {
            Node2D aliensNode = GetNode<Node2D>("Aliens");
            if (aliensNode.IsValid())
            {
                int aliensNodeCount = aliensNode.GetChildCount();
                for (int i = 0; i < aliensNodeCount; i++)
                {
                    AlienBase alien = aliensNode.GetChild(i) as AlienBase;
                    if (alien.IsValid())
                    {
                        _aliens.Add(alien);
                    }
                }
                _aliensCount = _aliens.Count;
            }
            else
            {
                GD.PrintErr("ERROR: Aliens collection node is not found! Aliens array construction failed!");
            }
        }

        private void SetAlienConnections()
        {
            foreach (AlienBase alien in _aliens)
            {
                if (alien.IsValid())
                {
                    alien.Connect("AlienDestroyed", this, "OnAlienDestroyed");
                }
            }
        }

        private void SetNodeReferences()
        {
            _columnCollision = GetNode<CollisionShape2D>("CollisionShape2D");
            _playerDetector = GetNode<RayCast2D>("RayCast2D");
        }

        private void CheckNodeReferences()
        {
            if (!_columnCollision.IsValid())
            {
                GD.PrintErr("ERROR: Alien column collision node is not valid!");
            }
            if (!_playerDetector.IsValid())
            {
                GD.PrintErr("ERROR: Player detector node is not valid!");
            }
        }

        public override void AnimateAliens()
        {
            foreach (AlienBase alien in _aliens)
            {
                if (alien.IsValid() && alien.IsActive)
                {
                    alien.ChangeFrame();
                }
            }
        }

        public override Vector2 GetShootingAlien()
        {
            foreach (AlienBase alien in _aliens)
            {
                if (alien.IsValid() && alien.CanShoot())
                {
                    return alien.GlobalPosition;
                }
            }

            throw new InvalidOperationException("ERROR: No aliens are active in this column!");
        }

        public override void SetActiveStatus(bool newActiveState)
        {
            IsActive = newActiveState;

            if (IsActive)
            {
                _aliensCount = _aliens.Count;
                _columnCollision.SetDeferred("disabled", false);
            }
            else
            {
                _columnCollision.SetDeferred("disabled", true);
            }

            foreach (AlienBase alien in _aliens)
            {
                alien.SetActive(newActiveState);
            }
        }

        public override bool IsOverPlayer()
        {
            if (!IsActive) { return false;}

            Node collisionNode = _playerDetector.GetCollider() as Node;
            if (collisionNode == null) {return false;}
            else
            {
                return collisionNode.IsInGroup(PLAYER_NODE_GROUP);
            }
        }

        public void OnAlienDestroyed()
        {
            _aliensCount--;
            EmitSignal("ColumnAlienDestroyed");

            if (_aliensCount == 0)
            {
                SetActiveStatus(false);
            }
        }

        public void OnColumnAreaEntered(Area2D area)
        {
            if (area.IsInGroup(EDGE_NODE_GROUP))
            {
                EmitSignal("EdgeTouched");
            }
        }
    }
}