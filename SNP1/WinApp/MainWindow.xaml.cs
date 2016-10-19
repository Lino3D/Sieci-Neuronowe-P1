using Castle.MicroKernel.Registration;
using SNP1.DataHelper;
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

        public void SetTextForRichTextBox(string text)
        {
            rtf.AppendText(text);
        }

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new costammodel();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {

            ProgramController.InitializeSimpleNetwork();
        }
    }
}
