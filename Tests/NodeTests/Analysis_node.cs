using System.Linq;
using NSubstitute;
using Shouldly;
using Xunit;

namespace AnnaLisa.Testing.NodeTests
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
        public void can_be_used_as_data_source_for_other_analysis_node()
        {
            var dataSource = new DataSource(new Point[] {(0, 0), (0, 0)});
            var childNode = new AnalysisNode();
            var parentNode = new AnalysisNode();
            
            childNode.AddSource(dataSource);

            parentNode.AddSource(childNode);

            parentNode.Data.PointSets.Single().ShouldBe(new Point[] {(0, 0), (0, 0)});
        }

        [Fact]
        public void evaluates_data_lazily()
        {
            var dataSource = new DataSource();
            var operation = Substitute.For<IAnalysisOperation>();
            var operationResult = Substitute.For<IAnalysisData>();
            operation.Perform(dataSource.Data).Returns(operationResult);
            var analysisNode = new AnalysisNode();
            
            analysisNode.AddSource(dataSource);
            analysisNode.Set(operation);

            operation.Received(0).Perform(Arg.Any<IAnalysisData>());
            analysisNode.Data.ShouldBe(operationResult);
        }
    }
}
