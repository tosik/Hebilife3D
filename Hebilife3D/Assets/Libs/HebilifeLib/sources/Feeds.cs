using System.Collections.Generic;

namespace Hebilife
{
    public class Feeds
    {
        List<Position> _feeds = new List<Position>();

        public bool Exists(Position position)
        {
            return _feeds.Contains(position);
        }

        public void Put(Position position)
        {
            if (!Exists(position))
            {
                _feeds.Add(position);
            }
        }

        public void Remove(Position position)
        {
            _feeds.Remove(position);
        }
    }
}
