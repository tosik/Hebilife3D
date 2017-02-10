using System;

namespace Hebilife
{
    public struct Position
    {
        public long X, Y;

        public Position(long x, long y)
        {
            X = x;
            Y = y;
        }

        public static Position operator+ (Position left, Position right)
        {
            return new Position(left.X + right.X, left.Y + right.Y);
        }
    }
}
