using MachineLearingInterfaces;
using MachineLearingInterfaces.ActivationFunc;
using MachineLearning;
using MachineLearning.Biases;
using MachineLearning.Wages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace MachineLearningsUi
{
    public partial class Form1 : Form
    {
        List<(ILayer layer, INeuron neuron, Panel panels)> neurons { get; set; } = new List<(ILayer layer, INeuron neuron, Panel panels)>();
        List<IActivationFunc> funcs = new List<IActivationFunc>();
        public Network Network { get; set; }

        #region Values
        private int Margin = 100;
        private int Diameter = 100;
        #endregion
        public Form1(Network network)
        {
            this.AutoScroll = true;
            Network = network;
            funcs = new List<IActivationFunc>() { new LineralActivationFunc() , new SigmoidActivationFunc() };
            InitializeComponent();
            TestM2ethod1();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            foreach (var item in funcs)
            {
                var panel = new Panel();
                panel.Width = 400;
                var text = new Label();
                text.Text = item.ToString();

                var textBox = new TextBox();
                textBox.BorderStyle = BorderStyle.FixedSingle;

                panel.Controls.Add(text);
                panel.Controls.Add(textBox);
              
            }
        }
        public void TestM2ethod1()
        {

            Network network;

            var inputLayer = new Layer(1,1, funcs[0]);
            var hiddenfirsLayer = new Layer(6,2, funcs[0]);
            var hiddenfirsLayer2 = new Layer(6,3, funcs[1]);
            var output = new Layer(1,4);
        
            network = new Network(new List<ILayer>() { inputLayer, hiddenfirsLayer2, hiddenfirsLayer, output }, new InitHeWages(), new InitZeroBiases(), 10);

            Random random = new Random(); // Inicjalizacja generatora liczb losowych
            int a;
           

            if (!int.TryParse(textBox1.Text, out a))
                a = 1;

            progressBar1.Minimum = 0;
            progressBar1.Maximum = a;
            
            while (count < a)
            {
                progressBar1.Value = count;

                double randinput = random.NextDouble();
                double input1 = ((randinput) * Math.PI * 2) ; ; // Losowa wartość z zakresu [0, 1)

                var inputs = new List<double> { randinput };
                var expected = new List<double> { ((Math.Sin(input1)+1)/2) }; // Oczekiwane wyniki to sinus wartości wejściowych

                network.TrainModel(inputs, expected);
               
                count++;
                if (count > 10000)
                {

                }
                var output1 = network.Outputs.NeuronList.First().Value;
                 chart1.Series["Series1"].Points.AddXY(inputs[0], expected[0]);
                chart1.Series["Series2"].Points.AddXY(inputs[0], output1);


                if (count % 10000 == 0)
                {
                    chart1.Series["Series1"].Points.Clear();
                    chart1.Series["Series2"].Points.Clear();
                }
            }

            count = 0;
        }
        public int count = 0;
        public void CreateNeatworkUI()
        {
            var size = (this.Width - 2 * Margin) / Network.Layers.Count();
            var numberLayer = 0;
            var MaxSizeNeurons = Network.Layers.Max(x => x.NeuronList.Count());

            foreach (var layer in Network.Layers)
            {
                int neuronid = 0;

                foreach (var neuron in layer.NeuronList)
                {

                    var label = GetLabelNeuron(neuronid);
                    var neuronPanel = GetNeuron(size, numberLayer, MaxSizeNeurons, layer.NeuronList.Count(), neuronid);

                    neuronPanel.Controls.Add(label);
                    neurons.Add((layer, neuron, neuronPanel));
                    this.Controls.Add(neuronPanel);

                    neuronid++;
                }

                numberLayer++;
            }
        }

        public Panel GetNeuron(int size,int numberLayer,int max, int coutNeuron,int neuronid)
        {
            var circle = new Panel();
            circle.BackColor = Color.Red;
            circle.Size = new System.Drawing.Size(Diameter, Diameter);
            circle.Location = new System.Drawing.Point(Margin + size * numberLayer, max * (int)(1.2 * Diameter) / 2 - (coutNeuron / 2 - neuronid) * (int)(1.2 * Diameter));
            circle.Region = new Region(new System.Drawing.Drawing2D.GraphicsPath());
            GraphicsPath path = new GraphicsPath();
            path.AddEllipse(0, 0, Diameter, Diameter);
            circle.Region = new Region(path);
            
            return circle;
        }
        public Label GetLabelNeuron(int neuronid)
        {
            var label = new Label();
            label.Text = neuronid.ToString();
            label.Location = new System.Drawing.Point(Diameter / 2, Diameter / 2);

            return label;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            int trackBarValue = trackBar1.Value;
            AdjustScaleFromTrackBarValue(trackBarValue);
        }
        private void AdjustScaleFromTrackBarValue(int trackBarValue)
        {
            // Przykładowe zakresy wartości
            double minValue = 0.1;
            double maxValue = 2.0;
            int minTrackBarValue = 0;
            int maxTrackBarValue = 100;

            // Mapowanie wartości z TrackBar na zakres od minValue do maxValue
            double scale = ((maxValue - minValue) * (double)(trackBarValue - minTrackBarValue) / (maxTrackBarValue - minTrackBarValue)) + minValue;

            neurons.First().panels.Size = new System.Drawing.Size(32, 32);
            GraphicsPath path = new GraphicsPath();
            path.AddEllipse(0, 0, 32, 32);
            neurons.First().panels.Region = new Region(path);
            // Teraz możesz wykorzystać zmienną "scale" do dostosowania skali lub wykonywania innych operacji z nią związanych
            // Przykład ustawienia skali w kontrolce, jeśli jest to kontrolka, która obsługuje skalę
            //yourControl.Scale = scale;
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            TestM2ethod1();
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }
    }
}
