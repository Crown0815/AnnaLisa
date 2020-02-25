using System;

namespace AnnaLisa.Extensions
{
    public static class DoubleExtensions
    {
        public static int Exponent(this double value) => IsZero(value) ? 0 : (int)Math.Log10(value);

        private static bool IsZero(double value) => Math.Abs(value) < double.Epsilon;
    }
}