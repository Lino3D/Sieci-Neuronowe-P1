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
using System.Linq;
using System.Collections.Generic;

namespace SNP1
{
    public class SimpleNeuralNetwork : INeuralNetwork
    {
        private IMLDataSet trainingSet;
        private IMLDataSet testSet;
        private BasicNetwork network;
        private IActivationFunction activationFunction = null;
        private double learningRate = 0.00001;
        private double theMomentum = 0.0005;
        private bool hasBias = false;
        private int outputSize;

        public List<IterationError> learningProcess;

        public IMLDataSet TestSet
        {
            get
            {
                return testSet;
            }

            set
            {
                testSet = value;
            }
        }
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

        public int OutputSize
        {
            get
            {
                return outputSize;
            }

            set
            {
                outputSize = value;
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

        public void InitializeTrainingSet(List<DataPointCls> points, int MaxClass)
        {
            double[][] input = new double[points.Count][];
            for (int i = 0; i < points.Count; i++)
            {
                input[i] = new[] { points[i].X, points[i].Y };
            }

            double[][] idealOutput = new double[points.Count][];
            for (int i = 0; i < points.Count; i++)
            {
                idealOutput[i] = GetExpectedOutput(points[i].Cls, MaxClass);
            }

            this.TrainingSet = new BasicMLDataSet(input, idealOutput);
        }

        private double[] GetExpectedOutput(int cls, int MaxClass)
        {
            double[] tab = new double[MaxClass];
            for (int i = 1; i < MaxClass + 1; i++)
            {
                if (i == cls)
                {
                    tab[i-1] = 1;
                }
                else
                    tab[i-1] = -1;
            }

            return tab;
        }
        public void AddLayer(int neuronCount)
        {
            this.Network.AddLayer(new BasicLayer(this.ActivationFunction, this.HasBias, neuronCount));
        }

        public void AddLayerBunch(int layerCount, int neuronCount)
        {
            for (int i = 0; i < layerCount; i++)
            {
                this.Network.AddLayer(new BasicLayer(this.ActivationFunction, this.HasBias, neuronCount));
            }
        }

        public void StartLearning(int iterationCount)
        {
            this.learningProcess = new List<IterationError>();

            this.Network.Structure.FinalizeStructure();
            Network.Reset();
            IOutput writer = MyCore.Resolve<IOutput>();

            Propagation train = new Backpropagation(this.Network, this.TrainingSet, this.LearningRate, this.TheMomentum);
            // train.BatchSize = 1;

            int epoch = 1;
            do
            {
                train.Iteration();
                writer.Write(String.Format(@"Epoch # {0} Error: {1}", epoch, train.Error));
                epoch++;
                this.learningProcess.Add(new IterationError(epoch, train.Error));
                
            } while (epoch < iterationCount);

        }

        



        public IEnumerable<IResult> ComputeTrainingSet()
        {
            if (TrainingSet == null)
                yield break;

            IOutput writer = MyCore.Resolve<IOutput>();

            foreach (IMLDataPair pair in TrainingSet)
            {
                IMLData output = Network.Compute(pair.Input);
                writer.Write(String.Format(@"{0},{1}, actual={2},ideal={3}", pair.Input[0], pair.Input[1], GetOutput(output), GetClass(pair.Ideal)));
                yield return new Result() { Input = pair, Output = output };
            }
        }
        private string GetOutput(IMLData lst)
        {
            string ret = " [Ouput: ";
            for (int i = 0; i < lst.Count; i++)
                ret += lst[i] + ",";
            ret = ret.TrimEnd(',');
            ret += "]";
            return ret;

        }

        // TODO : to nie powinno tak wygladac, szczegolnie przy porownywaniu actual punktu...
        private int GetClass(IMLData lst)
        {
            int i = 0;
            while (true)
            {
                if (lst[i] == 1)
                    return i + 1;
                i++;
            }

        }


    }
}