using System.Collections.Generic;

namespace AnnaLisa.DataStructures
{
    public class AnalysisData : IAnalysisData
    {
        public static AnalysisData Empty() => new AnalysisData();
        
        public IReadOnlyList<IReadOnlyCollection<Point>> PointSets { get; }
        public object XUnit { get; }
        public object YUnit { get; }
        public bool IsEmpty => PointSets.IsNullOrEmpty();

        public AnalysisData(params IReadOnlyCollection<Point>[] points) : this(null, null, points) {}
        
        public AnalysisData(object xUnit, object yUnit, params IReadOnlyCollection<Point>[] pointSets)
        {
            XUnit = xUnit;
            YUnit = yUnit;
            PointSets = pointSets;
        }
    }
}