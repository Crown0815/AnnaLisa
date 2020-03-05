using System;

namespace AnnaLisa.Extensions
{
    public static class DoubleExtensions
    {
        public static int Exponent(this double value) => IsZero(value) ? 0 : (int)Math.Log10(value);

        private static bool IsZero(this double value) => Math.Abs(value) < double.Epsilon;

        public static double E(this int value, int exponent) => value * Math.Pow(10, exponent);

        public static bool IsWholeNumber(this double value) => Math.Abs(value % 1) < double.Epsilon;
    }
}