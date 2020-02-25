using System.Globalization;
using System.Threading;
using AnnaLisa.Formatting;
using Shouldly;
using Xunit;

namespace AnnaLisa.Testing.Formatting
{
    public class Value_formatter
    {
        private const double ToleranceFactor = 1e-3; // 1‰
        private static double ToleranceFor(double value) => ToleranceFactor * value + double.Epsilon;
        
        [Theory]
        // special cases for 0
        [InlineData(0, 1, 0, 1, 0)]
        
        // exponential range
        [InlineData(1e-9, 1, 1e0, 1e+9, -9)]
        [InlineData(1e-8, 1, 1e1, 1e+9, -9)]
        [InlineData(1e-7, 1, 1e2, 1e+9, -9)]
        [InlineData(1e-6, 1, 1e0, 1e+6, -6)]
        [InlineData(1e-5, 1, 1e1, 1e+6, -6)]
        [InlineData(1e-4, 1, 1e2, 1e+6, -6)]
        [InlineData(1e-3, 1, 1e0, 1e+3, -3)]
        [InlineData(1e-2, 1, 1e1, 1e+3, -3)]
        [InlineData(1e-1, 1, 1e2, 1e+3, -3)]
        [InlineData(1e+0, 1, 1e0, 1e+0, +0)]
        [InlineData(1e+1, 1, 1e1, 1e+0, +0)]
        [InlineData(1e+2, 1, 1e2, 1e+0, +0)]
        [InlineData(1e+3, 1, 1e0, 1e-3, +3)]
        [InlineData(1e+4, 1, 1e1, 1e-3, +3)]
        [InlineData(1e+5, 1, 1e2, 1e-3, +3)]
        [InlineData(1e+6, 1, 1e0, 1e-6, +6)]
        [InlineData(1e+7, 1, 1e1, 1e-6, +6)]
        [InlineData(1e+8, 1, 1e2, 1e-6, +6)]
        [InlineData(1e+9, 1, 1e0, 1e-9, +9)]
        
        // linear range (except 0 and 1, since they were handled above)
        [InlineData(2e+9, 1, 2e0, 1e-9, +9)]
        [InlineData(3e+9, 1, 3e0, 1e-9, +9)]
        [InlineData(4e+9, 1, 4e0, 1e-9, +9)]
        [InlineData(5e+9, 1, 5e0, 1e-9, +9)]
        [InlineData(6e+9, 1, 6e0, 1e-9, +9)]
        [InlineData(7e+9, 1, 7e0, 1e-9, +9)]
        [InlineData(8e+9, 1, 8e0, 1e-9, +9)]
        [InlineData(9e+9, 1, 9e0, 1e-9, +9)]
        [InlineData(2e-9, 1, 2e0, 1e+9, -9)]
        [InlineData(3e-9, 1, 3e0, 1e+9, -9)]
        [InlineData(4e-9, 1, 4e0, 1e+9, -9)]
        [InlineData(5e-9, 1, 5e0, 1e+9, -9)]
        [InlineData(6e-9, 1, 6e0, 1e+9, -9)]
        [InlineData(7e-9, 1, 7e0, 1e+9, -9)]
        [InlineData(8e-9, 1, 8e0, 1e+9, -9)]
        [InlineData(9e-9, 1, 9e0, 1e+9, -9)]
        public void formats_with_standard_exponent_step(double value, double andError, double toExpectedValue, double andExpectedError, int withSharedExponent)
        {
            var valueFormatter = new ValueFormatter();

            var formattedValue = valueFormatter.Format(value, andError);
            
            formattedValue.Value.ShouldBe(toExpectedValue, ToleranceFor(toExpectedValue));
            formattedValue.Error.ShouldBe(andExpectedError, ToleranceFor(andExpectedError));
            formattedValue.Exponent.ShouldBe(withSharedExponent);
        }

