using Game.Bus;
using Godot;
using System;
using Util.ExtensionMethods;

namespace Game.Alien
{

    public class AlienInvaders : InvasionBase
    {
        private const int ALIEN_INVASION_SIZE = 55;

        private const float DELTA_X = 16f;
        private const float DELTA_Y = 20f;

        private const string ALIEN_NODE_GROUP = "Alien";

        private bool _alienHitGround = false;

        private Godot.Collections.Array<ColumnBase> _alienColumns;

        private enum MovementState
        {
            Left,
            Right,
            DownLeft,
            DownRight
        }
        private MovementState _currentMovementState;

        private Vector2 _alienStartingPosition;

        public override void _Ready()
        {
            SetAliensCount(ALIEN_INVASION_SIZE);

            _alienColumns = new Godot.Collections.Array<ColumnBase>();
            ConstructAlienColumnsArray();
            SetColumnConnections();

            _currentMovementState = MovementState.Right;
            _alienStartingPosition = GlobalPosition;
        }

        private void ConstructAlienColumnsArray()
        {
            Node2D alienColumnsNode = GetNode<Node2D>("AlienColumns");
            if (alienColumnsNode.IsValid())
            {
                int alienColumnsNodeCount = alienColumnsNode.GetChildCount();
                for (int i = 0; i < alienColumnsNodeCount; i++)
                {
                    ColumnBase alienColumn = alienColumnsNode.GetChild(i) as ColumnBase;
                    if (alienColumn.IsValid())
                    {
                        _alienColumns.Add(alienColumn);
                    }
                }
            }
            else
            {
                GD.PrintErr("ERROR: Alien columns node not found. Alien columns array construction failed!");
            }
        }

        private void SetColumnConnections()
        {
            foreach (ColumnBase column in _alienColumns)
            {
                if (column.IsValid())
                {
                    column.Connect("ColumnAlienDestroyed", this, "OnColumnAlienDestroyed");
                    column.Connect("EdgeTouched", this, "OnEdgeTouched");
                }
            }
        }

        public override void Move()
        {
            MoveAliens();
            AnimateAliens();
        }

        public override void ResetInvasion()
        {
            SetAliensCount(ALIEN_INVASION_SIZE);
            ResetColumns();
            _currentMovementState = MovementState.Right;
            GlobalPosition = _alienStartingPosition;
        }

        private void ResetColumns()
        {
            foreach (ColumnBase column in _alienColumns)
            {
                column.SetActiveStatus(true);
            }
        }

        public override ColumnBase GetRandomActiveColumn()
        {
            GD.Randomize();
            ColumnBase activeColumn = null;
            // check if there is an active column
            if (ActiveColumnPresent())
            {
                do
                {
                    int randomIndex = (int)(GD.Randi() % _alienColumns.Count);
                    activeColumn = _alienColumns[randomIndex];
                } while (activeColumn == null || !activeColumn.IsActive);
            }
            return activeColumn;
        }

        public override ColumnBase GetColumnOverPlayer()
        {
            foreach (ColumnBase column in _alienColumns)
            {
                if (column.IsOverPlayer())
                {
                    return column;
                }
            }

            return null;
        }

        private bool ActiveColumnPresent()
        {
            foreach (ColumnBase column in _alienColumns)
            {
                if (column.IsValid() && column.IsActive)
                {
                    return true;
                }
            }
            return false;
        }

        private void MoveAliens()
        {
            Vector2 newPosition = GlobalPosition;
            switch (_currentMovementState)
            {
                case MovementState.Left:
                    newPosition.x -= DELTA_X;
                    break;
                case MovementState.Right:
                    newPosition.x += DELTA_X;
                    break;
                case MovementState.DownLeft:
                    newPosition.y += DELTA_Y;
                    _currentMovementState = MovementState.Right;
                    break;
                case MovementState.DownRight:
                    newPosition.y += DELTA_Y;
                    _currentMovementState = MovementState.Left;
                    break;
            }
            GlobalPosition = newPosition;
        }

        private void AnimateAliens()
        {

            foreach (ColumnBase alienColumn in _alienColumns)
            {
                if (alienColumn.IsValid() && alienColumn.IsActive)
                {
                    alienColumn.AnimateAliens();
                }
            }
        }

        public void OnEdgeTouched()
        {
            switch (_currentMovementState)
            {
                case MovementState.Left:
                    _currentMovementState = MovementState.DownLeft;
                    break;
                case MovementState.Right:
                    _currentMovementState = MovementState.DownRight;
                    break;
            }
        }

        public void OnColumnAlienDestroyed()
        {
            SetAliensCount(AliensCount - 1);
        }

        public void OnAlienAreaEntered(Area2D area)
        {
            if (area.IsInGroup(ALIEN_NODE_GROUP) && !_alienHitGround)
            {
                _alienHitGround = true;
                PlayerEventBus.Instance.EmitSignal("AlienHitGround");
            }
        }
    }
}