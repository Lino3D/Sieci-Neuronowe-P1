﻿using Caliburn.Micro;
using Encog.App.Analyst;
using Encog.App.Analyst.CSV.Normalize;
using Encog.App.Analyst.Script.Normalize;
using Encog.App.Analyst.Wizard;
using Encog.Util.CSV;
using Microsoft.Win32;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using SNP1;
using SNP1.DataHelper;
using SNP1.EPPlus;
using SNP1.Models;
using SNP1.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace WinAppV2.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public MainViewModel()
        {
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
                OnPropertyChanged("LearningRate");
            }
        }

        public int Iterations
        {
            get
            {
                return iterations;
            }

            set
            {
                iterations = value;
                OnPropertyChanged("Iterations");
            }
        }

        public bool Bias
        {
            get
            {
                return bias;
            }

            set
            {
                bias = value;
                OnPropertyChanged("Bias");
            }
        }

        public string Console
        {
            get
            {
                return console;
            }

            set
            {
                console = value + "\n" + console;
                OnPropertyChanged("Console");
            }
        }

        public double MomentumRate
        {
            get
            {
                return momentumRate;
            }

            set
            {
                momentumRate = value;
                OnPropertyChanged("MomentumRate");
            }
        }

        private PlotModel classModel;

        public PlotModel LearningProcessModel
        {
            get
            {
                return learningProcessModel;
            }

            set
            {
                learningProcessModel = value;
                OnPropertyChanged("LearningProcessModel");
            }
        }

        public PlotModel ClassModel
        {
            get
            {
                return classModel;
            }

            set
            {
                classModel = value;
                OnPropertyChanged("ClassModel");
            }
        }

        public bool UnipolarChecked
        {
            get
            {
                return unipolarChecked;
            }

            set
            {
                unipolarChecked = value;
                OnPropertyChanged("UnipolarChecked");
            }
        }

        public bool DataLoadedChecked
        {
            get
            {
                return dataLoadedChecked;
            }

            set
            {
                dataLoadedChecked = value;
                OnPropertyChanged("DataLoadedChecked");
            }
        }

        public bool IsRegression
        {
            get
            {
                return isRegression;
            }

            set
            {
                isRegression = value;
                OnPropertyChanged("IsRegression");
            }
        }

        public int Layers
        {
            get
            {
                return layers;
            }

            set
            {
                layers = value;
                OnPropertyChanged("Layers");
            }
        }

        public int Neurons
        {
            get
            {
                return neurons;
            }

            set
            {
                neurons = value;
                OnPropertyChanged("Neurons");
            }
        }

        public bool TestLoadedChecked
        {
            get
            {
                return testLoadedChecked;
            }

            set
            {
                testLoadedChecked = value;
                OnPropertyChanged("TestLoadedChecked");
            }
        }

        public PlotModel RegressionModel
        {
            get
            {
                return regressionModel;
            }

            set
            {
                regressionModel = value;
                OnPropertyChanged("RegressionModel");

            }
        }

        private bool testLoadedChecked = false;
        private int neurons = 10;
        private int layers = 4;
        private bool isRegression = false;

        private bool dataLoadedChecked = false;
        private bool unipolarChecked = true;

        private double learningRate = 0.000001;
        private double momentumRate = 0.000001;
        private int iterations = 5000;
        private bool bias = true;
        private string console;
        private PlotModel learningProcessModel;
        private PlotModel regressionModel;

        private SimpleNeuralNetwork Network;
        private List<DataPointCls> DataPointsClassificationTraining;
        private List<DataPointCls> DataPointClassificationTest;
        private List<SNP1.Models.DataPoint> DataPointsRegressionTraining;
        private List<SNP1.Models.DataPoint> DataPointRegressionTest;
        private List<IterationError> LearningProcess;
        private string csvPath;
        private string csvPathTest;
        private static string csvPathNormalized;
        private static string csvPathNormalizedTest;
        public ResultsList resultList;

        public  void NormalizeData(string normalPath, string normalizedPath)
        {
            var toNormalizeTraining = new FileInfo(normalPath);
            var NormalizedTest = new FileInfo(normalizedPath);
            var analyst = new EncogAnalyst();


            var wizard = new AnalystWizard(analyst);
            if (isRegression)
                wizard.TargetFieldName = "y";
            else
                wizard.TargetFieldName = "cls";
            
            //toNormalize.
            wizard.Wizard(toNormalizeTraining, true, AnalystFileFormat.DecpntComma);
           

            var norm = new AnalystNormalizeCSV();
            norm.Analyze(toNormalizeTraining, true, CSVFormat.English, analyst);
            foreach (AnalystField field in analyst.Script.Normalize.NormalizedFields)
            {
                field.NormalizedHigh = 1;
                field.NormalizedLow = 0;
                field.Action = Encog.Util.Arrayutil.NormalizationAction.Normalize;
                if (field.Name == "cls")
                    field.Action = Encog.Util.Arrayutil.NormalizationAction.PassThrough;
            }


            norm.ProduceOutputHeaders = true;
            norm.Normalize(NormalizedTest);
        }




        public void Run()
        {
            IOutput writer = MyCore.Resolve<IOutput>();
            if (csvPath == null)
            {
                
                writer.Write("Please load training set first!");
                return;
            }
            if(this.csvPathTest==null)
            {
                writer.Write("Please load test set first!");
                return;
            }

            InitializeDataAndNeurons();
            if (!UnipolarChecked)
                Network.SetBiPolarActivation();
            else
                Network.SetSigmoidActivation();
            Network.StartLearning(Iterations);
            LearningProcess = Network.learningProcess;
            DrawLearningRate();

            Network.ComputeSet(Network.TestSet);

            resultList = Network.resultList;
            if (!isRegression)
                DrawGraph();
            else
                DrawRegressionFunction();
        }

        private void InitializeDataAndNeurons()
        {
            NormalizeData(csvPath, csvPathNormalized);
            NormalizeData(csvPathTest, csvPathNormalizedTest);
            Network = new SimpleNeuralNetwork((double)learningRate, (double)momentumRate, bias);
            if (isRegression == false)
            {
                DataPointsClassificationTraining = (new ImportDataPointSets(csvPathNormalized).DataPoints);
                DataPointClassificationTest = (new ImportDataPointSets(csvPathNormalizedTest).DataPoints);
                Network.TrainingSet = Network.InitializeClassificationSet(DataPointsClassificationTraining, 4);
                Network.TestSet = Network.InitializeClassificationSet(DataPointClassificationTest, 4);
                Network.AddLayer(2);
                Network.AddLayerBunch(Layers, Neurons);
                Network.AddLayer(4);
            }
            else
            {
                DataPointsRegressionTraining = (new ImportDataPointsSetsRegression(csvPathNormalized).DataPoints);
                DataPointRegressionTest = (new ImportDataPointsSetsRegression(csvPathNormalizedTest).DataPoints);
                Network.TrainingSet = Network.InitializeRegressionSet(DataPointsRegressionTraining);
                Network.TestSet = Network.InitializeRegressionSet(DataPointRegressionTest);
                Network.AddLayer(1);
                Network.AddLayerBunch(Layers, Neurons);
                Network.AddLayer(1);
            }

        }
        public void LoadDataTest()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == true)
            {
                csvPathTest = ofd.FileName;
                csvPathNormalizedTest = ofd.FileName.TrimEnd(".csv".ToCharArray()) + "Normalized.csv";
                TestLoadedChecked = true;
            }
            else
                TestLoadedChecked = false;
        }

        public void LoadData()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == true)
            {
                csvPath = ofd.FileName;
                csvPathNormalized = ofd.FileName.TrimEnd(".csv".ToCharArray())+"Normalized.csv";
                DataLoadedChecked = true;
            }
            else
                DataLoadedChecked = false;
        }

        public void DrawLearningRate()
        {
            LearningProcessModel = new PlotModel { Title = "Learning Rate" };

            var lineSeries = new LineSeries();

            foreach (IterationError e in LearningProcess)
            {
                lineSeries.Points.Add(new OxyPlot.DataPoint(e.Iteration1, e.Error1));
            }
            LearningProcessModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = 0, Maximum = 2 });
            LearningProcessModel.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = 0, Maximum = LearningProcess.Count });
            LearningProcessModel.Series.Add(lineSeries);
        }

        public void DrawRegressionFunction()
        {
            RegressionModel= new PlotModel { Title = "Regression function" };

            var lineSeries = new LineSeries();

            for (int i = 0; i < resultList.Count; i++)
            {
                lineSeries.Points.Add(new OxyPlot.DataPoint(resultList.ListX[i], resultList.ListY[i]));
            }
            RegressionModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = 0, Maximum = resultList.ListX.Max() });
            RegressionModel.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = 0, Maximum = resultList.ListY.Max() });
            RegressionModel.Series.Add(lineSeries);
        }

        public void DrawGraph()
        {
            ClassModel = new PlotModel { Title = "Class Model" };

            var scatterSeries = new ScatterSeries { MarkerType = MarkerType.Circle };
            for (int i = 0; i < resultList.Count; i++)
            {
                var x = resultList.ListX[i];
                var y = resultList.ListY[i];
                var size = 5;
                var colorValue = InitializeColor(resultList.ListActualOutput[i]);
                scatterSeries.Points.Add(new ScatterPoint(x, y, size, colorValue));
            }
            ClassModel.Series.Add(scatterSeries);
            ClassModel.Axes.Add(new LinearColorAxis { Position = AxisPosition.Right, Palette = OxyPalettes.Jet(200) });
        }

        public int InitializeColor(double[] output)
        {
            int color = 1;
            int foundClassIndex = output.ToList().IndexOf(output.Max());
            for (int i = 0; i < output.Count(); i++)
            {
                if (i ==foundClassIndex)
                {
                    color = i*40;
                }
            }

            return color;
        }

        public double ReturnMaxX(List<DataPointCls> list)
        {
            double maxX = 0;
            foreach (var x in list)
            {
                if (x.X > maxX)
                    maxX = x.X;
            }
            return maxX;
        }

        public double ReturnMaxY(List<DataPointCls> list)
        {
            double maxY = 0;
            foreach (var x in list)
            {
                if (x.Y > maxY)
                    maxY = x.X;
            }
            return maxY;
        }

        private void ThreadStartingPoint()
        {
            IWindowManager manager = new WindowManager();
            //var viewModel = Activator.CreateInstance(typeof(InProgressViewModel), null);
            var a = manager.ShowDialog(typeof(InProgressViewModel), null, null);
            //InProgressView tempWindow = new InProgressView();
            //     tempWindow.Show();
            System.Windows.Threading.Dispatcher.Run();
        }
    }
}