        [Theory]
        // exponential range with 4 as exponent step
        [InlineData(1e-8, 1, 4, 1e0, 1e+8, -8)]
        [InlineData(1e-7, 1, 4, 1e1, 1e+8, -8)]
        [InlineData(1e-6, 1, 4, 1e2, 1e+8, -8)]
        [InlineData(1e-5, 1, 4, 1e3, 1e+8, -8)]
        [InlineData(1e-4, 1, 4, 1e0, 1e+4, -4)]
        [InlineData(1e-3, 1, 4, 1e1, 1e+4, -4)]
        [InlineData(1e-2, 1, 4, 1e2, 1e+4, -4)]
        [InlineData(1e-1, 1, 4, 1e3, 1e+4, -4)]
        [InlineData(1e+0, 1, 4, 1e0, 1e-0, +0)]
        [InlineData(1e+1, 1, 4, 1e1, 1e-0, +0)]
        [InlineData(1e+2, 1, 4, 1e2, 1e-0, +0)]
        [InlineData(1e+3, 1, 4, 1e3, 1e-0, +0)]
        [InlineData(1e+4, 1, 4, 1e0, 1e-4, +4)]
        [InlineData(1e+5, 1, 4, 1e1, 1e-4, +4)]
        [InlineData(1e+6, 1, 4, 1e2, 1e-4, +4)]
        [InlineData(1e+7, 1, 4, 1e3, 1e-4, +4)]
        [InlineData(1e+8, 1, 4, 1e0, 1e-8, +8)]
        [InlineData(1e+9, 1, 4, 1e1, 1e-8, +8)]
        
        // exponential range with 2 as exponent step
        [InlineData(1e-8, 1, 2, 1e0, 1e+8, -8)]
        [InlineData(1e-7, 1, 2, 1e1, 1e+8, -8)]
        [InlineData(1e-6, 1, 2, 1e0, 1e+6, -6)]
        [InlineData(1e-5, 1, 2, 1e1, 1e+6, -6)]
        [InlineData(1e-4, 1, 2, 1e0, 1e+4, -4)]
        [InlineData(1e-3, 1, 2, 1e1, 1e+4, -4)]
        [InlineData(1e-2, 1, 2, 1e0, 1e+2, -2)]
        [InlineData(1e-1, 1, 2, 1e1, 1e+2, -2)]
        [InlineData(1e+0, 1, 2, 1e0, 1e+0, +0)]
        [InlineData(1e+1, 1, 2, 1e1, 1e+0, +0)]
        [InlineData(1e+2, 1, 2, 1e0, 1e-2, +2)]
        [InlineData(1e+3, 1, 2, 1e1, 1e-2, +2)]
        [InlineData(1e+4, 1, 2, 1e0, 1e-4, +4)]
        [InlineData(1e+5, 1, 2, 1e1, 1e-4, +4)]
        [InlineData(1e+6, 1, 2, 1e0, 1e-6, +6)]
        [InlineData(1e+7, 1, 2, 1e1, 1e-6, +6)]
        [InlineData(1e+8, 1, 2, 1e0, 1e-8, +8)]
        [InlineData(1e+9, 1, 2, 1e1, 1e-8, +8)]
        public void formats_(double value, double andError, int withExponentStep, double toExpectedValue, double andExpectedError, int withSharedExponent)
        {
            var valueFormatter = new ValueFormatter();

            var formattedValue = valueFormatter.Format(value, andError, withExponentStep);
            
            formattedValue.Value.ShouldBe(toExpectedValue, ToleranceFor(toExpectedValue));
            formattedValue.Error.ShouldBe(andExpectedError, ToleranceFor(andExpectedError));
            formattedValue.Exponent.ShouldBe(withSharedExponent);
        }
        
