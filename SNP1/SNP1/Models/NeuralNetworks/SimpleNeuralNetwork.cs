﻿using Encog.Engine.Network.Activation;
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
        private bool Regression = false;


        public ResultsList resultList;


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

        public BasicMLDataSet InitializeClassificationSet(List<DataPointCls> points, int MaxClass)
        {


            Regression = false;
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

            return new BasicMLDataSet(input, idealOutput);
        }
        public BasicMLDataSet InitializeRegressionSet(List<DataPoint> points)
        {
            Regression = true;
            double[][] input = new double[points.Count][];
            for (int i = 0; i < points.Count; i++)
            {
                input[i] = new[] { points[i].X};
            }

            double[][] idealOutput = new double[points.Count][];
            for (int i = 0; i < points.Count; i++)
            {
                idealOutput[i] = new[] { points[i].Y };
            }
            return new BasicMLDataSet(input, idealOutput);
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
                    tab[i-1] = 0;
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
            this.resultList = new ResultsList();

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

        



        public void ComputeSet(IMLDataSet setToCompute)
        {
            

            IOutput writer = MyCore.Resolve<IOutput>();

            foreach (IMLDataPair pair in setToCompute)
            {
               
                IMLData output = Network.Compute(pair.Input);
                if (!Regression)
                    writer.Write(String.Format(@"{0},{1}, actual={2},ideal={3}", pair.Input[0], pair.Input[1], GetOutputFormat(output), GetClass(pair.Ideal)));
                else
                    writer.Write(String.Format(@"{0}, actual={1},ideal={2}", pair.Input[0], output, pair.Ideal));
                AddToResults(pair, output);
            }
        }
        private string GetOutputFormat(IMLData output)
        {
            int maxIndex = 0;
            double max = double.MinValue;
            for (int i = 0; i < output.Count; i++)
            {
                if (output[i] > max)
                {
                    max = output[i];
                    maxIndex = i;
                }
            }
            return " Output class: " + (maxIndex + 1) + " ";
        }




        private void AddToResults(IMLDataPair pair, IMLData output)
        {
            resultList.ListX.Add(pair.Input[0]);
            if (Regression == false)
            {
                resultList.ListY.Add(pair.Input[1]);
                resultList.ListIdealOutput.Add(IMDataToDoubleArray(pair.Ideal));
                resultList.ListActualOutput.Add(IMDataToDoubleArray(output));
            }
            else
                resultList.ListY.Add(output[0]);
            resultList.Count++;
        }

        private double[] IMDataToDoubleArray(IMLData d)
        {
            double[] array = new double[d.Count];
            for(int i=0; i<d.Count; i++)
            {
                array[i] = d[i];
            }

            return array;


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