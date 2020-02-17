using System.Linq;
using Shouldly;
using Xunit;

namespace AnnaLisa.Testing.Operations
{
    public abstract class OperationTestBase
    {
        protected abstract IAnalysisOperation DataModifyingOperation();
        
        [Fact]
        public void leaves_original_data_unchanged()
        {
            var points = new Point[]{(1, 1), (2, 2)};
            var dataSource = new DataSource(points);
            
            var node = new AnalysisNode();
            var operationUnderTest = DataModifyingOperation();
            node.AddSource(dataSource);
            
            node.Set(operationUnderTest);

            dataSource.Data.PointSets.Single().ShouldBe(new Point[]{(1, 1), (2, 2)});
        }
    }
}