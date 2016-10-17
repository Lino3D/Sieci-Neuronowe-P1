using SNP1.Models;
using SNP1.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNP1.DataHelper
{
    public static class ErrorCalculator
    {
        public static double CalculateError(List<IResult> lst, INeuralNetwork network)
        {
            double sum = 0, sumIteration = 0;
            int i = network.Network.OutputCount;
            foreach (var item in lst)
            {
                sumIteration = 0;
                for (int j = 0; j < i; j++)
                    sumIteration += item.Input.Ideal[j] - item.Output[j];
                sum += sumIteration / i;
            }
            var ret = sum / lst.Count();
            MyCore.Container.Resolve<IOutput>().Write("Mean output with " + lst.Count + " neurons consisting of " + i + " layers is " + ret);
            return ret;
        }
    }
}
