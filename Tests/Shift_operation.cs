using System.Linq;
using AnnaLisa.Operations;
using Shouldly;
using Xunit;

namespace AnnaLisa.Testing
{
    public class Shift_operation
    {
        [Fact]
        public void shifts_all_x_data_values_by_x_shift()
        {
            var points = new Point[]{(1, 1), (2, 2)};
            var dataSource = new DataSource(points);
            
            var node = new AnalysisNode();
            var shift = new ShiftOperation {XShift = 5};
            node.AddSource(dataSource);
            
            node.Set(shift);

            node.Data.PointSets.Single().ShouldBe(new Point[]{(6, 1), (7, 2)});
        }
        
        [Fact]
        public void shifts_all_y_data_values_by_y_shift()
        {
            var points = new Point[]{(1, 1), (2, 2)};
            var dataSource = new DataSource(points);
            
            var node = new AnalysisNode();
            var shift = new ShiftOperation {YShift = 5};
            node.AddSource(dataSource);
            
            node.Set(shift);

            node.Data.PointSets.Single().ShouldBe(new Point[]{(1, 6), (2, 7)});
        }
        
        [Fact]
        public void shifts_all_x_and_y_data_values_by_x_and_y_shift_respectively()
        {
            var points = new Point[]{(1, 1), (2, 2)};
            var dataSource = new DataSource(points);
            
            var node = new AnalysisNode();
            var shift = new ShiftOperation {XShift = 3, YShift = 2};
            node.AddSource(dataSource);
            
            node.Set(shift);

            node.Data.PointSets.Single().ShouldBe(new Point[]{(4, 3), (5, 4)});
        }
        
        [Fact]
        public void shifts_values_of_data_from_all_data_sources()
        {
            var dataSource1 = new DataSource(new Point[]{(1, 1), (2, 2)});
            var dataSource2 = new DataSource(new Point[]{(2, 2), (3, 3)});
            
            var node = new AnalysisNode();
            var shift = new ShiftOperation {XShift = 3, YShift = 2};
            node.AddSource(dataSource1);
            node.AddSource(dataSource2);
            
            node.Set(shift);

            node.Data.PointSets[0].ShouldBe(new Point[]{(4, 3), (5, 4)});
            node.Data.PointSets[1].ShouldBe(new Point[]{(5, 4), (6, 5)});
        }

        [Fact]
        public void preserves_original_units()
        {
            const string xUnit = "x";
            const string yUnit = "y";
            var dataSource = new DataSource(xUnit, yUnit);
            
            var node = new AnalysisNode();
            node.AddSource(dataSource);
            var shift = new ShiftOperation();
            
            node.Set(shift);

            node.Data.XUnit.ShouldBe(xUnit);
            node.Data.YUnit.ShouldBe(yUnit);
        }
        
        [Fact]
        public void performed_on_empty_data_returns_empty_data()
        {
            var node = new AnalysisNode();
            var shift = new ShiftOperation();
            
            node.Set(shift);

            node.Data.PointSets.ShouldBeEmpty();
            node.Data.XUnit.ShouldBeNull();
            node.Data.YUnit.ShouldBeNull();
        }

        [Fact]
        public void leaves_original_data_unchanged()
        {
            var points = new Point[]{(1, 1), (2, 2)};
            var dataSource = new DataSource(points);
            
            var node = new AnalysisNode();
            var shift = new ShiftOperation {XShift = 1, YShift = 1};
            node.AddSource(dataSource);
            
            node.Set(shift);

            dataSource.Data.PointSets.Single().ShouldBe(new Point[]{(1, 1), (2, 2)});
        }
    }
}