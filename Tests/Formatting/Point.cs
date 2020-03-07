using System.Globalization;
using System.Threading;
using Shouldly;
using Xunit;

namespace AnnaLisa.Testing.Formatting
{
    public class Analysis_point
    {
        [Theory]
        [InlineData("en-us", "(1.1, 5.3)")]
        [InlineData("de-de", "(1,1. 5,3)")]
        public void ToString_(string inCulture, string returns)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(inCulture);
            var point = new Point(1.1, 5.3);

            var result = point.ToString();

            result.ShouldBe(returns);
        }
    }
}