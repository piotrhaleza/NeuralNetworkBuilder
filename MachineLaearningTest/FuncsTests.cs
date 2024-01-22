
using MachineLearingInterfaces;
using MachineLearingInterfaces.ActivationFunc;
using MachineLearning;
using MachineLearning.Biases;
using MachineLearning.Wages;
using System.Text.RegularExpressions;

namespace MachineLaearningTest
{
    [TestClass]
    public class FuncsTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            List<List<double>> wages = new List<List<double>>();
            wages.Add(new List<double>() { 0.11,  0.12, 0.21, 0.08 });
            wages.Add(new List<double>() { 0.14, 0.15 });
      
            var inputs = new List<double> { 2,3 };
            var expected = new List<double> {1 };
            var biases = new List<double> { 0,0 };

            var inputLayer = new Layer(2,1,new LineralActivationFunc());
            var hiddenfirsLayer = new Layer(2,2, new LineralActivationFunc());
            var output = new Layer(1,3);
          

            TestNetwork network = new TestNetwork(new List<ILayer>() { inputLayer,hiddenfirsLayer,output},wages,biases);

            network.TrainModel(inputs,expected);

            //network.Calculate(inputs, expected);
            //network.CalculateGradient(expected);
            //network.UpdateWages();


        }
        [TestMethod]
        public void TestM2ethod1()
        {

            //Network network;

            //var inputLayer = new Layer();
            //inputLayer.AddNewNeuron(1);
            //var hiddenfirsLayer = new Layer();
            //hiddenfirsLayer.AddNewNeuron(16);
           
            //var output = new Layer();
            //output.AddNewNeuron(1);

            //network = new Network(new List<ILayer>() { inputLayer, hiddenfirsLayer, output },new InitRandomWages(),new  InitZeroBiases(), new SigmoidActivationFunc(), 10);

            //Random random = new Random(); // Inicjalizacja generatora liczb losowych

            //while (true)
            //{
            //    double input1 = (random.NextDouble() * 2 * Math.PI) - Math.PI; ; // Losowa wartoœæ z zakresu [0, 1)

            //    var inputs = new List<double> { input1};
            //    var expected = new List<double> { Math.Sin(input1) + 1}; // Oczekiwane wyniki to sinus wartoœci wejœciowych

            //    network.Calculate(inputs, expected);
            //    network.CalculateGradient(expected);
            //    network.UpdateWages();
            //}


        }
    }
}