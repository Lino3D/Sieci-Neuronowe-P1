using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNP1.Models
{
   public class ResultsList
    {
        public List<double> ListX { get; set; }
        public List<double> ListY { get; set; }
        public List<double[]> ListActualOutput { get; set; }
        public List<double[]> ListIdealOutput { get; set; }

        public ResultsList()
        {
            ListX = new List<double>();
            ListY = new List<double>();
            ListActualOutput = new List<double[]>();
            ListIdealOutput = new List<double[]>();
        }





    }
}
