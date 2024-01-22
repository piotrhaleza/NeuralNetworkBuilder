using System;

namespace MachineLarningInterfaces
{
    public interface IWage
    {
        double Value { get; set; }
        double Gradient { get; set; }
        double Error { get; set; }
    }
}
