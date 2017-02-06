using System.Linq;

namespace Hebilife
{
    public class Brain
    {
        NeuralNetwork _nn = new NeuralNetwork();

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
    }
}
