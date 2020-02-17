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

        private static (IDataSource, IAnalysisData) TestDataSource()
        {
            var dataSource = Substitute.For<IDataSource>();
            var data = Substitute.For<IAnalysisData>();
            dataSource.Data.Returns(data);
            return (dataSource, data);
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
            var (dataSource1, data1) = TestDataSource();
            var (dataSource2, data2) = TestDataSource();

            analysisNode.AddSource(dataSource1);
            analysisNode.AddSource(dataSource2);

            analysisNode.Data.ShouldBeAssignableTo<IAccumulatedAnalysisData>();

            var accumulated = (IAccumulatedAnalysisData)analysisNode.Data;
            accumulated.SourceData.ShouldBe(new []{data1, data2});
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
        public void can_be_used_as_data_source_for_other_analysis_node()
        {
            var childNode = new AnalysisNode();
            var parentNode = new AnalysisNode();
            var (dataSource, data) = TestDataSource();
            childNode.AddSource(dataSource);

            parentNode.AddSource(childNode);

            parentNode.Data.ShouldBe(data);
        }
    }
}
