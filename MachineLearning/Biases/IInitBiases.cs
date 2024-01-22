using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearning
{
    public interface IInitBiases
    {
        void Init(Network network);
    }
}
