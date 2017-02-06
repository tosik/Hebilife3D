using System.Collections.Generic;
using System.Linq;

namespace Hebilife
{
    public class Snake
    {
        public Position Head { get { return _bodies.Last(); } }
        public Position NextPosition { get { return Head + Direction.AsPosition(); } }
        public Direction Direction { get; private set; }

        Queue<Position> _bodies = new Queue<Position>();

        public Snake()
        {
            Direction = Direction.North;
        }

        public void Turn(RelativeDirection relative)
        {
            Direction = Direction.Turn(relative);
        }

        public void Move()
        {
            Extend();
            Shrink();
        }

        public void MoveAndEat()
        {
            Extend();
        }

        void Extend()
        {
            _bodies.Enqueue(NextPosition);
        }

        void Shrink()
        {
            _bodies.Dequeue();
        }
    }
}
