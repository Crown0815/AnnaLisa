using System;
using AnnaLisa.Extensions;

namespace AnnaLisa.Formatting
{
    public class ValueFormatter
    {
        public FormattedValue Format(in double value, in double error, in int exponentStep = 3)
        {
            var exponent = value.Exponent();
            var shift = exponent % exponentStep;
            
            if (exponent < 0 && shift != 0) 
                exponent -= exponentStep;

            exponent -= shift;
            var factor = Math.Pow(10, exponent);
            return new FormattedValue(value/factor, error/factor, exponent);
        }
    }

    public readonly struct FormattedValue
    {
        public FormattedValue(double value, double error, int exponent) =>
            (Value, Error, Exponent) = (value, error, exponent);
        
        public double Value { get; }
        public double Error { get; }
        public int Exponent { get; }

        public string String(int significantDigits = 3)
        {
            var valueFormat = $"N{Math.Max(significantDigits - Value.Exponent() - 1, 0)}";
            var exponentString = Exponent == 0 ? "" : $" E{Exponent:+#;-#}";
            return $"{Value.ToString(valueFormat)} ± {Error.ToString(valueFormat)}{exponentString}";
        }
    }
}