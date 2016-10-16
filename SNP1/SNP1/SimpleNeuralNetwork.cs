using Encog.Engine.Network.Activation;
using Encog.ML.Data;
using Encog.ML.Data.Basic;
using Encog.ML.Train;
using Encog.Neural.Networks;
using Encog.Neural.Networks.Layers;
using Encog.Neural.Networks.Training.Propagation;
using Encog.Neural.Networks.Training.Propagation.Back;
using Encog.Neural.Networks.Training.Propagation.Resilient;
using SNP1.Models;
using System;
using System.Collections.Generic;

namespace SNP1
{
    public class SimpleNeuralNetwork
    {
        public IMLDataSet TrainingSet { get; set; }

        private BasicNetwork Network { get; set; }

        private IActivationFunction activationFunction { get; set; } = null;

        private double LearningRate { get; set; } = 0.00001;
        private double theMomentum { get; set; } = 0.0005;

        private bool HasBias { get; set; } = true;

        public SimpleNeuralNetwork()
        { Network = new BasicNetwork(); }

        public SimpleNeuralNetwork(double learnRate, double momentum, bool hasBias)
        {
            this.LearningRate = learnRate;
            this.theMomentum = momentum;
            this.HasBias = hasBias;

            Network = new BasicNetwork();
        }

        public void SetLearningRate(double learnRate)

        { this.LearningRate = learnRate; }

        public void SetMomentum(double momentum)
        {
            this.theMomentum = momentum;
        }

        public void SetActivationFunction(IActivationFunction activationFunction)
        {
            this.activationFunction = activationFunction;
        }

        public void ResetNetwork()
        {
            this.Network.Reset();
        }

        public void InitializeTrainingSet(List<DataPointCls> points)
        {
            double[][] input = new double[points.Count][];
            for (int i = 0; i < points.Count; i++)
            {
                input[i] = new[] { points[i].X, points[i].Y };
            }

            double[][] idealOutput = new double[points.Count][];
            for (int i = 0; i < points.Count; i++)
            {
                idealOutput[i] = new[] { (double)points[i].Cls };
            }

            this.TrainingSet = new BasicMLDataSet(input, idealOutput);
        }

        public void AddLayer(int neuronCount)
        {
            this.Network.AddLayer(new BasicLayer(this.activationFunction, this.HasBias, neuronCount));
        }

        public void AddLayerBunch(int layerCount, int neuronCount)
        {
            for(int i=0; i<layerCount; i++)
            {
                this.Network.AddLayer(new BasicLayer(this.activationFunction, this.HasBias, neuronCount));
            }
        }

        public void StartLearning(int iterationCount)
        {
            this.Network.Structure.FinalizeStructure();
            Network.Reset();

            Propagation train = new Backpropagation(this.Network, this.TrainingSet, this.LearningRate, this.theMomentum);
           // train.BatchSize = 1;

            int epoch = 1;
            do
            {
                train.Iteration();
                Console.WriteLine(@"Epoch #" + epoch + @" Error:" + train.Error);
                epoch++;
            } while (epoch<iterationCount);

     
            foreach (IMLDataPair pair in TrainingSet)
            {
                IMLData output = Network.Compute(pair.Input);
                Console.WriteLine(pair.Input[0] + @"," + pair.Input[1]
                                  + @", actual=" + output[0] + @",ideal=" + pair.Ideal[0]);
            }
        }
    }
}