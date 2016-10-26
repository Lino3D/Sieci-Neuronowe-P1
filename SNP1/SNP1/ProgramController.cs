using Encog;
using Encog.App.Analyst;
using Encog.App.Analyst.CSV.Normalize;
using Encog.App.Analyst.Script.Normalize;
using Encog.App.Analyst.Wizard;
using Encog.Engine.Network.Activation;
using Encog.ML;
using Encog.ML.Data;
using Encog.ML.Data.Basic;
using Encog.ML.Data.Versatile;
using Encog.ML.Data.Versatile.Columns;
using Encog.ML.Data.Versatile.Sources;
using Encog.ML.Factory;
using Encog.ML.Model;
using Encog.Neural.Pattern;
using Encog.Util.CSV;
using SNP1.EPPlus;
using SNP1.Models;
using SNP1.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNP1.DataHelper
{
    public static class ProgramController
    {
        private static string csvPath = @"..\..\Resource\datatrain.csv";
        private static string csvPathNormalized = @"..\..\Resource\datatrainNormalized.csv";

        public static void NormalizeData()
        {
            var toNormalize = new FileInfo(csvPath);
            var Normalized = new FileInfo(csvPathNormalized);
            var analyst = new EncogAnalyst();


            var wizard = new AnalystWizard(analyst);

            wizard.Wizard(toNormalize, true, AnalystFileFormat.DecpntComma);

            var norm = new AnalystNormalizeCSV();
            norm.Analyze(toNormalize, true, CSVFormat.English, analyst);
            foreach (AnalystField field in analyst.Script.Normalize.NormalizedFields)
            {
                field.NormalizedHigh = 1;
                field.NormalizedLow = -1;
                field.Action = Encog.Util.Arrayutil.NormalizationAction.Normalize;
                if (field.Name == "cls")
                    field.Action = Encog.Util.Arrayutil.NormalizationAction.PassThrough;
            }


            norm.ProduceOutputHeaders = true;
            norm.Normalize(Normalized);
        }



        public static void InitializeSimpleNetwork(int interations)
        {
            NormalizeData();
         



            List<DataPointCls> points = (new ImportDataPointSets(csvPathNormalized).DataPoints);


            SimpleNeuralNetwork myNetwork = new SimpleNeuralNetwork();
            myNetwork.InitializeTrainingSet(points,4);
            var ts = myNetwork.TrainingSet;
            myNetwork.ActivationFunction = new ActivationBiPolar();
            myNetwork.AddLayer(2);
            myNetwork.AddLayerBunch(2, 4);
            myNetwork.AddLayer(4);
            myNetwork.StartLearning(interations);
            ErrorCalculator.CalculateError(myNetwork.ComputeTrainingSet().ToList(), myNetwork);
        }
        public static void InitializeSimpleNetwork( )
        {
            InitializeSimpleNetwork(4000);
        }

        public static void SetBiPolarActivation(this INeuralNetwork network)
        {
            network.ActivationFunction = new ActivationBiPolar();
        }
        

        public static void SetSigmoidActivation(this INeuralNetwork network)
        {
            network.ActivationFunction = new ActivationSigmoid();
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
