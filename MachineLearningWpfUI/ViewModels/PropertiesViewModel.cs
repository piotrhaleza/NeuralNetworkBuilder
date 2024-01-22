using MachineLearning;
using MachineLearningWpfUI.Base;
using MachineLearningWpfUI.Models.Enums;
using MachineLearningWpfUI.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningWpfUI.ViewModels
{
    internal class PropertiesViewModel : BaseViewModel
    {
        #region Properties

        public LayerModel EditLayer
        {
            get { return editLayer; }
            set
            {
                editLayer = value;
                OnPropertyChanged();
            }
        }
        private LayerModel editLayer;
        
        #endregion
        #region Constructors
        public PropertiesViewModel(LayerModel layer)
        {
            EditLayer = layer;
        }
        
        #endregion
        #region Commands
        public Command EndCommand
        {
            get
            {
                return new Command(() =>
                {
                });
            }
        }
        #endregion
    }
}
