using HyperMock.Matchers;
using Xunit;

namespace Tests.HyperMock.Matchers
{
    public class AnyParameterMatcherTests
    {
        private readonly AnyParameterMatcher _matcher;

        public AnyParameterMatcherTests()
        {
            _matcher = new AnyParameterMatcher();
        }
        
        [Fact]
        public void IsMatchReturnsTrueForNull()
        {
            var result = _matcher.IsMatch(null, null);

            Assert.True(result);
        }

        [Fact]
        public void IsMatchReturnsTrueForSameReferenceTypeInstance()
        {
            var expected = new object();
            var actual = expected;

            var result = _matcher.IsMatch(expected, actual);

            Assert.True(result);
        }

        [Fact]
        public void IsMatchReturnsTrueForDifferentReferenceTypeInstance()
        {
            var expected = new object();
            var actual = new object();

            var result = _matcher.IsMatch(expected, actual);

            Assert.True(result);
        }

        [Fact]
        public void IsMatchReturnsTrueForSameValueTypeInstance()
        {
            var expected = 10;
            var actual = expected;

            var result = _matcher.IsMatch(expected, actual);

            Assert.True(result);
        }

        [Fact]
        public void IsMatchReturnsFalseForAltDataType()
        {
            var expected = 10;
            var actual = "Homer";

            var result = _matcher.IsMatch(expected, actual);

            Assert.False(result);
        }

        [Fact]
        public void IsMatchReturnsTrueForDifferentValueTypeInstance()
        {
            var expected = 10;
            var actual = 20;

            var result = _matcher.IsMatch(expected, actual);

            Assert.True(result);
        }

        [Fact]
        public void IsMatchReturnsTrueForSameStringValue()
        {
            var expected = "Homer";
            var actual = expected;

            var result = _matcher.IsMatch(expected, actual);

            Assert.True(result);
        }

        [Fact]
        public void IsMatchReturnsTrueForDifferentStringValue()
        {
            var expected = "Homer";
            var actual = "Marge";

            var result = _matcher.IsMatch(expected, actual);

            Assert.True(result);
        }
    }
}
