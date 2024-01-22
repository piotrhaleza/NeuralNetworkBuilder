using MachineLearingInterfaces.ActivationFunc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningWpfUI
{
    public enum ActivationFuncEnum
    {
        Sigmoid = 0,
        Lineral = 1,
        Tanh =2,
        ReLu =3,
        SoftMax = 4,
        
    }

    public static class ActivationFuncMapper
    {
        public static ActivationFuncEnum Map(this KindOfActivationFunc func)
        {
            return (ActivationFuncEnum)func;
        }
    }
}
