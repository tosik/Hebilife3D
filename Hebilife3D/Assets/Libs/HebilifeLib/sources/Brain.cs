using System.Linq;

namespace Hebilife
{
    public class Brain
    {
        public NeuralNetwork NeuralNetwork { get { return _nn; } }

        NeuralNetwork _nn = new NeuralNetwork();

        public Brain()
        {
            for (var i = 0; i < 3000; i++)
            {
                _nn.Mutate();
            }
        }

        public int Input(Feeling feeling)
        {
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
