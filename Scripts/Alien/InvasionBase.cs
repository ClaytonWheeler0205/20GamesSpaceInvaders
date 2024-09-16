using Godot;

namespace Game.Alien
{

    public abstract class InvasionBase : Node2D
    {
        private int _aliensCount;
        public int AliensCount => _aliensCount;

        public abstract void Move();

        protected void SetAliensCount(int count)
        {
            _aliensCount = count;
        }
    }
}