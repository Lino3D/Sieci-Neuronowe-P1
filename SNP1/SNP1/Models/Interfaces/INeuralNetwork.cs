using Encog.Engine.Network.Activation;
using Encog.ML.Data;
using Encog.Neural.Networks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNP1.Models.Interfaces
{
    public interface INeuralNetwork
    {
        IMLDataSet TrainingSet { get; set; }

        BasicNetwork Network { get; set; }

        IActivationFunction ActivationFunction { get; set; }

        double LearningRate { get; set; }

        double TheMomentum { get; set; }

        bool HasBias { get; set; }
    }
}
