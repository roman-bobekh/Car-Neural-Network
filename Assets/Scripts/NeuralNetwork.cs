using System;

public class NeuralNetwork {
    int inputNum;
    int hiddenNum;
    int outputNum;
    int layerNum;

    Matrix[] weights;
    Matrix[] bias;

    public double frequency = 0.01;
    public double learningRate = 0.1;
    public string activation = "sigmoid";

    public NeuralNetwork(int inputNum_, int hiddenNum_, int outputNum_, int layerNum_) {
        layerNum = layerNum_;
        if (layerNum < 1) {
            throw new InvalidOperationException("Layer amount is less than 1.");
        }

        // Set all instance fields
        inputNum = inputNum_;
        hiddenNum = hiddenNum_;
        outputNum = outputNum_;

        // Initialize and randomize all weights
        weights = new Matrix[layerNum];

        for (int i = 0; i < layerNum; i++) {
            int start;
            int end;
            if (i == 0) {
                start = inputNum;
            } else {
                start = hiddenNum;
            }
            if (i == layerNum - 1) {
                end = outputNum;
            } else {
                end = hiddenNum;
            }

            weights[i] = new Matrix(end, start);
            weights[i].Randomize();
        }

        // Add the bias
        bias = new Matrix[layerNum];
        for (int i = 0; i < layerNum; i++) {
            if (i == layerNum - 1) {
                bias[i] = new Matrix(outputNum, 1);
            } else {
                bias[i] = new Matrix(hiddenNum, 1);
            }
            bias[i].Randomize();
        }
    }

    public double[] Guess(double[] input_arr) {
        // Feed forward the input to get an output
        Matrix[] layers = new Matrix[layerNum + 1];

        layers[0] = Matrix.FromArray(input_arr);

        // Process input into output
        for (int i = 1; i < layerNum + 1; i++) {
            layers[i] = Matrix.Multiply(weights[i - 1], layers[i - 1]);
            layers[i].Add(bias[i - 1]);
            layers[i].Map(0, 0, activation);
        }

        // return output as array
        double[] output_arr = Matrix.ToArray(layers[layerNum]);
        return output_arr;
    }

    public void Mutate() {
        // mutate matrices
        foreach (Matrix matrix in weights) {
            matrix.Mutate(frequency, learningRate);
        }

        foreach (Matrix matrix in bias)
        {
            matrix.Mutate(frequency, learningRate);
        }
    }

    public NeuralNetwork Copy() {
        // copy neural network

        NeuralNetwork result = new NeuralNetwork(inputNum, hiddenNum, outputNum, layerNum);

        result.frequency = frequency;
        result.learningRate = learningRate;
        result.activation = activation;

        result.weights = new Matrix[layerNum];
        result.bias = new Matrix[layerNum];

        for(int i = 0; i < layerNum; i++) {
            result.weights[i] = weights[i].Copy();
            result.bias[i] = bias[i].Copy();
        }

        return result;
    }
}