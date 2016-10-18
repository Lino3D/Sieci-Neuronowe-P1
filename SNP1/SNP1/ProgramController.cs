using Encog.Engine.Network.Activation;
using Encog.ML;
using Encog.Neural.Pattern;
using SNP1.EPPlus;
using SNP1.Models;
using SNP1.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNP1.DataHelper
{
    public static class ProgramController
    {
        private static string csvPath = @"..\..\Resource\datatrain.csv";

        public static void InitializeSimpleNetwork(int interations)
        {
            List<DataPointCls> points = (new ImportDataPointSets(csvPath).DataPoints);

            SimpleNeuralNetwork myNetwork = new SimpleNeuralNetwork();
            myNetwork.InitializeTrainingSet(points);
            myNetwork.ActivationFunction = new ActivationBiPolar();
            myNetwork.AddLayer(2);
            myNetwork.AddLayerBunch(8, 3);
            myNetwork.AddLayer(1);

            ErrorCalculator.CalculateError(myNetwork.StartLearning(interations).ToList(), myNetwork);
        }
        public static void InitializeSimpleNetwork( )
        {
            InitializeSimpleNetwork(10000);
        }
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


    }
}
