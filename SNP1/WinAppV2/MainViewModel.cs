using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinAppV2
{
    public class MainViewModel
    {
        public string HelloString
        {
            get
            {
                return "Witaj!";
            }
        }

        public string FirstName
        {
            get
            {
                return firstName;
            }

            set
            {
                firstName = value;
            }
        }

        private string firstName = "Witam";
    }
}
