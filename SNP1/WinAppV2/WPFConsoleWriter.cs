using SNP1.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WinAppV2.ViewModels;
using WinAppV2.Views;

namespace WinAppV2
{
    public class WPFConsoleWriter : IOutput
    {
        public void Write(string str)
        {
            var a = Application.Current.Windows.OfType<MainView>().SingleOrDefault().DataContext as MainViewModel;
            a.Console = str;
        }
    }
}
