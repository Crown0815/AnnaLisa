using AnnaLisa.DataStructures;
using Shouldly;
using Xunit;

namespace AnnaLisa.Testing
{
    public class Analysis_data
    {
        [Fact]
        public void has_dimensional_units()
        {
            const string xUnit = "x";
            const string yUnit = "y";

            var analysisData = new AnalysisData(xUnit, yUnit);

            analysisData.XUnit.ShouldBe(xUnit);
            analysisData.YUnit.ShouldBe(yUnit);
        }

        [Fact]
        public void without_points_is_empty()
        {
            var analysisData = new AnalysisData();

            analysisData.IsEmpty.ShouldBeTrue();
        }

        [Fact]
        public void can_have_a_single_collection_of_points()
        {
            var points = new Point[] {(1, 1), (2, 2)};
            
            var analysisData = new AnalysisData(points);

            analysisData.PointSets.ShouldBe(new []{points});
        }

        [Fact]
        public void can_have_multiple_collections_of_points()
        {
            var points1 = new Point[] {(1, 1)};
            var points2 = new Point[] {(2, 2)};
            
            var analysisData = new AnalysisData(points1, points2);
            
            analysisData.PointSets.ShouldBe(new []{points1, points2});
        }

        [Fact]
        public void can_have_points_and_units()
        {
            const string xUnit = "x";
            const string yUnit = "y";
            var points = new Point[] {(1, 1), (2, 2)};
            
            var analysisData = new AnalysisData(xUnit, yUnit, points);

            analysisData.XUnit.ShouldBe(xUnit);
            analysisData.YUnit.ShouldBe(yUnit);
            analysisData.PointSets.ShouldBe(new []{points});
        }
        
        [Fact]
        public void can_have_units_and_multiple_collections_of_points()
        {
            const string xUnit = "x";
            const string yUnit = "y";
            var points1 = new Point[] {(1, 1)};
            var points2 = new Point[] {(2, 2)};
            
            var analysisData = new AnalysisData(xUnit, yUnit, points1, points2);

            analysisData.XUnit.ShouldBe(xUnit);
            analysisData.YUnit.ShouldBe(yUnit);
            analysisData.PointSets.ShouldBe(new []{points1, points2});
        }
    }
}