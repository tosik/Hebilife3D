using System.Linq;

namespace Hebilife
{
    public class Brain
    {
        public NeuralNetwork NeuralNetwork { get { return _nn; } }

        NeuralNetwork _nn = new NeuralNetwork();

        int _beat = 0;

        public Brain()
        {
            _nn.Randomize();
        }

        public int Input(Feeling feeling)
        {
            _beat++;
            if (_beat >= 30)
            {
                feeling.FeedOnLeft = true;
                feeling.FeedOnRight = true;
                feeling.FeedInFront = true;
                feeling.ObstacleOnLeft = true;
                feeling.ObstacleOnRight = true;
                feeling.ObstacleInFront = true;
            }

            var outputAsArray = _nn.Step(feeling.AsArray.Select(x => x ? 1 : 0).ToArray());

            int output = 0;
            for (var i = 0; i < outputAsArray.Length; i ++)
            {
                output += outputAsArray[i] * (i + 1);
            }

            return output;
        }

        public void CopyWithMutation(Brain origin)
        {
            _nn.Copy(origin.NeuralNetwork);
            _nn.Mutate();
        }
    }
}
