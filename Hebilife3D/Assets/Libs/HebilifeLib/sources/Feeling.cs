using System;
namespace Hebilife
{
    public struct Feeling
    {
        public bool FeedInFront, FeedOnRight, FeedOnLeft;
        public bool ObstacleInFront, ObstacleOnRight, ObstacleOnLeft;

        public Feeling(
            bool feedInFront,
            bool feedOnLeft,
            bool feedOnRight,
            bool obstacleInFront,
            bool obstacleOnRight,
            bool obstacleOnLeft)
        {
            FeedInFront = feedInFront;
            FeedOnRight = feedOnLeft;
            FeedOnLeft = feedOnRight;
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
                    FeedInFront, FeedOnLeft, FeedOnRight,
                    ObstacleInFront, ObstacleOnRight, ObstacleOnLeft,
                };
            }
        }
    }
}
