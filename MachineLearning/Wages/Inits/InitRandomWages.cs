using MachineLearingInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearning.Wages
{
    public class InitRandomWages : IInitWages
    {
        public void Init(INetwork network)
        {
            var random = new Random();

            for (int i = 0; i < network.Layers.Count() - 1; i++)
            {
                var firstLayer = network.Layers.ToArray()[i];
                var secondLayer = network.Layers.ToArray()[i + 1];

                foreach (var item in firstLayer.NeuronList)
                    foreach (var secondItem in secondLayer.NeuronList)
                        item.Wages.Add(secondItem,new Wage() { Value = (random.NextDouble() * 2)});
            }
        }
    }
}
