using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearingInterfaces
{
    public interface IWage
    {
        double Value { get; set; }
        double Gradient { get; set; }
        double Error { get; set; }
    }
}
