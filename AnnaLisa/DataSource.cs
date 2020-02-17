using System.Collections.Generic;
using AnnaLisa.DataStructures;

namespace AnnaLisa
{
    public class DataSource : IDataSource
    {
        public DataSource(IReadOnlyCollection<Point> points)
        {
            Data = new AnalysisData(points);
        }
        
        public DataSource(string xUnit = null, string yUnit = null, params IReadOnlyCollection<Point>[] pointSets)
        {
            Data = new AnalysisData(xUnit, yUnit, pointSets);
        }

        public IAnalysisData Data { get; }
    }
}