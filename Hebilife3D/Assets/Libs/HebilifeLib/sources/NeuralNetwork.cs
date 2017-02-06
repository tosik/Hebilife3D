namespace Hebilife
{
    public class NeuralNetwork
    {
        public const int NumOfLayers = 3;
        public const int NumOfNeuron = 3;

        public int[,] _neurons = new int[NumOfLayers, NumOfNeuron];
        public double[,,] _weights = new double[NumOfLayers, NumOfNeuron, NumOfNeuron];

        public int[] Step(int[] input)
        {
            WriteFirstLayer(input);

            for (int layer = 1; layer < NumOfLayers; layer++)
            {
                for (int y = 0; y < NumOfNeuron; y++)
                {
                    int t = 0;
                    for (int x = 0; x < NumOfNeuron; x++)
                    {
                        t += (int)(_weights[layer - 1, y, x] * _neurons[layer - 1, x]);
                        if (t - 0.5 >= 0)
                            _neurons[layer, y] = 1;
                        else
                            _neurons[layer, y] = 0;
                    }
                }
            }

            var dest = new int[NumOfNeuron];
            for (var i = 0; i < NumOfNeuron; i++)
            {
                dest[i] = _neurons[NumOfLayers - 1, i];
            }
            return dest;
        }

        void WriteFirstLayer(int[] input)
        {
            for (int n = 0; n < NumOfNeuron; n++)
            {
                _neurons[0, n] = input[n];
            }
        }
   }
}