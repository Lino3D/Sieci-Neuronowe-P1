using CsvHelper;
using SNP1.CsvOperations.Maps;
using SNP1.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SNP1.EPPlus
{
    public class ImportDataPointSets
    {
        public List<DataPointCls> DataPoints { get; set; }

        public string DataPath { get; set; }

        public ImportDataPointSets()
        {
        }

        public ImportDataPointSets(string dataFilePath)
        {
            this.DataPath = dataFilePath;
            ImportFromCsV();
        }

        public void ImportFromCsV()
        {
            using (var streamReader = File.OpenText(this.DataPath))
            {
                var reader = new CsvReader(streamReader);
                reader.Configuration.RegisterClassMap<DataPointClsMap>();
                var dataPoints = reader.GetRecords<DataPointCls>().ToList();
                this.DataPoints = dataPoints;
            }
        }




    }
}