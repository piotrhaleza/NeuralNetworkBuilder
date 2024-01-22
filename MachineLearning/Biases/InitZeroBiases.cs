using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearning.Biases
{
    public class InitZeroBiases : IInitBiases
    {
        public void Init(Network network)
        {
            foreach (var item in network.Layers)
                item.Bias = 0;
        }
    }
}
