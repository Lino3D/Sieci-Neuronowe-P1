using Caliburn.Micro;
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
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WinAppV2.Views;

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


        private bool testLoadedChecked = false;
        private int neurons = 10;
        private int layers = 4;
        private bool isRegression = false;

        private bool dataLoadedChecked = false;
        private bool unipolarChecked = true;

        private double learningRate = 0.7;
        private double momentumRate = 0.8;
        private int iterations = 100;
        private bool bias = true;
        private string console;
        private PlotModel learningProcessModel;

        private SimpleNeuralNetwork Network;
        private List<DataPointCls> DataPoints;
        private List<SNP1.Models.DataPoint> DataPointsRegression;      
        private List<IterationError> LearningProcess;
        private string csvPath = @"..\..\Resource\datatrain.csv";
        private string csvPathTest = @"..\..\Resource\datatrain.csv";

        public ResultsList resultList;

        //public event PropertyChangedEventHandler PropertyChanged;
        //protected void OnPropertyChanged(string propertyName)
        //{
        //    PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}

        public void Run()
        {
          
            //   ProgramController.InitializeSimpleNetwork();
            Network = new SimpleNeuralNetwork((double)learningRate, (double)momentumRate, bias);
            if (isRegression == false)
            {
                DataPoints = (new ImportDataPointSets(csvPath).DataPoints);
                Network.InitializeTrainingSet(DataPoints, 4);
                Network.AddLayer(2);
                Network.AddLayerBunch(Layers, Neurons);
                Network.AddLayer(4);
            }
            else
            {
                DataPointsRegression = (new ImportDataPointsSetsRegression(csvPath).DataPoints);
                Network.InitializeTrainingSetRegression(DataPointsRegression);
                Network.AddLayer(1);
                Network.AddLayerBunch(Layers, Neurons);
                Network.AddLayer(1);
            }
            if (!UnipolarChecked)
                Network.SetBiPolarActivation();
            else
                Network.SetSigmoidActivation();
       

            //Thread newWindowThread = new Thread(new ThreadStart(ThreadStartingPoint));
            //newWindowThread.SetApartmentState(ApartmentState.STA);
            //newWindowThread.IsBackground = true;
            //newWindowThread.Start();

            //  ErrorCalculator.CalculateError(Network.StartLearning(Iterations).ToList(), Network);
            Network.StartLearning(Iterations);
            LearningProcess = Network.learningProcess;
            DrawLearningRate();

            var result = Network.ComputeTrainingSet().ToList();
            //ErrorCalculator.CalculateError(Network.ComputeTrainingSet().ToList(), Network);
            resultList = Network.resultList;
            if(!isRegression)
            DrawGraph();

        }
        public void LoadDataTest()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == true)
            {
                csvPathTest = ofd.FileName;
                TestLoadedChecked = true;
            }
            else
                TestLoadedChecked = false;
        }
        public void LoadData()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if( ofd.ShowDialog() == true)
            {
                csvPath = ofd.FileName;
                DataLoadedChecked = true;

            }
            else
            DataLoadedChecked = false;
        }

        public void DrawLearningRate()
        {
            LearningProcessModel = new PlotModel { Title = "Learning Rate" };

            var lineSeries = new LineSeries();

            foreach(IterationError e in LearningProcess)
            {
                lineSeries.Points.Add(new OxyPlot.DataPoint(e.Iteration1, e.Error1));
            }
           LearningProcessModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = 0, Maximum = 2 });
           LearningProcessModel.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = 0, Maximum = LearningProcess.Count });
           LearningProcessModel.Series.Add(lineSeries);
            
        }

        public void DrawGraph()
        {




            ClassModel= new PlotModel { Title = "Class Model" };


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

            //ClassModel.Axes.Add(new OxyPlot.Axes.LinearColorAxis
            //{
            //    Position = OxyPlot.Axes.AxisPosition.Right,
            //    Palette = OxyPalettes.Jet(500),
            //    HighColor = OxyColors.Gray,
            //    LowColor = OxyColors.Black
            //});

            //OxyPlot.Series.ContourSeries contour = new OxyPlot.Series.ContourSeries
            //{
            //    ColumnCoordinates = arrayFromMinToMax1,
            //    RowCoordinates = shitList.,
            //    ContourLevels = arrayOfLevels,
            //    ContourColors = arrayOfColors, // Same # elements as the levels' array
            //    Data = (Double[,])data
            //};

            //model.Series.Add(contour);

        }


        public int InitializeColor(double[] output)
        {
            int color =1;

            for(int i=0;i<output.Count(); i++)
            {
                if (output[i] == 1)
                {
                    color = i + 100;
                }
            }

            return color;
        }

        public double ReturnMaxX(List<DataPointCls> list)
        {
            double maxX = 0;
            foreach(var x in list)
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
            var a  = manager.ShowDialog(typeof(InProgressViewModel), null, null);
            //InProgressView tempWindow = new InProgressView();
       //     tempWindow.Show();
            System.Windows.Threading.Dispatcher.Run();

        }
        
    }
}
