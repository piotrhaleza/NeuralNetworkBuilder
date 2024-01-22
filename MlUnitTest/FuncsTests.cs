using MachineLearingInterfaces.ActivationFunc;
using MachineLearning;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlUnitTest
{
    [TestFixture]
    public class FuncsTests
    {
        [TestCase(1, 2, 1, ExpectedResult = 3)]
        [TestCase(1, 0, 1, ExpectedResult = 1)]
        [TestCase(0, 5, 1, ExpectedResult = 5)]
        [TestCase(10, 2, 2, ExpectedResult = 22, Description = "zupa")]
        public double LineralFuncActivateTest(double a, double b, double x)
        {
            var func = new LineralActivationFunc(a, b);

            var result = func.Activate(x);

            return result;
        }
        [Test]
        public void SigmoidFuncActivateTest()
        {
            var func = new LineralActivationFunc();

            Assert.That(func, Is.Not.Null, "Ma nie byc nullem");
        }

    }
}
