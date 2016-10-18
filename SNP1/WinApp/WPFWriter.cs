using SNP1.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WinApp
{
    public class WPFWriter : IOutput
    {
        public void Write(string str)
        {
            //WinApp.MainWindow.myrtf.
            (Application.Current.MainWindow as MainWindow).SetTextForRichTextBox(str);
        }
    }
}
