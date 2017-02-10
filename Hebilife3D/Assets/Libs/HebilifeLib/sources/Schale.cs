using System;
using System.Collections.Generic;
using System.Linq;

namespace Hebilife
{
    public class Schale
    {
        public IEnumerable<Snake> Snakes { get { return _snakes; } }
        public Feeds Feeds { get { return _feeds; } }
        public IEnumerable<Position> Walls { get { return _walls; } }

        List<Snake> _snakes = new List<Snake>();
        readonly Feeds _feeds = new Feeds();
        readonly List<Position> _walls = new List<Position>();
        Random _random = new Random();

        int Rand()
        {
            return _random.Next(1024);
        }

        public void GenerateSnakes(int num, long sizeX, long sizeY)
        {
            for (var i = 0; i < num; i++)
            {
                var snake = new Snake(new Position(Rand() % (sizeX - 2) + 1, Rand() % (sizeY - 2) + 1), RandomDirection());
                _snakes.Add(snake);
            }
        }

        public void GenerateFeeds(int num, long sizeX, long sizeY)
        {
            for (var i = 0; i < num; i++)
            {
                _feeds.Put(new Position(Rand() % (sizeX - 2) + 1, Rand() % (sizeY - 2) + 1));
            }
        }

        public void CreateFrame(long sizeX, long sizeY)
        {
            for (var i = 0; i < sizeX; i++)
            {
                _walls.Add(new Position(i, 0));
                _walls.Add(new Position(i, sizeY - 1));
            }
            for (var i = 0; i < sizeY; i++)
            {
                _walls.Add(new Position(0, i));
                _walls.Add(new Position(sizeX - 1, i));
            }
        }

        public void Step()
        {
            LetSnakesThinking();
            DecideSnakesDeath();
            LetSnakesMoving();
            ChangeDeadSnakesIntoFeeds();
            DivideMatureSnakes();
        }

        void LetSnakesThinking()
        {
            foreach (var snake in _snakes)
            {
                Think(snake);
            }
        }

        bool IsObstacle(Position position, Snake ignoringSnake)
        {
            return ObstacleExistsInNextTiming(position) ||
                CollidedWithOtherNextPositions(position, ignoringSnake);
        }

        void Think(Snake snake)
        {
            var feeling = new Feeling();
            feeling.FeedInFront = _feeds.Exists(snake.NextPosition);
            feeling.ObstacleInFront = IsObstacle(snake.NextPosition, snake);
            feeling.ObstacleOnLeft = IsObstacle(snake.Head + snake.Direction.Turn(RelativeDirection.Left).AsPosition(), snake);
            feeling.ObstacleOnRight = IsObstacle(snake.Head + snake.Direction.Turn(RelativeDirection.Right).AsPosition(), snake);

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
            if (IsObstacle(snake.NextPosition, snake))
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

        bool ObstacleExistsInNextTiming(Position position)
        {
            var wall = _walls.Contains(position);
            var snake = _snakes.Any(s => s.BodiesWithoutTerminal.Contains(position));

            return wall || snake;
        }

        bool CollidedWithOtherNextPositions(Position position, Snake me)
        {
            return _snakes.Where(x => x != me).Select(x => x.NextPosition).Contains(position);
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

        void DivideMatureSnakes()
        {
            foreach (var snake in _snakes.Where(x => x.Mature).ToArray())
            {
                Divide(snake);
            }
        }

        void Divide(Snake snake)
        {
            var bodies = snake.Bodies.ToArray();
            snake.ShrinkToHalfSize();
            var newBodies = bodies.Reverse().Skip((int)Math.Ceiling(bodies.Count() / 2.0)).Reverse();

            var newSnake = new Snake(snake, newBodies);
            _snakes.Add(newSnake);
        }

        Direction RandomDirection()
        {
            var key = Rand() % 4;
            return new Direction[] { Direction.North, Direction.East, Direction.South, Direction.West }[key];
        }
    }
}
