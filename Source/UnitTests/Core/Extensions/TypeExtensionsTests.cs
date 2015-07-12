using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Nelibur.ObjectMapper.Core.Extensions;
using Xunit;
using Xunit.Extensions;

namespace UnitTests.Core.Extensions
{
    public sealed class TypeExtensionsTests
    {
        [Fact]
        public void HasDefaultCtor_MemoryStream_True()
        {
            Assert.True((typeof(MemoryStream)).HasDefaultCtor());
        }

        [Fact]
        public void HasDefaultCtor_String_False()
        {
            Assert.False((typeof(string)).HasDefaultCtor());
        }

        [InlineData(typeof(Dictionary<string, int>), true)]
        [InlineData(typeof(List<int>), false)]
        [Theory]
        public void IsDictionaryOf_Types_True(Type type, bool result)
        {
            Assert.Equal(result, type.IsDictionaryOf());
        }

        [InlineData(typeof(List<int>), true)]
        [InlineData(typeof(int[]), true)]
        [InlineData(typeof(ArrayList), false)]
        [InlineData(typeof(int), false)]
        [Theory]
        public void IsIEnumerableOf_Types_True(Type type, bool result)
        {
            Assert.Equal(result, type.IsIEnumerableOf());
        }

        [InlineData(typeof(List<int>), true)]
        [InlineData(typeof(int[]), true)]
        [InlineData(typeof(ArrayList), true)]
        [InlineData(typeof(int), false)]
        [Theory]
        public void IsIEnumerable_Types_True(Type type, bool result)
        {
            Assert.Equal(result, type.IsIEnumerable());
        }

        [InlineData(typeof(List<int>), true)]
        [InlineData(typeof(int[]), false)]
        [Theory]
        public void IsListOf_Types_True(Type type, bool result)
        {
            Assert.Equal(result, type.IsListOf());
        }

        [Fact]
        public void IsNullable_NotNullable_False()
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
