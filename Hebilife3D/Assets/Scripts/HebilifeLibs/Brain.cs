using System;

public class Brain
{
    public Brain()
    {
        for (int y = 0; y < NN.N_NEURON_LAYER; y++)
            for (int x = 0; x < NN.N_NEURON; x++)
                nn.neurons[y, x] = 0;
        for (int z = 0; z < NN.N_NEURON; z++)
            for (int y = 0; y < NN.N_NEURON; y++)
                for (int x = 0; x < NN.N_NEURON_LAYER; x++)
                {
                    nn.weight[x, y, z] = 0.5;
                    for (int i = 0; i < 5; i++)
                        nn.weight[x, y, z] += rand() % 2 == 1 ? 0.1 : -0.1;
                    if (nn.weight[x, y, z] < 0)
                        nn.weight[x, y, z] = 0;
                    if (nn.weight[x, y, z] > 1)
                        nn.weight[x, y, z] = 1;
                }
    }

	NN nn = new NN();
    Random _random = new Random();

    int rand()
    {
        return _random.Next(1024);
    }

    public int input_and_output(int[] input)
    {
        int[] output = nn.step(input);
        return output[0] * 2 + output[1];
    }

    public void copy(Brain b)
    {
        for (int y = 0; y < NN.N_NEURON_LAYER; y++)
            for (int x = 0; x < NN.N_NEURON; x++)
                nn.neurons[y, x] = b.nn.neurons[y, x];
        for (int z = 0; z < NN.N_NEURON; z++)
            for (int y = 0; y < NN.N_NEURON; y++)
                for (int x = 0; x < NN.N_NEURON_LAYER; x++)
                    nn.weight[x, y, z] = b.nn.weight[x, y, z];

        for (int i = 0; i < 100; i++)
            nn.weight[
                rand() % NN.N_NEURON_LAYER,
                rand() % NN.N_NEURON,
                rand() % NN.N_NEURON] += 0.1 * (rand() % 2 == 0 ? -1 : 1);
    }

}
