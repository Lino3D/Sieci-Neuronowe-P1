using Castle.MicroKernel.Registration;
using SNP1.DataHelper;
using SNP1.Models;
using SNP1.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WinApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MyCore.Container.Register(Component.For<IOutput>().ImplementedBy<ConsoleWriter>());
        }

        public string _ResultLabel
        {
            get
            {
                return ResultLabel.Content.ToString();
            }
            set
            {
                ResultLabel.Content = value;
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            //ProgramController.InitializeSimpleNetwork();
            _ResultLabel = ProgramController.InitializeSimpleNetwork().ToString();

        }
    }
}
