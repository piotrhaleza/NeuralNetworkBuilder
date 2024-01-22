using MachineLearning;
using MachineLearningWpfUI.Models;
using MachineLearningWpfUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;

namespace MachineLearningWpfUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ParameterViewModel Context { get; set; }

        public MainWindow()
        {
            DataContext = Context = new ParameterViewModel(this);
            InitializeComponent();
        }
        public MainWindow(Network network)
        {
            DataContext = Context = new ParameterViewModel(network, this);
            InitializeComponent();
        }

        #region Validate Numbers
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        #endregion

        #region Moving Mode
        public void TurnOnMovingMode()
        {
            ListBox layersListBox = (ListBox)FindName("LayersWpf");
            layersListBox.SelectedItem = null;

            Style itemContainerStyle = new Style(typeof(ListBoxItem));
            itemContainerStyle.Setters.Add(new Setter(ListBoxItem.AllowDropProperty, true));
            itemContainerStyle.Setters.Add(new EventSetter(ListBoxItem.PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(s_PreviewMouseLeftButtonDown)));
            itemContainerStyle.Setters.Add(new EventSetter(ListBoxItem.DropEvent, new DragEventHandler(listbox1_Drop)));
            itemContainerStyle.Setters.Add(new Setter(ListBoxItem.BackgroundProperty, Brushes.Transparent));
            layersListBox.ItemContainerStyle = itemContainerStyle;
        }
        public void TurnOffMovingMode()
        {
            ListBox layersListBox = (ListBox)FindName("LayersWpf");

            Style itemContainerStyle = new Style(typeof(ListBoxItem));
            layersListBox.ItemContainerStyle = itemContainerStyle;
        }

        public LayerModel GetSelectedItem()
        {
            ListBox layersListBox = (ListBox)FindName("LayersWpf");

            return layersListBox.SelectedItem as LayerModel;
        }


        private Point startPoint;

        void s_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            if (sender is ListBoxItem item)
            {
                ListBoxItem draggedItem = sender as ListBoxItem;
                DragDrop.DoDragDrop(draggedItem, draggedItem.DataContext, DragDropEffects.Move);
                item.Background = Brushes.LightBlue;
                draggedItem.IsSelected = true;
            }
        }

        void listbox1_Drop(object sender, DragEventArgs e)
        {
            ListBox layersListBox = (ListBox)FindName("LayersWpf");

            LayerModel droppedData = e.Data.GetData(typeof(LayerModel)) as LayerModel;
            LayerModel target = ((ListBoxItem)(sender)).DataContext as LayerModel;

            int removedIdx = layersListBox.Items.IndexOf(droppedData);
            int targetIdx = layersListBox.Items.IndexOf(target);

            if (removedIdx < targetIdx)
            {
                Context.Layers.Insert(targetIdx + 1, droppedData);
                Context.Layers.RemoveAt(removedIdx);
            }
            else
            {
                int remIdx = removedIdx + 1;
                if (Context.Layers.Count + 1 > remIdx)
                {
                    Context.Layers.Insert(targetIdx, droppedData);
                    Context.Layers.RemoveAt(remIdx);
                }
            }

            for (int i = 0; i < Context.Layers.Count(); i++)
            {
                Context.Layers[i].Id = i;
            }
        }

        #endregion

        private int lastSelectedItem;

       
    }
}
