using Encog.Engine.Network.Activation;
using Encog.ML.Data;
using Encog.ML.Data.Basic;
using Encog.ML.Train;
using Encog.Neural.Networks;
using Encog.Neural.Networks.Layers;
using Encog.Neural.Networks.Training.Propagation;
using Encog.Neural.Networks.Training.Propagation.Back;
using Encog.Neural.Networks.Training.Propagation.Resilient;
using SNP1.DataHelper;
using SNP1.Models;
using SNP1.Models.Interfaces;
using System;
using System.Collections.Generic;

namespace SNP1
{
    public class SimpleNeuralNetwork : INeuralNetwork
    {
        private IMLDataSet trainingSet;
        private BasicNetwork network;
        private IActivationFunction activationFunction = null;
        private double learningRate = 0.00001;
        private double theMomentum = 0.0005;
        private bool hasBias = true;

        public IMLDataSet TrainingSet
        {
            get
            {
                return trainingSet;
            }

            set
            {
                trainingSet = value;
            }
        }
        public BasicNetwork Network
        {
            get
            {
                return network;
            }

            set
            {
                network = value;
            }
        }
        public double LearningRate
        {
            get
            {
                return learningRate;
            }

            set
            {
                learningRate = value;
            }
        }
        public double TheMomentum
        {
            get
            {
                return theMomentum;
            }

            set
            {
                theMomentum = value;
            }
        }
        public bool HasBias
        {
            get
            {
                return hasBias;
            }

            set
            {
                hasBias = value;
            }
        }
        public IActivationFunction ActivationFunction
        {
            get
            {
                return activationFunction;
            }

            set
            {
                activationFunction = value;
            }
        }


        public SimpleNeuralNetwork()
        { Network = new BasicNetwork(); }

        public SimpleNeuralNetwork(double learnRate, double momentum, bool hasBias)
        {
            this.LearningRate = learnRate;
            this.TheMomentum = momentum;
            this.HasBias = hasBias;

            Network = new BasicNetwork();
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
            this.Network.AddLayer(new BasicLayer(this.ActivationFunction, this.HasBias, neuronCount));
        }

        public void AddLayerBunch(int layerCount, int neuronCount)
        {
            for(int i=0; i<layerCount; i++)
            {
                this.Network.AddLayer(new BasicLayer(this.ActivationFunction, this.HasBias, neuronCount));
            }
        }

        public void StartLearning(int iterationCount)
        {
            this.Network.Structure.FinalizeStructure();
            Network.Reset();

            Propagation train = new Backpropagation(this.Network, this.TrainingSet, this.LearningRate, this.TheMomentum);
           // train.BatchSize = 1;

            int epoch = 1;
            do
            {
                train.Iteration();
                MyCore.Resolve<IOutput>().Write(@"Epoch #" + epoch + @" Error:" + train.Error);
                epoch++;
            } while (epoch<iterationCount);

     
            foreach (IMLDataPair pair in TrainingSet)
            {
                IMLData output = Network.Compute(pair.Input);
                MyCore.Resolve<IOutput>().Write(pair.Input[0] + @"," + pair.Input[1]
                                  + @", actual=" + output[0] + @",ideal=" + pair.Ideal[0]);
            }
        }
    }
}