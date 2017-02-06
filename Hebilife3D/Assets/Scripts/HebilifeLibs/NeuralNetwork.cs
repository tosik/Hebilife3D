public class NN
{
    public const int N_NEURON_LAYER = 3;
    public const int N_NEURON = 3;

    /** ニューロンの状態を記録 */
    public int[,] neurons = new int[N_NEURON_LAYER, N_NEURON];

    /** ニューロン同士の結合荷重 */
    public double[,,] weight = new double[N_NEURON_LAYER, N_NEURON, N_NEURON];

    public int[] step(int[] input)
    {
        for (int n = 0; n < N_NEURON; n++)
        {
            neurons[0, n] = input[n];
        }

        for (int layer = 1; layer < N_NEURON_LAYER; layer++)
        {
            for (int y = 0; y < N_NEURON; y++)
            {
                int t = 0;
                for (int x = 0; x < N_NEURON; x++)
                {
                    t += (int)(weight[layer - 1, y, x] * neurons[layer - 1, x]);
                    if (t - 0.5 >= 0)
                        neurons[layer, y] = 1;
                    else
                        neurons[layer, y] = 0;
                }
            }
        }

        var dest = new int[N_NEURON];
        for (var i = 0; i < N_NEURON; i++)
        {
            dest[i] = neurons[N_NEURON_LAYER - 1, i];
        }
        return dest;
    }
}
