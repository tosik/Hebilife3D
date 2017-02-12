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
        Dictionary<Position, bool> _nextObstacleMap;

        public event Action<Snake> SnakeGenerated;
        public event Action<Position> FeedGenerated;
        public event Action<Position> FeedRemoved;

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
                if (SnakeGenerated != null)
                {
                    SnakeGenerated(snake);
                }
            }
        }

        public void GenerateFeeds(int num, long sizeX, long sizeY)
        {
            for (var i = 0; i < num; i++)
            {
                var feed = new Position(Rand() % (sizeX - 2) + 1, Rand() % (sizeY - 2) + 1);
                _feeds.Put(feed);

                if (FeedGenerated != null)
                {
                    FeedGenerated(feed);
                }
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

        public void CreateRooms(int numberOfRooms, long sizeX, long sizeY)
        {
            for (var i = 0; i < numberOfRooms; i++)
            {
                var direction = RandomDirection();
                var pos = new Position(Rand() % (sizeX - 10), Rand() % (sizeY - 10));
                for (int x = 0; x < 10; x++)
                {
                    if (direction != Direction.North)
                        _walls.Add(pos + new Position(x, 0));
                    if (direction != Direction.South)
                        _walls.Add(pos + new Position(x, 10));
                    if (direction != Direction.East)
                        _walls.Add(pos + new Position(10, x));
                    if (direction != Direction.West)
                        _walls.Add(pos + new Position(0, x));
                }
            }
        }

        public void Step()
        {
            UpdateNextObstacleMap();
            LetSnakesThinking();
            DecideSnakesDeath();
            LetSnakesMoving();
            ChangeDeadSnakesIntoFeeds();
            DivideMatureSnakes();
        }

        // caution: without a head
        void UpdateNextObstacleMap()
        {
            _nextObstacleMap = new Dictionary<Position, bool>();

            foreach (var snake in Snakes)
            {
                foreach (var body in snake.BodiesWithoutTerminal)
                {
                    _nextObstacleMap[body] = true;
                }
            }
            foreach (var wall in Walls)
            {
                _nextObstacleMap[wall] = true;
            }
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
            feeling.FeedOnLeft = _feeds.Exists(snake.Head + snake.Direction.Turn(RelativeDirection.Left).AsPosition());
            feeling.FeedOnRight = _feeds.Exists(snake.Head + snake.Direction.Turn(RelativeDirection.Right).AsPosition());
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

                if (FeedRemoved != null)
                {
                    FeedRemoved(snake.NextPosition);
                }

                snake.MoveAndEat();
            }
            else
            {
                snake.Move();
            }
        }

        bool ObstacleExistsInNextTiming(Position position)
        {
            bool value;
            if (_nextObstacleMap.TryGetValue(position, out value))
            {
                return value;
            }
            else
            {
                return false;
            }
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

                    if (FeedGenerated != null)
                    {
                        FeedGenerated(body);
                    }
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
            if (SnakeGenerated != null)
            {
                SnakeGenerated(newSnake);
            }
        }

        Direction RandomDirection()
        {
            var key = Rand() % 4;
            return new Direction[] { Direction.North, Direction.East, Direction.South, Direction.West }[key];
        }
    }
}
