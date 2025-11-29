using System;
using System.Collections.Generic;
using System.Text;

namespace MvvmCalculator.Models
{
    public class CalculatorModel
    {
        public double Calculate(double a, double b, string op)
        {
            return op switch
            {
                "+" => a + b,
                "-" => a - b,
                "*" => a * b,
                "/" => b == 0 ? 0 : a / b,
                _ => 0
            };
        }
    }
}
