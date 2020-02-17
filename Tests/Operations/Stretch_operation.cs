using System.Linq;
using AnnaLisa.Operations;
using Shouldly;
using Xunit;

namespace AnnaLisa.Testing.Operations
{
    public class Stretch_operation : OperationTestBase
    {
        protected override IAnalysisOperation DataModifyingOperation() => new ShiftOperation{XShift = 2, YShift = 2};

        [Fact]
        public void multiplies_all_x_values_by_x_stretch_value()
        {
            var points = new Point[]{(1, 1), (2, 2)};
            var dataSource = new DataSource(points);
            
            var node = new AnalysisNode();
            var stretch = new StretchOperation {XStretch = 5};
            node.AddSource(dataSource);
            
            node.Set(stretch);

            node.Data.PointSets.Single().ShouldBe(new Point[]{(5, 1), (10, 2)});
        }
        
        [Fact]
        public void multiplies_all_y_values_by_y_stretch_value()
        {
            var points = new Point[]{(1, 1), (2, 2)};
            var dataSource = new DataSource(points);
            
            var node = new AnalysisNode();
            var stretch = new StretchOperation {YStretch = 5};
            node.AddSource(dataSource);
            
            node.Set(stretch);

            node.Data.PointSets.Single().ShouldBe(new Point[]{(1, 5), (2, 10)});
        }
        
        [Fact]
        public void multiplies_all_x_and_y_data_values_by_x_and_y_stretch_respectively()
        {
            var points = new Point[]{(1, 1), (2, 2)};
            var dataSource = new DataSource(points);
            
            var node = new AnalysisNode();
            var stretch = new StretchOperation() {XStretch = 3, YStretch = 2};
            node.AddSource(dataSource);
            
            node.Set(stretch);

            node.Data.PointSets.Single().ShouldBe(new Point[]{(3, 2), (6, 4)});
        }
        
        [Fact]
        public void multiplies_values_in_all_point_sets()
        {
            var dataSource1 = new DataSource(new Point[]{(1, 1), (2, 2)});
            var dataSource2 = new DataSource(new Point[]{(2, 2), (3, 3)});
            
            var node = new AnalysisNode();
            var stretch = new StretchOperation() {XStretch = 3, YStretch = 2};
            node.AddSource(dataSource1);
            node.AddSource(dataSource2);
            
            node.Set(stretch);

            node.Data.PointSets[0].ShouldBe(new Point[]{(3, 2), (6, 4)});
            node.Data.PointSets[1].ShouldBe(new Point[]{(6, 4), (9, 6)});
        }
        
        [Fact]
        public void without_x_or_y_stretch_unit_preserves_original_units()
        {
            const string xUnit = "x";
            const string yUnit = "y";
            var dataSource = new DataSource(xUnit, yUnit);
            
            var node = new AnalysisNode();
            node.AddSource(dataSource);
            var stretch = new StretchOperation();
            
            node.Set(stretch);

            node.Data.XUnit.ShouldBe(xUnit);
            node.Data.YUnit.ShouldBe(yUnit);
        }
    }
}