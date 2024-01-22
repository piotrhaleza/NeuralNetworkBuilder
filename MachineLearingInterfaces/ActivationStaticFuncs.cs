using MachineLearingInterfaces.ActivationFunc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearingInterfaces.ActivationFunc
{
    public partial class SigmoidActivationFunc
    {
        public static string OrginalPattern => "f(x) = 1/(1+e^x)";
        public static string RealPattern => "f(x) = {m}/({b}+e^({a}*x)) + {d}";
        public static Dictionary<string, double> OrginalParameters => new Dictionary<string, double>() { { "a", 1 }, };

    }
}
