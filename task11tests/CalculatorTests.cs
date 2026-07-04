using Xunit;
using task11;

namespace task11tests
{
    public class CalculatorTests
    {
        [Fact]
        public void Calculator_Add_ReturnCorrectResult()
        {
            dynamic calc = CalculatorGenerator.CreateCalculator();
            int result = calc.Add(3, 7);
            Assert.Equal(10, result);
        }

        [Fact]
        public void Calculator_ReturnCorrectResult()
        {
            dynamic calc = CalculatorGenerator.CreateCalculator();
            int result = calc.Minus(5, 4);
            Assert.Equal(1, result);
        }

        [Fact]
        public void Calculator_Mul_ReturnCorrectResult()
        {
            dynamic calc = CalculatorGenerator.CreateCalculator();
            int result = calc.Mul(5, 4);
            Assert.Equal(20, result);
        }

        [Fact]
        public void Calculator_Div_ReturnCorrectResult()
        {
            dynamic calc = CalculatorGenerator.CreateCalculator();
            int result = calc.Div(10, 2);
            Assert.Equal(5, result);
        }
    }
}