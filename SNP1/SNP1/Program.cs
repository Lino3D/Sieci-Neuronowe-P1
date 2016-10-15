using Encog;
using Encog.Engine.Network.Activation;
using Encog.ML;
using Encog.ML.Data.Versatile;
using Encog.ML.Data.Versatile.Columns;
using Encog.ML.Data.Versatile.Sources;
using Encog.ML.Factory;
using Encog.ML.Model;
using Encog.Neural.Pattern;
using Encog.Util.CSV;
using SNP1.EPPlus;
using SNP1.Models;
using System;
using System.Collections.Generic;

namespace SNP1
{
    internal class Program
    {
        private static string csvPath = "C:\\Users\\Lino\\Desktop\\SN\\Klasyfikacja\\datatrain.csv";

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
            List<DataPointCls> points = (new ImportDataPointSets(csvPath).DataPoints);

            double[][] input = new double[points.Count][];

            for (int i = 0; i < points.Count; i++)
            {
                input[i] = new[] { points[i].X, points[i].Y };
            }

            double[][] output = new double[points.Count][];
            for (int i = 0; i < points.Count; i++)
            {
                output[i] = new[] { (double)points[i].Cls };
            }

            //

            IVersatileDataSource dataSource = new CSVDataSource(csvPath, true, CSVFormat.DecimalPoint);
            VersatileMLDataSet data = new VersatileMLDataSet(dataSource);
            data.DefineSourceColumn("x", 0, ColumnType.Nominal);
            data.DefineSourceColumn("y", 1, ColumnType.Nominal);
            ColumnDefinition outputColumn = data.DefineSourceColumn("cls", 2, ColumnType.Nominal);
            data.Analyze();
            data.DefineSingleOutputOthersInput(outputColumn);


//            var methodFactory = new MLMethodFactory();
//            var method = methodFactory.Create(
//            MLMethodFactory.TYPEFEEDFORWARD,
//            ”?:B−> SIGMOID−> 4:B−> SIGMOID−>?”,
//2,
//1);

            var model = new EncogModel(data);
            model.SelectMethod(data, MLMethodFactory.TypeFeedforward);
            model.Report = new ConsoleStatusReportable();
            data.Normalize();

            Console.ReadKey();
        }
    }
}