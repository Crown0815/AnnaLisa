using Shouldly;
using Xunit;

namespace AnnaLisa.Testing
{
    public class Analysis_node_with_multiple_data_sources_and_no_operations
    {
        [Fact]
        public void exposes_accumulated_data_from_all_data_sources()
        {
            var analysisNode = new AnalysisNode();

            var points1 = new Point[] {(0, 0), (1, 1)};
            var points2 = new Point[] {(2, 2), (3, 3)};
            var dataSource1 = new DataSource(points1);
            var dataSource2 = new DataSource(points2);
            

            analysisNode.AddSource(dataSource1);
            analysisNode.AddSource(dataSource2);

            analysisNode.Data.PointSets[0].ShouldBe(points1);
            analysisNode.Data.PointSets[1].ShouldBe(points2);
            analysisNode.Data.PointSets.Count.ShouldBe(2);
        }
        
        [Fact]
        public void preserves_units_from_from_sources()
        {
            const string xUnit = "x";
            const string yUnit = "y";
            
            var analysisNode = new AnalysisNode();
            var dataSource1 = new DataSource(xUnit, yUnit);
            var dataSource2  = new DataSource(xUnit, yUnit);

            analysisNode.AddSource(dataSource1);
            analysisNode.AddSource(dataSource2);

            analysisNode.Data.XUnit.ShouldBe(xUnit);
            analysisNode.Data.YUnit.ShouldBe(yUnit);
        }
        
        [Fact]
        public void where_all_sources_are_empty_exposes_empty_data_source()
        {
            var analysisNode = new AnalysisNode();
            var dataSource1 = new DataSource();
            var dataSource2 = new DataSource();

            analysisNode.AddSource(dataSource1);
            analysisNode.AddSource(dataSource2);

            analysisNode.Data.IsEmpty.ShouldBeTrue();
        }
    }
}