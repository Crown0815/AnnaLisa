using System;
using System.Collections.Generic;
using AnnaLisa.Extensions;
using Shouldly;
using Xunit;

namespace AnnaLisa.Testing.Extensions
{
    public class Double
    {
        private static IEnumerable<double> WholeNumberRange()
        {
            for (var exponent = 0; exponent < 9; exponent++)
            for (var integer = -9; integer < 9; integer++)
                yield return integer.E(exponent);
        }
        
        public static TheoryData<double, bool> IsWholeNumberTheoryData()
        {
            var data = new TheoryData<double, bool>();
            foreach (var number in WholeNumberRange())
            {
                data.Add(number,           true);
                data.Add(number + Math.PI, false);
            }

            return data;
        }
        [Theory]
        [MemberData(nameof(IsWholeNumberTheoryData))]
        public void is_whole_number_for(double value, bool returns)
        {
            value.IsWholeNumber().ShouldBe(returns);
        }
        
        [Theory]
        [InlineData(9, 1, 1e9)]
        [InlineData(8, 2, 2e8)]
        [InlineData(7, 3, 3e7)]
        [InlineData(6, 4, 4e6)]
        [InlineData(5, 5, 5e5)]
        [InlineData(4, 6, 6e4)]
        [InlineData(3, 7, 7e3)]
        [InlineData(2, 8, 8e2)]
        [InlineData(1, 9, 9e1)]
        public void E(int exponent, int ofValue, double returns)
        {
            ofValue.E(exponent).ShouldBe(returns);
        }
    }
}