using System;
using System.Linq;

public class NeuroEvolution {
    public NeuralNetwork[] brains;
    int population;
    int generation = 1;
    int batchSize;
    double[][] batch;
    int batchTime = 0; 

    public NeuroEvolution(int inputs, int hiddens, int outputs, int layers, int population_, int batchSize_, double learningRate, double frequency, String activation) {
        population = population_;
        batchSize = batchSize_;
        batch = new double[batchSize][];

        brains = new NeuralNetwork[population];
        for (int i = 0; i < population; i++) {
            brains[i] = new NeuralNetwork(inputs, hiddens, outputs, layers);
            brains[i].learningRate = learningRate;
            brains[i].frequency = frequency;
            brains[i].activation = activation;
        }
    }

    public void Evolve(double[] scores) {
        batch[batchTime] = scores;
        batchTime++;
        
        if (batchTime % batchSize == 0) {   
            batchTime = 0;
            
            // calculate average for each nn
            double[] average = new double[population];
            for (int i = 0; i < population; i++) {
                double batchSum = 0;
                for (int j = 0; j < batchSize; j++) {
                    batchSum += batch[j][i]; 
                }
                average[i] = batchSum / batchSize;
            }
            
            // pass the best with a high probability into the next generation and mutate them
            NeuralNetwork[] new_brains = new NeuralNetwork[population];
            double sum = average.Sum();
            double subtotal;
    
            for (int i = 0; i < population - 1; i++) {
                subtotal = new Random().NextDouble() * sum;
    
                for (int j = 0; j < population; j++) {
                    subtotal -= average[j];
                    if (subtotal < 0) {
                        new_brains[i] = brains[j].Copy();
                        new_brains[i].Mutate();
                        break;
                    }
                }
            }
    
            // copy best nn
            int highest = 0;
            for (int i = 0; i < population; i++) {
                if (average[i] > average[highest]) {
                    highest = i;
                }
            }
    
            new_brains[population - 1] = brains[highest].Copy();
            
            // print highest average
            Console.WriteLine(average[highest]);
    
            brains = new_brains;
            generation++;
        }  
    }
}