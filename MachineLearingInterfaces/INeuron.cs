using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearingInterfaces
{
    public interface INeuron
    {
        int Id { get; set; }

        double Value { get; set; }
        double DerivativeValue { get; set; }


        Dictionary<INeuron, IWage> Wages { get; }
    }
   
}
