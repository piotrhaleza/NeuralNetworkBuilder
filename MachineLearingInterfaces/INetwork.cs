using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearingInterfaces
{
    public interface INetwork
    {
        ILayer Inputs { get; }

        ILayer Outputs { get; }

        IList<ILayer> Layers { get; }

        int BatchSize { get; }

        double Loss { get; set; }

        double LearningRate { get; set; }

        bool ForwardPropagation(IList<double> inputs, IList<double> expected);

        bool BackPropagation(IList<double> expected);

        IList<double> TrainModel(IList<double> inputs, IList<double> expected);

        IList<double> GetResults(IList<double> inputs, IList<double> expected);
    }
}
