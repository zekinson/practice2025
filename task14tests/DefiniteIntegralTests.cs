using System;
using Xunit;
using task14;

namespace task14tests
{
    public class IntegralTests
    {
        [Fact]
        public void Solve_IntegralOfX_Minus1To1_ShouldBeZero()
        {
            var X = (double x) => x;

            double result = DefiniteIntegral.Solve(-1, 1, X, 1e-4, 2);

            Assert.Equal(0, result, 1e-4);
        }

        [Fact]
        public void Solve_IntegralOfSin_Minus1To1_ShouldBeZero()
        {
            var SIN = (double x) => Math.Sin(x);

            double result = DefiniteIntegral.Solve(-1, 1, SIN, 1e-5, 8);

            Assert.Equal(0, result, 1e-4);
        }

        [Fact]
        public void Solve_IntegralOfX_0To5_ShouldBe12_5()
        {
            var X = (double x) => x;

            double result = DefiniteIntegral.Solve(0, 5, X, 1e-6, 8);

            Assert.Equal(12.5, result, 1e-5);
        }

        [Fact]
        public void Solve_IntegralOfConstant_ShouldBeCorrect()
        {
            var constant = (double x) => 5.0;

            double result = DefiniteIntegral.Solve(0, 1, constant, 1e-4, 4);

            Assert.Equal(5.0, result, 1e-4);
        }

        [Fact]
        public void Solve_WithNegativeThreads_ThrowsException()
        {
            var X = (double x) => x;
            Assert.Throws<ArgumentException>(() => DefiniteIntegral.Solve(0, 1, X, 1e-4, -1));
        }

        [Fact]
        public void Solve_WithZeroThreads_ThrowsException()
        {
            var X = (double x) => x;
            Assert.Throws<ArgumentException>(() => DefiniteIntegral.Solve(0, 1, X, 1e-4, 0));
        }

        [Fact]
        public void Solve_WithInvalidBounds_ThrowsException()
        {
            var X = (double x) => x;
            Assert.Throws<ArgumentException>(() => DefiniteIntegral.Solve(5, 0, X, 1e-4, 2));
        }
    }
}