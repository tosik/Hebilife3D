﻿using System;

namespace Hebilife
{
    public class NeuralNetwork
    {
        public const int NumOfLayers = 3;
        public const int NumOfNeuron = 6;

        public int[,] _neurons = new int[NumOfLayers, NumOfNeuron];
        public double[,,] _weights = new double[NumOfLayers - 1, NumOfNeuron, NumOfNeuron];

        Random _random = new Random();

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

        public void Copy(NeuralNetwork origin)
        {
            for (int layer = 0; layer < NumOfLayers; layer++)
            {
                for (int x = 0; x < NumOfNeuron; x++)
                {
                    _neurons[layer, x] = origin.GetNeuron(layer, x);
                }
            }
            for (int layer = 0; layer < NumOfLayers - 1; layer++)
            {
                for (int x = 0; x < NumOfNeuron; x++)
                {
                    for (int y = 0; y < NumOfNeuron; y++)
                    {
                        _weights[layer, x, y] = origin.GetWeight(layer, x, y);
                    }
                }
            }
        }

        public int GetNeuron(int layer, int x)
        {
            return _neurons[layer, x];
        }

        public double GetWeight(int layer, int x, int y)
        {
            return _weights[layer, x, y];
        }

        public void Mutate()
        {
            for (int i = 0; i < 100; i++)
            {
                _weights[
                    Rand() % (NumOfLayers - 1),
                    Rand() % NumOfNeuron,
                    Rand() % NumOfNeuron] += 0.1 * (Rand() % 2 == 0 ? -1 : 1);
            }
        }

        public void Randomize()
        {
            for (int layer = 0; layer < NumOfLayers - 1; layer++)
            {
                for (int x = 0; x < NumOfNeuron; x++)
                {
                    for (int y = 0; y < NumOfNeuron; y++)
                    {
                        _weights[layer, x, y] = ((double)Rand()) / 1024.0;
                    }
                }
            }
        }

        void WriteFirstLayer(int[] input)
        {
            for (int n = 0; n < NumOfNeuron; n++)
            {
                _neurons[0, n] = input[n];
            }
        }

        int Rand()
        {
            return _random.Next(1024);
        }

        public long Gene
        {
            get
            {
                long dest = 0;
                var bit = 0;
                for (var layer = 0; layer < NumOfLayers - 1; layer++)
                {
                    for (var y = 0; y < NumOfNeuron; y++)
                    {
                        for (var x = 0; x < NumOfNeuron; x++)
                        {
                            if (bit < 64)
                            {
                                var boolean = _weights[layer, y, x] > 0.5 ? 1 : 0;
                                dest += boolean << bit;
                                bit++;
                            }
                        }
                    }
                }

                return dest;
            }
        }
   }
}