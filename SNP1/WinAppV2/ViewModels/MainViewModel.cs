using Caliburn.Micro;
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

        private double learningRate = 0.0005;
        private double momentumRate = 0.0005;
        private int iterations = 100;
        private bool bias = true;
        private string console;

        private SimpleNeuralNetwork Network;
        private List<DataPointCls> points;
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
            points = (new ImportDataPointSets(csvPath).DataPoints);
    //      Network.InitializeTrainingSet(points);
            ProgramController.SetBiPolarActivation(Network);

            Network.AddLayer(2);
            Network.AddLayerBunch(8, 3);
            Network.AddLayer(1);

            //Thread newWindowThread = new Thread(new ThreadStart(ThreadStartingPoint));
            //newWindowThread.SetApartmentState(ApartmentState.STA);
            //newWindowThread.IsBackground = true;
            //newWindowThread.Start();

            //  ErrorCalculator.CalculateError(Network.StartLearning(Iterations).ToList(), Network);
            Network.StartLearning(Iterations);

            ErrorCalculator.CalculateError(Network.ComputeTrainingSet().ToList(), Network);

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
