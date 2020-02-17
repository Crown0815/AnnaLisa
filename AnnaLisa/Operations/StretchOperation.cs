using System.Collections.Generic;
using System.Linq;
using AnnaLisa.DataStructures;

namespace AnnaLisa.Operations
{
    public class StretchOperation : IAnalysisOperation
    {
        public IAnalysisData Perform(IAnalysisData data)
        {
            var points = data.PointSets.Select(Shifted).ToArray();
            return new AnalysisData(data.XUnit, data.YUnit, points);
        }
        
        private IReadOnlyCollection<Point> Shifted(IReadOnlyCollection<Point> points)
        {
            return points.Select(p => new Point(p.X * XStretch, p.Y * YStretch)).ToList();
        }

        public double XStretch { get; set; } = 1;
        public int YStretch { get; set; } = 1;
    }
}