using CsvHelper.Configuration;
using SNP1.Models;
using System.Globalization;

namespace SNP1.CsvOperations.Maps
{
    public class DataPointMap : CsvClassMap<DataPoint>
    {
        public DataPointMap()
        {
            Map(m => m.X).Index(0).TypeConverterOption(CultureInfo.InvariantCulture);
            Map(m => m.Y).Index(1).TypeConverterOption(CultureInfo.InvariantCulture);
        }
    }
}