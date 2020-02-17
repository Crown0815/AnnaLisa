using System;
using System.Collections.Generic;
using Shouldly;
using Xunit;

namespace AnnaLisa.Testing
{
    public class Enumerable
    {
        [Fact]
        public void with_null_value_assigned_is_null_or_empty()
        {
            ((IEnumerable<object>?) null).IsNullOrEmpty().ShouldBeTrue();
        }
        
        [Fact]
        public void without_elements_is_null_or_empty()
        {
            IEnumerable<object> emptyEnumerable = new object[0];
            
            emptyEnumerable.IsNullOrEmpty().ShouldBeTrue();
        }

        [Fact]
        public void with_shared_items_property_returns_shared_property_on_call_to_Shared()
        {
            var item1 = new { Shared = 1, NotShared = 2 };
            var item2 = new { Shared = 1, NotShared = 3 };
            var items = new []{item1, item2};
            
            items.Shared(x => x.Shared).ShouldBe(1);
        }
        
        [Fact]
        public void with_unshared_items_property_throws_invalid_operation_exception_on_call_to_Shared()
        {
            var item1 = new { Shared = 1, NotShared = 2 };
            var item2 = new { Shared = 1, NotShared = 3 };
            var items = new []{item1, item2};
            
            Action callToShared = () => items.Shared(x => x.NotShared);

            callToShared.ShouldThrow<InvalidOperationException>();
        }
    }
}