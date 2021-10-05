using HyperMock.Matchers;
using Shouldly;
using Xunit;
// ReSharper disable ConvertToConstant.Local
// ReSharper disable InlineTemporaryVariable

namespace HyperMock.Tests.Matchers
{
    public class ExactParameterMatcherTests
    {
        private readonly ExactParameterMatcher _matcher;

        public ExactParameterMatcherTests()
        {
            _matcher = new ExactParameterMatcher();
        }

        [Fact]
        public void IsMatchReturnsTrueForNull()
        {
            var result = _matcher.IsMatch(null, null);

            result.ShouldBeTrue();
        }

        [Fact]
        public void IsMatchReturnsTrueForSameReferenceTypeInstance()
        {
            var expected = new object();
            var actual = expected;

            var result = _matcher.IsMatch(expected, actual);

            result.ShouldBeTrue();
        }

        [Fact]
        public void IsMatchReturnsFalseForDifferentReferenceTypeInstance()
        {
            var expected = new object();
            var actual = new object();

            var result = _matcher.IsMatch(expected, actual);

            result.ShouldBeFalse();
        }

        [Fact]
        public void IsMatchReturnsTrueForSameValueTypeInstance()
        {
            var expected = 10;
            var actual = expected;

            var result = _matcher.IsMatch(expected, actual);

            result.ShouldBeTrue();
        }

        [Fact]
        public void IsMatchReturnsFalseForDifferentValueTypeInstance()
        {
            var expected = 10;
            var actual = 20;

            var result = _matcher.IsMatch(expected, actual);

            result.ShouldBeFalse();
        }

        [Fact]
        public void IsMatchReturnsTrueForSameStringValue()
        {
            var expected = "Homer";
            var actual = "Homer";

            var result = _matcher.IsMatch(expected, actual);

            result.ShouldBeTrue();
        }

        [Fact]
        public void IsMatchReturnsFalseForDifferentStringValue()
        {
            var expected = "Homer";
            var actual = "Marge";

            var result = _matcher.IsMatch(expected, actual);

            result.ShouldBeFalse();
        }
    }
}
