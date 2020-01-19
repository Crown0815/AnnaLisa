using System;
using Xunit;
using Shouldly;
using NSubstitute;
using AnnaLisa;

namespace AnnaLisa.Testing
{
    public class Analysis_node
    {
        [Fact]
        public void new_instance_has_no_data()
        {
            var analysisNode = new AnalysisNode();

            analysisNode.Data.ShouldBeNull();
            analysisNode.HasData.ShouldBeFalse();
        }

        [Fact]
        public void new_instance_has_completed_calculation_task()
        {
            var analysisNode = new AnalysisNode();

            analysisNode.Calculation.IsCompleted.ShouldBeTrue();
        }

        [Fact]
        public void with_single_data_source_and_no_operations_has_same_data_as_data_source()
        {
            var analysisNode = new AnalysisNode();
            var dataSource = Substitute.For<IDataSource>();
            var data = new object();
            dataSource.Data.Returns(data);
            
            analysisNode.AddSource(dataSource);

            analysisNode.Data.ShouldBe(data);
            analysisNode.HasData.ShouldBeTrue();
        }
    }
}
