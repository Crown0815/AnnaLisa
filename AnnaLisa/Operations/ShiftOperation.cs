using System.Collections.Generic;
using System.Linq;
using AnnaLisa.DataStructures;

namespace AnnaLisa.Operations
{
    public class ShiftOperation : IAnalysisOperation
    {
        public IAnalysisData Perform(IAnalysisData data)
        {
            var points = data.PointSets.Select(Shifted).ToArray();
            return new AnalysisData(data.XUnit, data.YUnit, points);
        }

        private IReadOnlyCollection<Point> Shifted(IReadOnlyCollection<Point> points)
        {
            return points.Select(p => new Point(p.X + XShift, p.Y + YShift)).ToList();
        }

        public int XShift { get; set; }
        public int YShift { get; set; }
    }
}