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
            var operation = Substitute.For<IAnalysisOperation>();
            var operationResult = Substitute.For<IAnalysisData>();
            operation.Perform(dataSource.Data).Returns(operationResult);
            
            analysisNode.AddSource(dataSource);
            analysisNode.Set(operation);

            analysisNode.Data.ShouldBe(operationResult);
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
