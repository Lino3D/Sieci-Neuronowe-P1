using CsvHelper;
using SNP1.CsvOperations.Maps;
using SNP1.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNP1
{
    public class ImportDataPointsSetsRegression
    {
        public List<DataPoint> DataPoints { get; set; }

        public string DataPath { get; set; }

        public ImportDataPointsSetsRegression()
        {
        }

        public ImportDataPointsSetsRegression(string dataFilePath)
        {
            this.DataPath = dataFilePath;
            ImportFromCsV();
        }

        public void ImportFromCsV()
        {
            using (var streamReader = File.OpenText(this.DataPath))
            {
                var reader = new CsvReader(streamReader);
                reader.Configuration.RegisterClassMap<DataPointMap>();
                var dataPoints = reader.GetRecords<DataPoint>().ToList();
                this.DataPoints = dataPoints;
            }
        }
    }
}
