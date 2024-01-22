using MachineLearingInterfaces;
using MachineLearingInterfaces.ActivationFunc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearingInterfaces
{
    public interface ILayer
    {
        IList<INeuron> NeuronList { get; }

        IEnumerable<double> NeuronValues { get; }

        IEnumerable<Dictionary<INeuron, IWage>> Wages { get; }

        IActivationFunc ActivationFunc { get; }

        ILayer NextLayer { get; set; }

        ILayer PreviousLayer { get; set; }

        string Name { get; set; }

        int Size { get; }

        int Id { get; set; }

        double Bias { get; set; }

        double GradientBiasa { get; set; }

        void CalculateNextNeuron(INeuron neuron);
    }

}
