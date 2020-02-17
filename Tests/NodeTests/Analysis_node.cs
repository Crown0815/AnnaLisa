using System.Linq;
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
    }
}
