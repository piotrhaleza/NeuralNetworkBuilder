using MachineLearingInterfaces;
using MachineLearning.errors;
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
    public class Network : INetwork
    {
        #region Readonly Properties
        public readonly int _batchSize;
        public readonly IList<ILayer> _layers;
        #endregion

        #region Public Properties
        public ILayer Inputs => Layers.FirstOrDefault();

        public ILayer Outputs => Layers.LastOrDefault();

        public IList<ILayer> Layers => _layers;

        public int BatchSize => _batchSize;

        public double Loss { get; set; }

        public double LearningRate { get; set; }
        #endregion

        #region Constructors

        public Network(IList<ILayer> layers, IInitWages initWages, IInitBiases initBiases, int batchSize)
        {
            _layers = PrepareLayers(layers);

            initWages?.Init(this);
            initBiases?.Init(this);

            _batchSize = batchSize;

            LearningRate = 0.05;
        }
        public Network(IInitWages initWages, IInitBiases initBiases, int batchSize)
        {
            initWages?.Init(this);
            initBiases?.Init(this);

            _batchSize = batchSize;

            LearningRate = 0.05;
        }
        #endregion

        #region Public Methods

        public IList<ILayer> PrepareLayers(IList<ILayer> layers)
        {
            for (int i = 0; i < layers.Count(); i++)
            {
                if (i != layers.Count() - 1)
                    layers[i].NextLayer = layers[i + 1];
                if (i != 0)
                    layers[i].PreviousLayer = layers[i - 1];
            }

            return layers;
        }

        public bool ForwardPropagation(IList<double> inputs, IList<double> expected)
        {
            //tutaj dodaj warunki i logi

            for (int i = 0; i < inputs.Count(); i++)
                Inputs.NeuronList[i].Value = inputs[i];

            foreach (var layer in Layers.Skip(1))
                foreach (var neuron in layer.NeuronList)
                    layer.CalculateNextNeuron(neuron);

            Loss = MachineLearning.errors.Loss.SquareLoss(Outputs.NeuronValues.ToArray(), expected);

            return true;
        }
        public bool BackPropagation(IList<double> expected)
        {
            BackPropagationOutputs(expected);

            foreach (var layer in Layers.Reverse().Skip(1))
                BackPropagationHiddenLayer(layer);

            UpdateWages();
            return true;
        }
        public IList<double> TrainModel(IList<double> inputs, IList<double> expected)
        {
            ForwardPropagation(inputs, expected);
            BackPropagation(expected);
            return Outputs.NeuronValues.ToList();
        }

        public IList<double> GetResults(IList<double> inputs, IList<double> expected)
        {
            ForwardPropagation(inputs, expected);
            return Outputs.NeuronValues.ToList();
        }
        #endregion

        #region Private Methods
        private List<double> GetSumOfGradtients(IEnumerable<Dictionary<INeuron, IWage>> dicts)
        {
            List<double> sum = new List<double>();

            foreach (var neuron_wages in dicts)
                sum.Add(neuron_wages.Sum(x => x.Value.Error * x.Value.Value));

            return sum;
        }

        private void BackPropagationHiddenLayer(ILayer layer)
        {
            if (layer.PreviousLayer == null)
                return;

            var listOfSumsGradients = GetSumOfGradtients(layer.Wages);

            foreach (var neuron in layer.NeuronList)
            {
                foreach (var wage in layer.PreviousLayer.NeuronList.Select(x => (x, x.Wages[neuron])))
                {
                    wage.Item2.Gradient = listOfSumsGradients[neuron.Id] * layer.PreviousLayer.ActivationFunc.Pochodna(neuron.DerivativeValue) * wage.x.Value;
                    wage.Item2.Error = listOfSumsGradients[neuron.Id] * layer.PreviousLayer.ActivationFunc.Pochodna(neuron.DerivativeValue);
                }
                layer.PreviousLayer.GradientBiasa += listOfSumsGradients[neuron.Id] * layer.PreviousLayer.ActivationFunc.Pochodna(neuron.DerivativeValue);
            }
            layer.PreviousLayer.GradientBiasa /= layer.NeuronList.Count();
        }
        private void BackPropagationOutputs(IList<double> expected)
        {
            foreach (var neuron in Outputs.NeuronList)
            {
                foreach (var previousNeuron in Outputs.PreviousLayer.NeuronList)
                {
                    var wage = previousNeuron.Wages[neuron];

                    wage.Gradient = (neuron.Value - expected[neuron.Id]) * Outputs.PreviousLayer.ActivationFunc.Pochodna(neuron.DerivativeValue) * previousNeuron.Value;
                    wage.Error = (neuron.Value - expected[neuron.Id]) * Outputs.PreviousLayer.ActivationFunc.Pochodna(neuron.DerivativeValue);
                }
                Outputs.PreviousLayer.GradientBiasa += (neuron.Value - expected[neuron.Id]) * Outputs.PreviousLayer.ActivationFunc.Pochodna(neuron.DerivativeValue);
            }
            Outputs.PreviousLayer.GradientBiasa /= Outputs.NeuronList.Count;
        }

        private void UpdateWages()
        {
            foreach (var layer in Layers)
            {
                foreach (var item in layer.NeuronList)
                    foreach (var wage in item.Wages.Select(x => x.Value))
                        wage.Value = wage.Value - LearningRate * wage.Gradient;

                layer.Bias = layer.Bias - LearningRate * layer.GradientBiasa;
                layer.GradientBiasa = 0;
            }
        }

       
        #endregion
    }
}
