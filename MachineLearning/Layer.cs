using MachineLearingInterfaces;
using MachineLearingInterfaces.ActivationFunc;
using MachineLearning.Wages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearning
{
    public class Layer : ILayer
    {
        #region Readonly properties
        private readonly IActivationFunc _activationFunc;
        private readonly IList<INeuron> _neuronList;
        #endregion

        #region Properites
        public IActivationFunc ActivationFunc => _activationFunc;
        public IList<INeuron> NeuronList => _neuronList;
        public IEnumerable<double> NeuronValues => NeuronList.Select(x=>x.Value);
        public ILayer NextLayer { get; set; }
        public ILayer PreviousLayer { get;  set; }

        public IEnumerable<Dictionary<INeuron,IWage>> Wages => NeuronList.Select(x => x.Wages);

        public int Id { get; set; }
        public int Size => NeuronList.Count();
        public double Bias { get; set; }
        public string Name { get; set; }
        public double GradientBiasa { get; set; }

        #endregion

        #region Constructors
        public Layer(int countOfNeurons,int id ,IActivationFunc activationFunc = null)
        {
            Id = id;
            _activationFunc = activationFunc;
            _neuronList = new INeuron[countOfNeurons];
            AddNeurons(countOfNeurons);
        }
        #endregion

        #region Public Methods
        public void CalculateNextNeuron(INeuron neuron)
        {
            if(PreviousLayer == null) 
                return;
            neuron.DerivativeValue = PreviousLayer.NeuronList.Select(x => x.Value * x.Wages[neuron].Value).Sum() + Bias;
            neuron.Value = PreviousLayer.ActivationFunc.Activate(neuron.DerivativeValue);
        }
        #endregion

        #region Private Methods
        private void AddNeurons(int count)
        {
            for (int i = 0; i < count; i++)
                NeuronList[i] =(new Neuron(i));
        }     
        #endregion

    }
}
