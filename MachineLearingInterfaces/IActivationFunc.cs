using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearingInterfaces.ActivationFunc
{
    public enum KindOfActivationFunc
    {
        Sigmoid = 0,
        Lineral = 1,
        Tanh =2,
        Relu = 3,
        SoftMax = 4,
    }

    public interface IActivationFunc
    {
        double Activate(double input);

        double Pochodna(double input);

        Dictionary<string, double> Parameters { get; set; }

        KindOfActivationFunc KindActivation {get;}

    }

    public abstract class ActivationFunc : IActivationFunc
    {
        public Dictionary<string, double> Parameters { get; set; }

        public abstract KindOfActivationFunc KindActivation {get;}

        public abstract double Activate(double input);

        public abstract double Pochodna(double input);
        
    }

    public partial class SigmoidActivationFunc : ActivationFunc
    {
        public override KindOfActivationFunc KindActivation => KindOfActivationFunc.Sigmoid;

        public SigmoidActivationFunc(double a = 1, double b = 0, double DenominatorBias = 1, double MeterBias = 1)
        {
           var c = SigmoidActivationFunc.OrginalParameters;

            Parameters = new Dictionary<string, double>();
            Parameters.Add("a", a);
            Parameters.Add("b",b);
            Parameters.Add("d", DenominatorBias);
            Parameters.Add("m", MeterBias);
        }

        public override double Activate(double input)
        {
            return (Parameters["m"]) / (Math.Exp(-input* Parameters["a"]) + Parameters["d"]) + Parameters["b"];
        }

        public override double Pochodna(double input)
        {
            return Activate(input) * (1.0 - Activate(input));
        }

    }

    public class TanhActivationFunc : ActivationFunc
    {
        public static string OrginalPattern => "f(x) = (e^x – e^-x) / (e^x + e^-x)";
        public static string RealPattern => "f(x) = m/(b+e^(a*x)) + d";


        public override KindOfActivationFunc KindActivation => KindOfActivationFunc.Tanh;

        public static double TanhActivation(double x)
        {
            return (Math.Exp(x) - Math.Exp(-x)) / (Math.Exp(x) + Math.Exp(-x));
        }

        public override double Activate(double input)
        {
            return Math.Tanh(input);
        }

        public override double Pochodna(double input)
        {
            double coshValue = Math.Cosh(input);
            return 1.0 / (coshValue * coshValue);
        }
    }
    public class ReLuActivationFunc : ActivationFunc
    {

        public static string OrginalPattern => "f(x) = ax + b (f(x) < 0 = 0)";
        public static string RealPattern => "f(x) = {a}*x + {b}";

        public override KindOfActivationFunc KindActivation => KindOfActivationFunc.Relu;

        public int A { get; set; } = 1;
        public int B { get; set; } = 0;

        public override double Activate(double input)
        {
            return Math.Max(0, A * input + B);
        }

        public override double Pochodna(double input)
        {
            if (input > 0)
                return A;
            else
                return 0;
        }
    }
   
    public class LineralActivationFunc : ActivationFunc
    {
        public override KindOfActivationFunc KindActivation => KindOfActivationFunc.Lineral;
        public static string OrginalPattern => "f(x) = ax + b";
        public static string RealPattern => "f(x) = {a}*x + {b}";

        public LineralActivationFunc(double a = 1, double b = 0)
        {
            Parameters = new Dictionary<string, double>();
            Parameters.Add("a", a);
            Parameters.Add("b", b);
        }

        public override double Activate(double input)
        {
            return Parameters["a"] * input + Parameters["b"];
        }

        public override double Pochodna(double input)
        {
            return Parameters["a"];
        }
     
    }
    public class SoftmaxActivationFunc : ActivationFunc
    {
        public static string OrginalPattern => "Nie obsłużone";
        public static string RealPattern => "f(x) = {a}*x + {b}";

        public override KindOfActivationFunc KindActivation => KindOfActivationFunc.SoftMax;

        public override double Activate(double input)
        {
            return 1.0 / (1.0 + Math.Exp(-input));
        }

        public override double Pochodna(double input)
        {
            throw new NotImplementedException();
        }
    }
}
