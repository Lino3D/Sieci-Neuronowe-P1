using SNP1.Models.Interfaces;

namespace SNP1.Models
{
    public class DataPointCls : DataPoint, IDataPoint
    {
        public int Cls { get; set; }

        public DataPointCls()
        {
        }

        public DataPointCls(double a, double b, int c)
        {
            this.X = a;
            this.Y = b;
            this.Cls = c;
        }

        public DataPointCls(double a, double b) : base(a, b)
        {
        }
    }
}