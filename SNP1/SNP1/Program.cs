using SNP1.EPPlus;
using System;

namespace SNP1
{
    internal class Program
    {
        private static string csvPath = "C:\\Users\\Lino\\Desktop\\SN\\Klasyfikacja\\datatrain.csv";

        private static void Main(string[] args)
        {
            var trainSet = new ImportDataPointSets(csvPath);

            Console.ReadKey();
        }
    }
}