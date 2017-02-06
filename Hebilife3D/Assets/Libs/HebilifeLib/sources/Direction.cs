using System;
using System.Collections.Generic;

namespace Hebilife
{
    using Pair = Tuple<Direction, RelativeDirection>;
    using TableDict = Dictionary<Tuple<Direction, RelativeDirection>, Direction>;

    public enum Direction
    {
        North, East, South, West
    }

    public static class DirectionExtensions
    {
        public static Position AsPosition(this Direction direction)
        {
            switch (direction)
            {
                case Direction.North:
                    return new Position(0, -1);
                case Direction.East:
                    return new Position(1, 0);
                case Direction.South:
                    return new Position(0, 1);
                case Direction.West:
                    return new Position(-1, 0);

                default:
                    throw new InvalidOperationException();
            }
        }

        static TableDict _table = new TableDict()
        {
            { new Pair(Direction.North, RelativeDirection.Straight), Direction.North },
            { new Pair(Direction.North, RelativeDirection.Left), Direction.West },
            { new Pair(Direction.North, RelativeDirection.Back), Direction.South },
            { new Pair(Direction.North, RelativeDirection.Right), Direction.East },
            { new Pair(Direction.East, RelativeDirection.Straight), Direction.East },
            { new Pair(Direction.East, RelativeDirection.Left), Direction.North },
            { new Pair(Direction.East, RelativeDirection.Back), Direction.West },
            { new Pair(Direction.East, RelativeDirection.Right), Direction.South },
            { new Pair(Direction.South, RelativeDirection.Straight), Direction.South },
            { new Pair(Direction.South, RelativeDirection.Left), Direction.East },
            { new Pair(Direction.South, RelativeDirection.Back), Direction.North },
            { new Pair(Direction.South, RelativeDirection.Right), Direction.West },
            { new Pair(Direction.West, RelativeDirection.Straight), Direction.West },
            { new Pair(Direction.West, RelativeDirection.Left), Direction.South },
            { new Pair(Direction.West, RelativeDirection.Back), Direction.East },
            { new Pair(Direction.West, RelativeDirection.Right), Direction.North },
        };
        public static Direction Turn(this Direction direction, RelativeDirection relative)
        {
            return _table[new Pair(direction, relative)];
        }
    }
}
