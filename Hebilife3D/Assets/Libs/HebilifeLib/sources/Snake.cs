using System.Collections.Generic;
using System.Linq;

namespace Hebilife
{
    public class Snake
    {
        public Position Head { get { return _bodies.Last(); } }
        public Position NextPosition { get { return Head + Direction.AsPosition(); } }
        public Direction Direction { get; private set; }
        public bool Dead { get; private set; }
        public IEnumerable<Position> Bodies { get { return _bodies; } }
        public IEnumerable<Position> BodiesWithoutTerminal { get { return _bodies.Skip(1); } }
        public bool Mature { get { return _bodies.Count() >= 10; } }
        public Brain Brain { get { return _brain; } }

        Queue<Position> _bodies = new Queue<Position>();
        Brain _brain = new Brain();

        public Snake()
        {
            Direction = Direction.North;
        }

        public Snake(Position head, Direction direction = Direction.North)
        {
            Direction = direction;
            _bodies.Enqueue(head);
        }

        public Snake(Snake parent, IEnumerable<Position> bodies)
        {
            Direction = Direction.North;
            foreach (var body in bodies)
            {
                _bodies.Enqueue(body);
            }

            _brain.CopyWithMutation(parent.Brain);
        }

        public void FeelAndThink(Feeling feeling)
        {
            var output = _brain.Input(feeling);
            switch (output)
            {
                case 0:
                    break;
                case 1:
                    Turn(RelativeDirection.Left);
                    break;
                case 2:
                    Turn(RelativeDirection.Right);
                    break;
                case 3:
                    break;
            }
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

        public void Die()
        {
            Dead = true;
        }

        public void ShrinkToHalfSize()
        {
            var size = _bodies.Count() / 2;
            for (var i = 0; i < size; i++)
            {
                Shrink();
            }
        }

        void Extend()
        {
            _bodies.Enqueue(NextPosition);
        }

        void Shrink()
        {
            _bodies.Dequeue();
        }

        void Turn(RelativeDirection relative)
        {
            Direction = Direction.Turn(relative);
        }
    }
}
