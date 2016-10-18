using Caliburn.Micro;
using SNP1.DataHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinAppV2
{
    public class MainViewModel : INotifyPropertyChanged
    {

        private string firstName = "Witam";
        public string FirstName
        {
            get
            {
                return firstName;
            }

            set
            {
                firstName = value;
                OnPropertyChanged("FirstName");
            }
        }

     


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Run()
        {
            ProgramController.InitializeSimpleNetwork(100);
        }
    }
}
