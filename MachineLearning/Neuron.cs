using MachineLearingInterfaces;
using MachineLearning.Wages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearning
{
    public class Neuron : INeuron
    {
        #region Readonly properties
        private readonly Dictionary<INeuron, IWage> _wages;
        #endregion

        #region Public properties
        public int Id { get; set; }
        public double Value { get; set; }
        public double DerivativeValue { get; set; }
        public Dictionary<INeuron, IWage> Wages => _wages;
        #endregion

        #region Constructors
        public Neuron(int layerId)
        {
            Id = layerId;
            _wages = new Dictionary<INeuron, IWage>();
        }
        #endregion
    }
}
