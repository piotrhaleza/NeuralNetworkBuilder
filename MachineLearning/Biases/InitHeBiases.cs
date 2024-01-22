using MachineLearingInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearning.Biases
{
    public class InitHeBiases : IInitWages
    {
        public void Init(INetwork network)
        {
            var random = new Random();

            foreach (var item in network.Layers)
                item.Bias = random.NextDouble() * Math.Sqrt(2.0 / network.Inputs.Size);
        }
    }
}
