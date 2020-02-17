using NSubstitute;
using Shouldly;
using Xunit;

namespace AnnaLisa.Testing
{
    public class Analysis_node_with_single_data_source
    {
        [Fact]
        public void and_no_operations_forwards_data_from_data_source()
        {
            var analysisNode = new AnalysisNode();
            var dataSource = new DataSource();
            var expected = dataSource.Data;
            
            analysisNode.AddSource(dataSource);

            analysisNode.Data.ShouldBe(expected);
        }
        
        [Fact]
        public void and_an_operation_exposes_operation_result()
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
    }
}