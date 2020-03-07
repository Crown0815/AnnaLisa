using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using AnnaLisa.DataStructures;

namespace AnnaLisa.Sources
{
    public class CsvSource
    {
        private readonly string _separator;
        private int _columnCount;

        public CsvSource(string filePath, string separator = null)
        {
            _separator = separator ?? DefaultSeparator();
            var fileReader = new StreamReader(filePath);
            Data = Read(fileReader);
        }

        private static string DefaultSeparator() => CultureInfo.CurrentCulture.TextInfo.ListSeparator;

        private IAnalysisData Read(StreamReader reader)
        {
            var firstLine = reader.ReadLine();
            var (xUnit, yUnit) = UnitsFrom(firstLine);
            
            if (firstLine is null)
                return new AnalysisData(xUnit, yUnit);
            
            var points = new List<Point>();
            var counter = 0;
            while (!reader.EndOfStream) points.Add(PointFrom(reader.ReadLine(), counter++));

            return new AnalysisData(xUnit, yUnit, points);
        }

        private (double, double) PointFrom(string line, int index)
        {
            var pointStrings = Split(line);
            if (pointStrings.Length != _columnCount)
                throw new DataException($"Inconsistent number of columns, expected {_columnCount} " +
                                        $"but got {pointStrings.Length}");
            
            return _columnCount == 2 
                ? (double.Parse(pointStrings[0]), double.Parse(pointStrings[1])) 
                : (index, double.Parse(pointStrings[0]));
        }

        private (string, string) UnitsFrom(string firstLine)
        {
            if (firstLine is null) return (string.Empty, string.Empty);
            
            var labels = Split(firstLine);
            _columnCount = labels.Length;
            
            return _columnCount == 2 
                ? (UnitFrom(labels[0]), UnitFrom(labels[1])) 
                : (string.Empty, UnitFrom(labels[0]));
        }
        
        private string[] Split(string line) => line.Split(new []{_separator}, StringSplitOptions.None);

        private static string UnitFrom(string label)
        {
            var regex = new Regex(@".*\((.*)\)");
            return regex.Match(label).Groups[1].Value;
        }

        public IAnalysisData Data { get; }
    }
}