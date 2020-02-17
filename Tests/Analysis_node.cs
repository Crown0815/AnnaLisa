using System.Linq;
using NSubstitute;
using Shouldly;
using Xunit;

namespace AnnaLisa.Testing
{
    public class Analysis_node
    {
        [Fact]
        public void new_instance_has_empty_data()
        {
            var analysisNode = new AnalysisNode();

            analysisNode.Data.IsEmpty.ShouldBeTrue();
        }

        [Fact]
        public void new_instance_returns_same_empty_data_for_multiple_requests()
        {
            var analysisNode = new AnalysisNode();

            analysisNode.Data.ShouldBe(analysisNode.Data);
        }

        [Fact]
        public void with_single_data_source_and_no_operations_forwards_data_from_data_source()
        {
            var analysisNode = new AnalysisNode();
            var dataSource = new DataSource();
            var expected = dataSource.Data;
            
            analysisNode.AddSource(dataSource);

            analysisNode.Data.ShouldBe(expected);
        }
        
        [Fact]
        public void with_single_data_source_and_operations_exposes_operation_result()
        {
            var analysisNode = new AnalysisNode();
            var dataSource = new DataSource();
            var data = dataSource.Data;
            var operation = Substitute.For<IAnalysisOperation>();
            var operationResult = Substitute.For<IAnalysisData>();
            operation.Perform(data).Returns(operationResult);
            
            analysisNode.AddSource(dataSource);
            analysisNode.Set(operation);

            analysisNode.Data.ShouldBe(operationResult);
        }

        [Fact]
        public void with_multiple_data_sources_and_no_operations_exposes_accumulated_data_from_all_data_sources()
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
        public void with_multiple_data_sources_and_no_operations_preserves_units_from_from_sources()
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
        public void with_multiple_empty_data_sources_and_no_operations_exposes_empty_data_source()
        {
            var analysisNode = new AnalysisNode();
            var dataSource1 = new DataSource();
            var dataSource2 = new DataSource();

            analysisNode.AddSource(dataSource1);
            analysisNode.AddSource(dataSource2);

            analysisNode.Data.IsEmpty.ShouldBeTrue();
        }

        [Fact]
        public void can_be_used_as_data_source_for_other_analysis_node()
        {
            var dataSource = new DataSource(new Point[] {(0, 0), (0, 0)});
            var childNode = new AnalysisNode();
            var parentNode = new AnalysisNode();
            
            childNode.AddSource(dataSource);

            parentNode.AddSource(childNode);

            parentNode.Data.PointSets.Single().ShouldBe(new Point[] {(0, 0), (0, 0)});
        }
    }
}
