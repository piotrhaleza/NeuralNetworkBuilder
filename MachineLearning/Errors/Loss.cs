using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearning.errors
{
    public static class Loss
    {
        public static double CrossEntropyLoss(double[] predicted, double[] target)
        {
            double epsilon = 1e-15; // Mała stała zapobiegająca dzieleniu przez zero
            double[] clippedPredicted = predicted.Select(p => Math.Max(epsilon, Math.Min(1 - epsilon, p))).ToArray();

            double loss = 0;
            for (int i = 0; i < predicted.Length; i++)
            {
                loss += -target[i] * Math.Log(clippedPredicted[i]);
            }

            return loss;
        }
        public static double SquareLoss(IList<double> predicted, IList<double> target)
        {
            double loss = 0;
            for (int i = 0; i < predicted.Count; i++)
                loss += 0.5 * (target[i] - predicted[i]) * (target[i] - predicted[i]);

            return loss;
        }
    }
}
