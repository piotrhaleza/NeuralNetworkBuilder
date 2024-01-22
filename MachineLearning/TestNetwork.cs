using MachineLearingInterfaces;
using MachineLearning.Wages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearning
{
    public class TestNetwork : Network
    {
        public TestNetwork(IList<ILayer> layers, List<List<double>> wages, IList<double> bias) : base(layers, new InitRandomWages(), null, 0)
        {
            int i = 0;

            foreach (var layer in Layers)
            {
                if (layer == Outputs)
                    continue;

                int z = 0;
                foreach (var item in layer.Wages.ToList())
                    foreach (var wage in item)
                        wage.Value.Value = wages[i][z++];

                layer.Bias = bias[i];
                i++;
            }

        }
    }
}
