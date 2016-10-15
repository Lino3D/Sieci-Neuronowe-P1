using CsvHelper.Configuration;
using SNP1.Models;
using System.Globalization;

namespace SNP1.CsvOperations.Maps
{
    internal sealed class DataPointClsMap : CsvClassMap<DataPointCls>
    {
        public DataPointClsMap()
        {
            Map(m => m.X).Index(0).TypeConverterOption(CultureInfo.InvariantCulture);
            Map(m => m.Y).Index(1).TypeConverterOption(CultureInfo.InvariantCulture);
            Map(m => m.Cls).Index(2);
        }
    }
}