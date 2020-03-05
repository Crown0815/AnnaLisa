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
        
        public static TheoryData<double> WholeNumbersTheoryData()
        {
            var data = new TheoryData<double>();
            foreach (var wholeNumber in WholeNumberRange()) 
                data.Add(wholeNumber);

            return data;
        }
        
        [Theory]
        [MemberData(nameof(WholeNumbersTheoryData))]
        public void is_whole_number_returns_true_for(double value)
        {
            value.IsWholeNumber().ShouldBeTrue();
        }
        
        
        public static TheoryData<double> RealNumbersTheoryData()
        {
            var data = new TheoryData<double>();
            foreach (var wholeNumber in WholeNumberRange()) 
                data.Add(wholeNumber + Math.PI);

            return data;
        }
        
        [Theory]
        [MemberData(nameof(RealNumbersTheoryData))]
        public void is_whole_number_returns_false_for(double value)
        {
            value.IsWholeNumber().ShouldBeFalse();
        }
    }
}