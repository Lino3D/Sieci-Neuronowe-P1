using SNP1.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNP1.Models
{
    public class ConsoleWriter : IOutput
    {
        public void Write(string str)
        {
            Console.WriteLine(str);
        }
    }
}
