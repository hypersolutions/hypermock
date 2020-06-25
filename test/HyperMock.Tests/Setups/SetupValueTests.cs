using System;
using HyperMock.Setups;
using Shouldly;
using Xunit;

namespace HyperMock.Tests.Setups
{
    public class SetupValueTests
    {
        [Fact]
        public void CtorSetsValue()
        {
            var setupValue = new SetupValue(true);
            
            setupValue.Value.ShouldBe(true);
        }
        
        [Fact]
        public void CtorSetsValueAsExceptionNotToBeThrown()
        {
            var error = new Exception();
            var setupValue = new SetupValue(error);
            
            setupValue.Value.ShouldBe(error);
            setupValue.IsException.ShouldBeFalse();
        }
        
        [Fact]
        public void CtorSetsValueAsExceptionToBeThrown()
        {
            var error = new Exception();
            var setupValue = new SetupValue(error, true);
            
            setupValue.Value.ShouldBe(error);
            setupValue.IsException.ShouldBeTrue();
        }
    }
}
