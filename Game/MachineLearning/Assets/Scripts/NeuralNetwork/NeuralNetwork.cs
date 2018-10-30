using System.Collections.Generic;
using System;
using System.Linq;

/// <summary>
/// Neural Network C# (Unsupervised)
/// </summary>
public class NeuralNetwork : IComparable<NeuralNetwork>
{
	private int[] layers;
	private float[][] neurons;
	private float[][][] weights;
	public float fitness;

	/// <summary>
	/// Neural network constructor
	/// </summary>
	/// <param name="layers"></param>
	public NeuralNetwork(int[] layers)
	{
		this.layers = new int[layers.Length];
		for (int i = 0; i < layers.Length; i++)
		{
			this.layers[i] = layers[i];
		}

		InitNeurons();
		InitWeights();
	}

	/// <summary>
	/// Deep copy constructor
	/// </summary>
	/// <param name="copyNetwork"></param>
	public NeuralNetwork(NeuralNetwork copyNetwork)
	{
		this.layers = new int[copyNetwork.layers.Length];
		for (int i = 0; i < copyNetwork.layers.Length; i++)
		{
			this.layers[i] = copyNetwork.layers[i];
		}

		InitNeurons();
		InitWeights();
		CopyWeights(copyNetwork.weights);
	}

	/// <summary>
	/// Copies weigths
	/// </summary>
	/// <param name="copyWeights"></param>
	private void CopyWeights(float[][][] copyWeights)
	{
		for (int i = 0; i < weights.Length; i++)
		{
			for (int j = 0; j < weights[i].Length; j++)
			{
				for (int k = 0; k < weights[i][j].Length; k++)
				{
					weights[i][j][k] = copyWeights[i][j][k];
				}
			}
		}
	}

	/// <summary>
	/// Creates neuron matrix
	/// </summary>
	private void InitNeurons()
	{
		/*List<float[]> neuronList = new List<float[]>();

		for (int i = 0; i < layers.Length; i++)
		{
			neuronList.Add(new float[layers[i]]);
		}

		neurons = neuronList.ToArray();*/

		neurons = layers.Select(I => new float[I]).ToArray();
	}

	/// <summary>
	/// Creates weights matrix
	/// </summary>
	private void InitWeights()
	{
		List<float[][]> weightList = new List<float[][]>();

		for (int i = 1; i < layers.Length; i++)
		{
			List<float[]> layerWeightList = new List<float[]>();

			int neuronsInPreviousLayer = layers[i - 1];

			for (int j = 0; j < neurons[i].Length; j++)
			{
				float[] neuronWeights = new float[neuronsInPreviousLayer];

				for (int k = 0; k < neuronsInPreviousLayer; k++)
				{
					neuronWeights[k] = UnityEngine.Random.Range(-0.5f, 0.5f);
				}

				layerWeightList.Add(neuronWeights);
			}

			weightList.Add(layerWeightList.ToArray());
		}

		weights = weightList.ToArray();
	}

	/// <summary>
	/// Feedforwards this neural network with given input
	/// </summary>
	/// <param name="inputs"></param>
	/// <returns></returns>
	public float[] FeedForward(float[] inputs)
	{
		// Adds the input to the first neuron layer
		for (int i = 0; i < inputs.Length; i++)
		{
			neurons[0][i] = inputs[i];
		}

		// Itterates over all neurons and compute feedforward values
		for (int i = 1; i < layers.Length; i++)
		{
			for (int j = 0; j < neurons[i].Length; j++)
			{
				float value = 0.25f;

				for (int k = 0; k < neurons[i - 1].Length; k++)
				{
					value += weights[i - 1][j][k] * neurons[i - 1][k]; // Sum off all weights connections of this neuron with their values in previous layer
				}

				neurons[i][j] = (float)Math.Tanh(value);
			}
		}

		return neurons[neurons.Length - 1];
	}

	/// <summary>
	/// Mutates weights
	/// </summary>
	public void Mutate()
	{
		for (int i = 0; i < weights.Length; i++)
		{
			for (int j = 0; j < weights[i].Length; j++)
			{
				for (int k = 0; k < weights[i][j].Length; k++)
				{
					float weight = weights[i][j][k];

					float randomNumber = UnityEngine.Random.Range(0, 1000f);

					if (randomNumber <= 2f)
					{
						weight *= -1;
					}
					else if (randomNumber <= 4f)
					{
						weight = UnityEngine.Random.Range(-0.5f, 0.5f);
					}
					else if (randomNumber <= 6f)
					{
						float factor = UnityEngine.Random.Range(0f, 1f) + 1f;
						weight *= factor;
					}
					else if (randomNumber <= 8f)
					{
						float factor = UnityEngine.Random.Range(0f, 1f);
						weight *= factor;
					}

					weights[i][j][k] = weight;
				}
			}
		}
	}

	public void AddFitness(float fit)
	{
		fitness += fit;
	}

	public void SetFitness(float fit)
	{
		fitness = fit;
	}

	public float GetFitness()
	{
		return fitness;
	}

	/// <summary>
	/// Compare two neural networks and sort based on fitness
	/// </summary>
	/// <param name="other">Network to be compared to</param>
	/// <returns></returns>
	public int CompareTo(NeuralNetwork other)
	{
		if (other == null) return 1;

		if (fitness > other.fitness)
			return 1;
		else if (fitness < other.fitness)
			return -1;
		else
			return 0;
	}
}
