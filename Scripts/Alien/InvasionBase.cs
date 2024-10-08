using System.Runtime;
using Godot;

namespace Game.Alien
{

    public abstract class InvasionBase : Node2D
    {
        private int _aliensCount;
        public int AliensCount => _aliensCount;

        public abstract void Move();

        public abstract void ResetInvasion();

        public abstract ColumnBase GetRandomActiveColumn();

        public abstract ColumnBase GetColumnOverPlayer();

        protected void SetAliensCount(int count)
        {
            _aliensCount = count;
        }
    }
}