        public static TheoryData<double, double, int, CultureInfo, string> ErrorStringTestData()
        {
            return new TheoryData<double, double, int, CultureInfo, string>
            {
                // values with zero exponent
                {1E0, 1, 3, new CultureInfo("en-us", false), "1.00 ± 1.00"},
                {1E0, 1, 3, new CultureInfo("de-de", false), "1,00 ± 1,00"},
                
                // values with negative exponent
                {1E-1, 1, 3, new CultureInfo("en-us", false), "100 ± 1,000 E-3"},
                {1E-1, 1, 3, new CultureInfo("de-de", false), "100 ± 1.000 E-3"},
                {1E-2, 1, 3, new CultureInfo("en-us", false), "10.0 ± 1,000.0 E-3"},
                {1E-2, 1, 3, new CultureInfo("de-de", false), "10,0 ± 1.000,0 E-3"},
                {1E-3, 1, 3, new CultureInfo("en-us", false), "1.00 ± 1,000.00 E-3"},
                {1E-3, 1, 3, new CultureInfo("de-de", false), "1,00 ± 1.000,00 E-3"},
                {1E-4, 1, 3, new CultureInfo("en-us", false), "100 ± 1,000,000 E-6"},
                {1E-4, 1, 3, new CultureInfo("de-de", false), "100 ± 1.000.000 E-6"},
                {1E-5, 1, 3, new CultureInfo("en-us", false), "10.0 ± 1,000,000.0 E-6"},
                {1E-5, 1, 3, new CultureInfo("de-de", false), "10,0 ± 1.000.000,0 E-6"},
                
                // values with positive exponent
                {1E1, 1, 3, new CultureInfo("en-us", false), "10.0 ± 1.0"},
                {1E1, 1, 3, new CultureInfo("de-de", false), "10,0 ± 1,0"},
                {1E2, 1, 3, new CultureInfo("en-us", false), "100 ± 1"},
                {1E2, 1, 3, new CultureInfo("de-de", false), "100 ± 1"},
                {1E3, 1, 3, new CultureInfo("en-us", false), "1.00 ± 0.00 E+3"},
                {1E3, 1, 3, new CultureInfo("de-de", false), "1,00 ± 0,00 E+3"},
                {1E4, 1, 3, new CultureInfo("en-us", false), "10.0 ± 0.0 E+3"},
                {1E4, 1, 3, new CultureInfo("de-de", false), "10,0 ± 0,0 E+3"},
                {1E5, 1, 3, new CultureInfo("en-us", false), "100 ± 0 E+3"},
                {1E5, 1, 3, new CultureInfo("de-de", false), "100 ± 0 E+3"},
                
                // formatting with four significant digits
                {1E0, 1, 4, new CultureInfo("en-us", false), "1.000 ± 1.000"},
                {1E1, 1, 4, new CultureInfo("en-us", false), "10.00 ± 1.00"},
                {1E2, 1, 4, new CultureInfo("en-us", false), "100.0 ± 1.0"},
                {1E3, 1, 4, new CultureInfo("en-us", false), "1.000 ± 0.001 E+3"},
                {1E4, 1, 4, new CultureInfo("en-us", false), "10.00 ± 0.00 E+3"},
                {1E5, 1, 4, new CultureInfo("en-us", false), "100.0 ± 0.0 E+3"},
                {1E-1, 1, 4, new CultureInfo("en-us", false), "100.0 ± 1,000.0 E-3"},
                {1E-2, 1, 4, new CultureInfo("en-us", false), "10.00 ± 1,000.00 E-3"},
                {1E-3, 1, 4, new CultureInfo("en-us", false), "1.000 ± 1,000.000 E-3"},
                {1E-4, 1, 4, new CultureInfo("en-us", false), "100.0 ± 1,000,000.0 E-6"},
                {1E-5, 1, 4, new CultureInfo("en-us", false), "10.00 ± 1,000,000.00 E-6"},
                
                // formatting with two significant digits
                {1E0, 1, 2, new CultureInfo("en-us", false), "1.0 ± 1.0"},
                {1E1, 1, 2, new CultureInfo("en-us", false), "10 ± 1"},
                {1E2, 1, 2, new CultureInfo("en-us", false), "100 ± 1"},
                {1E3, 1, 2, new CultureInfo("en-us", false), "1.0 ± 0.0 E+3"},
                {1E4, 1, 2, new CultureInfo("en-us", false), "10 ± 0 E+3"},
                {1E5, 1, 2, new CultureInfo("en-us", false), "100 ± 0 E+3"},
                {1E-1, 1, 2, new CultureInfo("en-us", false), "100 ± 1,000 E-3"},
                {1E-2, 1, 2, new CultureInfo("en-us", false), "10 ± 1,000 E-3"},
                {1E-3, 1, 2, new CultureInfo("en-us", false), "1.0 ± 1,000.0 E-3"},
                {1E-4, 1, 2, new CultureInfo("en-us", false), "100 ± 1,000,000 E-6"},
                {1E-5, 1, 2, new CultureInfo("en-us", false), "10 ± 1,000,000 E-6"},
            };
        }

        [Theory]
        [MemberData(nameof(ErrorStringTestData))]
        public void string_for_formatted(double value, double andError, int with, CultureInfo significantDigitsInCulture, string returnsExpected)
        {
            var valueFormatter = new ValueFormatter();
            Thread.CurrentThread.CurrentCulture = significantDigitsInCulture;

            var formattedValueString = valueFormatter.Format(value, andError).String(with);
            
            formattedValueString.ShouldBe(returnsExpected);
        }
    }
}