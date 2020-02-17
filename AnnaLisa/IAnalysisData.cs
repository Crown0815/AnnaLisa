using System.Collections.Generic;

namespace AnnaLisa
{
    public interface IAnalysisData
    {
        bool IsEmpty { get; }
        object XUnit { get; }
        object YUnit { get; }
        IReadOnlyList<IReadOnlyCollection<Point>> PointSets { get; }
    }

    public interface IAccumulatedAnalysisData : IAnalysisData
    {
        IReadOnlyList<IAnalysisData> SourceData { get; }
    }
}