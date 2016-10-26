using SNP1.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Encog.ML.Data;

namespace SNP1.Models
{
    public class Result : IResult
    {
        private IMLDataPair input;
        private IMLData output;

        public IMLDataPair Input
        {
            get
            {
                return input;
            }

            set
            {
                input = value;
            }
        }

        public IMLData Output
        {
            get
            {
                return output;
            }

            set
            {
                output = value;
            }
        }

       
    }
}
