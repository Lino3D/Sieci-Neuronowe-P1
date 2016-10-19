using SNP1.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WinAppV2
{
    public class WPFWriter : IOutput
    {
        public void Write(string str)
        {
            //WinApp.MainWindow.myrtf.
            MainViewModel mv;
            var a = Application.Current.Windows.OfType<MainView>().SingleOrDefault().DataContext as MainViewModel;
            a.FirstName = str;
       ///     Application.Current.Windows.OfType<MainView>().SingleOrDefault().UpdateLayout();
            //   mv.FirstName = str;
            //     (Application.Current.MainWindow as MainView).label.Content = str;

        }
    }
}
