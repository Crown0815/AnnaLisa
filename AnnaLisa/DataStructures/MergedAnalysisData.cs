using System.Collections.Generic;
using System.Linq;
using AnnaLisa.Extensions;

namespace AnnaLisa.DataStructures
{
    internal class MergedAnalysisData : IAccumulatedAnalysisData
    {
        public MergedAnalysisData(params IAnalysisData[] sourceData)
        {
            SourceData = sourceData.ToList();
            XUnit = sourceData.Shared(x => x.XUnit);
            YUnit = sourceData.Shared(x => x.YUnit);
            IsEmpty = sourceData.All(data => data.IsEmpty);

            PointSets = SourceData.SelectMany(x => x.PointSets).ToList();
        }

        public IReadOnlyList<IAnalysisData> SourceData { get; }
        public bool IsEmpty { get; }
        public object XUnit { get; }
        public object YUnit { get; }

        public IReadOnlyList<IReadOnlyCollection<Point>> PointSets { get; }
    }
}