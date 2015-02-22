using System;
using Nelibur.ObjectMapper.Core.DataStructures;
using Nelibur.ObjectMapper.Mappers;
using Nelibur.ObjectMapper.Mappers.PrimitiveTypes;
using Nelibur.ObjectMapper.Reflection;
using Xunit;
using Xunit.Extensions;

namespace UnitTests.Mappers.PrimitiveTypes
{
    public sealed class PrimitiveTypeMapperTests
    {
        [InlineData(typeof(bool), typeof(bool), true, true)]
        [InlineData(typeof(int), typeof(int), 1, 1)]
        [Theory]
        public void Test(Type sourceType, Type targetType, object source, object expected)
        {
            var builder = new PrimitiveTypeMapperBuilder(new MappingBuilderConfigStub());
            Mapper mapper = builder.Create(new TypePair(sourceType, targetType));
            object actual = mapper.Map(source);
            Assert.Equal(expected, actual);
        }


        private class MappingBuilderConfigStub : IMapperBuilderConfig
        {
            public IDynamicAssembly Assembly { get; private set; }

            public MapperBuilder GetMapperBuilder(TypePair typePair)
            {
                throw new NotImplementedException();
            }
        }
    }
}
