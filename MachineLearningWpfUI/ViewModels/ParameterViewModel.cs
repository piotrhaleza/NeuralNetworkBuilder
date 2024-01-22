using MachineLearning;
using MachineLearning.Biases;
using MachineLearning.Wages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using MachineLearingInterfaces;
using MachineLearningWpfUI.Models;
using System.Collections.ObjectModel;
using MachineLearningWpfUI.Base;
using MachineLearningWpfUI.Models.Enums;
using MachineLearningWpfUI.Views;

namespace MachineLearningWpfUI.ViewModels
{
    public class ParameterViewModel : BaseViewModel
    {
        #region Properties
        public Network Network { get; set; }
        private ObservableCollection<LayerModel> layers;

        public ObservableCollection<LayerModel> Layers
        {
            get { return layers; }
            set
            {
                layers = value;
                OnPropertyChanged();
            }
        }
        private AppMode appMode;

        public AppMode AppMode
        {
            get { return appMode; }
            set
            {
                appMode = value;
                OnPropertyChanged();
            }
        }
        private CreatingMode creatingMode;

        public CreatingMode CreatingMode
        {
            get { return creatingMode; }
            set
            {
                if (creatingMode == CreatingMode.Moving && value != CreatingMode.Moving)
                    Parent.TurnOffMovingMode();
                if (creatingMode != CreatingMode.Moving && value == CreatingMode.Moving)
                    Parent.TurnOnMovingMode();

                creatingMode = value;
                OnPropertyChanged();
            }
        }
        public MainWindow Parent { get; set; }
        #endregion
        #region Constructors
        public ParameterViewModel(Network network, MainWindow parent)
        {
            Network = network;
            Parent = parent;
            layers = new ObservableCollection<LayerModel>(LayerModel.Map(network.Layers));
            AppMode = AppMode.Create;
        }
        public ParameterViewModel( MainWindow parent)
        {
            Network = null;
            Parent = parent;
            layers = new ObservableCollection<LayerModel>();
            AppMode = AppMode.Create;
        }
        #endregion
        #region Commands
        public Command AddCommand
        {
            get
            {
                return new Command(() =>
                {
                    Layers.Add(new LayerModel(Layers.Count() > 0? Layers.Max(x => x.Id) + 1:0));
                }, () => CreatingMode != CreatingMode.Moving);
            }
        }
        public Command DeleteCommand
        {
            get
            {
                return new Command(() =>
                {
                    var model = Parent.GetSelectedItem();

                    if(model != null)
                        Layers.Remove(model);

                    for (int i = 0; i < Layers.Count(); i++)
                        Layers[i].Id = i;
                }, () => CreatingMode != CreatingMode.Moving);
            }
        }
        public Command SaveCommand
        {
            get
            {
                return new Command(() =>
                {
                    Layers.Add(new LayerModel(Layers.Max(x => x.Id) + 1));
                }, () => CreatingMode != CreatingMode.Moving);
            }
        }
        public Command MoveCommand
        {
            get
            {
                return new Command(() =>
                {
                    CreatingMode = CreatingMode == CreatingMode.Moving ? CreatingMode.None : CreatingMode.Moving;
                });
            }
        }
        public Command ClickCommand
        {
            get
            {
                return new Command(() =>
                
                
                {
                    var propertiesWindow = new PropertiesWindow();

                    // Ustaw okno właścicielskie
                    propertiesWindow.Owner = Application.Current.MainWindow;

                    // Ustaw pozycję na środku okna właścicielskiego
                    propertiesWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;

                    // Pokaż okno
                    propertiesWindow.ShowDialog();
                });
            }
        }
        #endregion
    }
}
