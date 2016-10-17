using Castle.MicroKernel.Registration;
using Encog;
using Encog.Engine.Network.Activation;
using Encog.ML;
using Encog.ML.Data;
using Encog.ML.Data.Basic;
using Encog.ML.Data.Versatile;
using Encog.ML.Data.Versatile.Columns;
using Encog.ML.Data.Versatile.Sources;
using Encog.ML.Factory;
using Encog.ML.Model;
using Encog.Neural.Networks;
using Encog.Neural.Networks.Layers;
using Encog.Neural.Networks.Training;
using Encog.Neural.Networks.Training.Propagation;
using Encog.Neural.Networks.Training.Propagation.Back;
using Encog.Neural.Pattern;
using Encog.Util.CSV;
using SNP1.DataHelper;
using SNP1.EPPlus;
using SNP1.Models;
using SNP1.Models.Interfaces;
using System;
using System.Collections.Generic;

namespace SNP1
{
    internal class Program
    {
        private static string csvPath = @"..\..\Resource\datatrain.csv";

        private static IMLMethod CreateFeedforwardNetwork()
        {
            // construct a feedforward type network
            var pattern = new FeedForwardPattern();
            pattern.ActivationFunction = new ActivationSigmoid();
            pattern.InputNeurons = 1;
            pattern.AddHiddenLayer(2);
            pattern.OutputNeurons = 1;
            return pattern.Generate();
        }

        private static void Main(string[] args)
        {
            MyCore.Container.Register(Component.For<IOutput>().ImplementedBy<ConsoleWriter>());

            // To nie pasuje bo nie pozwala na customizację ( jest generowane przez factory, chyba lepiej tworzyć samemu)

            //            IVersatileDataSource dataSource = new CSVDataSource(csvPath, true, CSVFormat.DecimalPoint);
            //            VersatileMLDataSet data = new VersatileMLDataSet(dataSource);
            //            data.DefineSourceColumn("x", 0, ColumnType.Nominal);
            //            data.DefineSourceColumn("y", 1, ColumnType.Nominal);
            //            ColumnDefinition outputColumn = data.DefineSourceColumn("cls", 2, ColumnType.Nominal);
            //            data.Analyze();
            //            data.DefineSingleOutputOthersInput(outputColumn);


            ////            var methodFactory = new MLMethodFactory();
            ////            var method = methodFactory.Create(
            ////            MLMethodFactory.TYPEFEEDFORWARD,
            ////            ”?:B−> SIGMOID−> 4:B−> SIGMOID−>?”,
            ////2,
            ////1);

            //            var model = new EncogModel(data);
            //            model.SelectMethod(data, MLMethodFactory.TypeFeedforward);
            //            model.Report = new ConsoleStatusReportable();
            //            data.Normalize();

            List<DataPointCls> points = (new ImportDataPointSets(csvPath).DataPoints);

            SimpleNeuralNetwork myNetwork = new SimpleNeuralNetwork();
            myNetwork.InitializeTrainingSet(points);
            myNetwork.ActivationFunction =new ActivationBiPolar();
            myNetwork.AddLayer(2);
            myNetwork.AddLayerBunch(8, 3);
            myNetwork.AddLayer(1);
            myNetwork.StartLearning(10000);


      



            Console.ReadKey();
        }
    }
}