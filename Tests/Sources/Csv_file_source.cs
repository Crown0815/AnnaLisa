using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using AnnaLisa.Sources;
using AnnaLisa.Testing.CsvTestFiles;
using Shouldly;
using Xunit;

namespace AnnaLisa.Testing.Sources
{
    public class Csv_file_source
    {
        private static readonly string BasePath = Path.Join(".", "CsvTestFiles");
        
        [Fact]
        public void reads_data_from_csv_file_with_two_columns_into_analysis_data()
        {
            var file = Path.Join(BasePath, "TwoColumns.csv");
            
            var csvSource = new CsvSource(file, ",");

            csvSource.Data.XUnit.ShouldBe("x");
            csvSource.Data.YUnit.ShouldBe("y");
            csvSource.Data.PointSets.ShouldBe(new []{new Point[]{(0.1, 1), (0.2, 2), (0.3, 3)}});
        }
        
        [Fact]
        public void reads_data_from_csv_file_with_single_column_into_analysis_data_with_indices_as_x_values()
        {
            var file = Path.Join(BasePath, "SingleColumn.csv");
            
            var csvSource = new CsvSource(file, ",");

            csvSource.Data.XUnit.ShouldBe("");
            csvSource.Data.YUnit.ShouldBe("y");
            csvSource.Data.PointSets.ShouldBe(new []{new Point[]{(0, 0.1), (1, 0.2), (2, 0.3)}});
        }

        [Theory]
        [InlineData("a(x),b(y)"    , "x",   "y")]
        [InlineData("a ,b(y)"      , "",    "y")]
        [InlineData("a(x) ,b"      , "x",   "")]
        [InlineData("a(abc),b(def)", "abc", "def")]
        public void resolves_(string header, string toXUnit, string andYUnit)
        {
            using var testFile = new TestFile(BasePath, header);
            
            var csvSource = new CsvSource(testFile.FilePath, ",");

            csvSource.Data.XUnit.ShouldBe(toXUnit);
            csvSource.Data.YUnit.ShouldBe(andYUnit);
        }
        
        [Fact]
        public void resolves_empty_file_to_empty_analysis_data()
        {
            using var testFile = new TestFile(BasePath, "");
            
            var csvSource = new CsvSource(testFile.FilePath);

            csvSource.Data.XUnit.ShouldBe("");
            csvSource.Data.YUnit.ShouldBe("");
            csvSource.Data.PointSets.ShouldBe(new IReadOnlyCollection<Point>[0]);
            csvSource.Data.IsEmpty.ShouldBeTrue();
        }
        
        [Theory]
        [InlineData(",")]
        [InlineData(";")]
        [InlineData("\t")]
        [InlineData("|")]
        public void supports_files_with_(string separator)
        {
            using var testFile = new TestFile(BasePath, 
                "header1(a)" + separator + "header2(b)", 
                "0.1" + separator + "0.01", 
                "0.2" + separator + "0.02");
            
            var csvSource = new CsvSource(testFile.FilePath, separator);

            csvSource.Data.XUnit.ShouldBe("a");
            csvSource.Data.YUnit.ShouldBe("b");
            csvSource.Data.PointSets.ShouldBe(new []{new Point[]{(0.1, 0.01), (0.2, 0.02)}});
        }
        
        [Theory]
        [InlineData("de-de")]
        [InlineData("en-us")]
        public void defaults_to_current_culture_for_reading(string inCulture)
        {
            var culture = new CultureInfo(inCulture);
            var separator = culture.TextInfo.ListSeparator;
            Thread.CurrentThread.CurrentCulture = culture;
            
            using var testFile = new TestFile(BasePath, 
                "header1(a)" + separator + "header2(b)", 
                0.1 + separator + 0.01, 
                0.2 + separator + 0.02);
            
            var csvSource = new CsvSource(testFile.FilePath);

            csvSource.Data.XUnit.ShouldBe("a");
            csvSource.Data.YUnit.ShouldBe("b");
            csvSource.Data.PointSets.ShouldBe(new []{new Point[]{(0.1, 0.01), (0.2, 0.02)}});
        }

        [Fact]
        public void reading_file_with_different_column_count_in_rows_throws_exception()
        {
            using var testFile = new TestFile(BasePath, "header_with, two_columns", "0.1");

            // ReSharper disable once ObjectCreationAsStatement
            void Constructor() => new CsvSource(testFile.FilePath, ",");
            
            Should.Throw<DataException>(Constructor);
        }
    }
}