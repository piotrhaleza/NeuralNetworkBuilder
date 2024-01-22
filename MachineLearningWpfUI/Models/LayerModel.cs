using MachineLearingInterfaces;
using MachineLearingInterfaces.ActivationFunc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningWpfUI.Models
{
    public class LayerModel : INotifyPropertyChanged
    {
        #region Public Properties
        public ActivationFuncEnum KindActivation
        {
            get
            {
                return kindActivation;
            }
            set
            {
                kindActivation = value;

                switch (kindActivation)
                {
                    case ActivationFuncEnum.Sigmoid:
                        Pattern = SigmoidActivationFunc.OrginalPattern; break;
                    case ActivationFuncEnum.Lineral:
                        Pattern = LineralActivationFunc.OrginalPattern; break;
                    case ActivationFuncEnum.Tanh:
                        Pattern = TanhActivationFunc.OrginalPattern; break;
                    case ActivationFuncEnum.ReLu:
                        Pattern = ReLuActivationFunc.OrginalPattern; break;
                    case ActivationFuncEnum.SoftMax:
                        Pattern = SoftmaxActivationFunc.OrginalPattern; break;
                    default:
                        break;
                }

                OnPropertyChanged();
            }
        }
        public int Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
                OnPropertyChanged(nameof(Odd));
                OnPropertyChanged();
            }
        }
        private int id { get; set; }
        private ActivationFuncEnum kindActivation;
        public string Pattern
        {
            get
            {
                return pattern;
            }
            set
            {
                pattern = value;
                OnPropertyChanged(nameof(Odd));
                OnPropertyChanged();
            }
        }
        private string pattern { get; set; }
        public string RealPattern
        {
            get
            {
                return realPattern;
            }
            set
            {
                realPattern = value;
                OnPropertyChanged();
            }
        }
        private string realPattern { get; set; }
        public bool Odd => Id % 2 == 0;
        public int NumberOfNeurons { get; set; }
        #endregion
        public LayerModel(int id)
        {
            KindActivation = ActivationFuncEnum.Lineral;
            Id = id;
            NumberOfNeurons = 1;
        }
        public LayerModel(ILayer layer)
        {
            if (layer.ActivationFunc != null)
            {
                KindActivation = layer.ActivationFunc.KindActivation.Map();
            }
            Id = layer.Id;
            NumberOfNeurons = layer.NeuronList.Count;
        }

        public static IEnumerable<LayerModel> Map(IList<ILayer> layers)
        {
            foreach (var layer in layers)
                yield return new LayerModel(layer);
        }
        #region PropertyChanged
        /// <summary>
        /// Zdarzenie obsługujące zmianę wartości właściwości (implementowane przez INotifyPropertyChanged).
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Podnosi zdarzenie PropertyChanged dla konkretnej wałaściwości.
        /// </summary>
        /// <param name="name">Nazwa właściwości.</param>
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion
    }
}
