using System.Collections.Generic;
using System.Linq;

namespace Hebilife
{
    public class Schale
    {
        public IEnumerable<Snake> Snakes { get { return _snakes; } }

        List<Snake> _snakes = new List<Snake>();
        readonly Feeds _feeds = new Feeds();
        readonly List<Position> _walls = new List<Position>();

        public void Step()
        {
            LetSnakesThinking();
            DecideSnakesDeath();
            LetSnakesMoving();
            ChangeDeadSnakesIntoFeeds();
        }

        void LetSnakesThinking()
        {
            foreach (var snake in _snakes)
            {
                Think(snake);
            }
        }

        void Think(Snake snake)
        {
            var feeling = new Feeling();
            snake.FeelAndThink(feeling);
        }

        void DecideSnakesDeath()
        {
            foreach (var snake in _snakes)
            {
                DecideDeath(snake);
            }
        }

        void DecideDeath(Snake snake)
        {
            if (ObstacleExists(snake.NextPosition) ||
                CollidedWithOtherNextPositions(snake.NextPosition))
            {
                snake.Die();
            }
        }

        void LetSnakesMoving()
        {
            foreach (var snake in _snakes.Where(x => !x.Dead))
            {
                Move(snake);
            }
        }

        void Move(Snake snake)
        {
            if (_feeds.Exists(snake.NextPosition))
            {
                _feeds.Remove(snake.NextPosition);
                snake.MoveAndEat();
            }
            else
            {
                snake.Move();
            }
        }

        bool ObstacleExists(Position position)
        {
            var wall = _walls.Contains(position);
            var snake = _snakes.Any(s => s.Bodies.Contains(position));

            return wall || snake;
        }

        bool CollidedWithOtherNextPositions(Position position)
        {
            return _snakes.Select(x => x.NextPosition).Contains(position);
        }

        void ChangeDeadSnakesIntoFeeds()
        {
            var newSnakes = _snakes.Where(x => !x.Dead).ToList();

            foreach (var snake in _snakes.Where(x => x.Dead))
            {
                foreach (var body in snake.Bodies)
                {
                    _feeds.Put(body);
                }
            }

            _snakes = newSnakes;
        }
    }
}
