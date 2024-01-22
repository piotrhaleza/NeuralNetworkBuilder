using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearning.Biases
{
    public class InitRandomBiases : IInitBiases
    {
        public void Init(Network network)
        {
            var random = new Random();

            foreach (var item in network.Layers)
                item.Bias = random.NextDouble();
        }
    }
}
