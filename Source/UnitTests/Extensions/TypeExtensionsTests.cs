using TinyMapper.Extensions;
using Xunit;

namespace UnitTests.Extensions
{
    public sealed class TypeExtensionsTests
    {
        [Fact]
        public void IsNullable_NotNullable_True()
        {
            Assert.False(typeof(int).IsNullable());
        }

        [Fact]
        public void IsNullable_Nullable_True()
        {
            Assert.True(typeof(int?).IsNullable());
        }
    }
}
