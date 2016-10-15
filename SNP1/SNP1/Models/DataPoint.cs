using SNP1.Models.Interfaces;

namespace SNP1.Models
{
    public class DataPoint : IDataPoint
    {
        public double X { get; set; }
        public double Y { get; set; }

        public DataPoint()
        {
        }

        public DataPoint(double a, double b)
        {
            this.X = a;
            this.Y = b;
        }
    }
}