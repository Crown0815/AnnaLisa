using System.Collections.Generic;
using System.Linq;

namespace AnnaLisa
{
    public interface IAnalysisData
    {
        bool IsEmpty { get; }
        object XUnit { get; }
        object YUnit { get; }
    }

    public class AnalysisData : IAnalysisData
    {
        public IReadOnlyCollection<IReadOnlyCollection<Point>> PointSets { get; }
        public object XUnit { get; }
        public object YUnit { get; }
        public bool IsEmpty => PointSets.IsNullOrEmpty();

        public AnalysisData(params IReadOnlyCollection<Point>[] points) : this(null, null, points) {}
        
        public AnalysisData(string xUnit, string yUnit, params IReadOnlyCollection<Point>[] pointSets)
        {
            XUnit = xUnit;
            YUnit = yUnit;
            PointSets = pointSets;
        }
    }

    public interface IAccumulatedAnalysisData : IAnalysisData
    {
        IReadOnlyList<IAnalysisData> SourceData { get; }
    }

    internal class EmptyAnalysisData : IAnalysisData
    {
        public bool IsEmpty { get; } = true;
        public object XUnit { get; }
        public object YUnit { get; }
    }

    internal class MergedAnalysisData : IAccumulatedAnalysisData
    {
        public MergedAnalysisData(params IAnalysisData[] sourceData)
        {
            SourceData = sourceData.ToList();
            XUnit = sourceData.Shared(x => x.XUnit);
            YUnit = sourceData.Shared(x => x.YUnit);
            IsEmpty = !SourceData.Any();
        }

        public IReadOnlyList<IAnalysisData> SourceData { get; }
        public bool IsEmpty { get; }
        public object XUnit { get; }
        public object YUnit { get; }
    }
}