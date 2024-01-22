using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MachineLearingInterfaces;
using MachineLearning.Wages;

namespace MachineLearning.Wages
{
    public class Wage : IWage
    {
        public double Value { get; set; }
        public double Gradient { get; set; }
        public double Error { get; set; }
    }
}
