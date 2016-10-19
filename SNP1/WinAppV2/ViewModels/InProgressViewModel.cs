using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinAppV2.ViewModels
{
    public class InProgressViewModel : BaseViewModel
    {
        private string progresUczenia = "Liczenie...";

        public string ProgresUczenia
        {
            get
            {
                return progresUczenia;
            }

            set
            {
                progresUczenia = value;
            }
        }
    }
}
