using System;
namespace Hebilife
{
    public class View
    {
        public enum Cell
        {
            None, Snake, Feed,
        }

        public long SizeX { get; private set; }
        public long SizeY { get; private set; }

        Cell[,] _values;

        public View(long sizeX, long sizeY)
        {
            SizeX = sizeX;
            SizeY = sizeY;

            _values = new Cell[sizeX * 10, sizeY * 10];
        }

        public void Reflect(Schale schale)
        {
            for (int y = 0; y < SizeY; y++)
            {
                for (int x = 0; x < SizeX; x++)
                {
                    if (schale.Feeds.Exists(new Position(x, y)))
                        TrySet(x, y, Cell.Feed);
                    else
                        TrySet(x, y, Cell.None);
                }
            }

            foreach (var snake in schale.Snakes)
            {
                foreach (var body in snake.Bodies)
                {
                    TrySet(body.X, body.Y, Cell.Snake);
                }

            }
        }

        public Cell Get(long x, long y)
        {
            return _values[x + SizeX / 2 + 1, y + SizeY / 2 + 1];
        }

        void TrySet(long x, long y, Cell cell)
        {
            try
            {
                _values[x + SizeX / 2 + 1, y + SizeY / 2 + 1] = cell;
            }
            catch(IndexOutOfRangeException)
            {
            }
        }
    }
}
