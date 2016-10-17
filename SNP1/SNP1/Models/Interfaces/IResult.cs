using Encog.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNP1.Models
{
    public interface IResult
    {
        IMLData Output { get; set; }
        IMLDataPair Input { get; set; }        
    }
}
