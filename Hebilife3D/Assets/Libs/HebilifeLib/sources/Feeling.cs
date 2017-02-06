using System;
namespace Hebilife
{
    public struct Feeling
    {
        public bool FeedInFront, ObstacleInFront, ObstacleOnRight, ObstacleOnLeft;

        public Feeling(
            bool feedInFront,
            bool obstacleInFront,
            bool obstacleOnRight,
            bool obstacleOnLeft)
        {
            FeedInFront = feedInFront;
            ObstacleInFront = obstacleInFront;
            ObstacleOnRight = obstacleOnRight;
            ObstacleOnLeft = obstacleOnLeft;
        }

        public bool[] AsArray
        {
            get
            {
                return new bool[]
                {
                    FeedInFront, ObstacleInFront, ObstacleOnRight, ObstacleOnLeft
                };
            }
        }
    }
}
