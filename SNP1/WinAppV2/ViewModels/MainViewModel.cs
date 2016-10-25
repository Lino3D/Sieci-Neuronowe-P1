using Caliburn.Micro;
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

        private double learningRate = 0.0005;
        private double momentumRate = 0.0005;
        private int iterations = 100;
        private bool bias = true;
        private string console;
        private PlotModel learningProcessModel;

        private SimpleNeuralNetwork Network;
        private List<DataPointCls> DataPoints;
        private List<IterationError> LearningProcess;
        private string csvPath = @"..\..\Resource\datatrain.csv";

        //public event PropertyChangedEventHandler PropertyChanged;
        //protected void OnPropertyChanged(string propertyName)
        //{
        //    PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}

        public void Run()
        {
         
            //   ProgramController.InitializeSimpleNetwork();
            Network = new SimpleNeuralNetwork((double)learningRate, (double)momentumRate, bias);
            DataPoints = (new ImportDataPointSets(csvPath).DataPoints);
            Network.InitializeTrainingSet(DataPoints,4);
            ProgramController.SetBiPolarActivation(Network);

            Network.AddLayer(2);
            Network.AddLayerBunch(2, 4);
            Network.AddLayer(4);

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

        public void DrawShit()
        {
            ClassModel= new PlotModel { Title = "Class Model" };

            
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
