using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNP1.Models
{
    public class IterationError
    {
        private double Error;
        private int Iteration;

        public double Error1
        {
            get
            {
                return Error;
            }

            set
            {
                Error = value;
            }
        }

        public int Iteration1
        {
            get
            {
                return Iteration;
            }

            set
            {
                Iteration = value;
            }
        }

        internal IterationError() { }
        internal IterationError(int x, double y)
        {
            Iteration = x;
            Error = y;          
        }


    }
}
