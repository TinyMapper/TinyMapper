using System;
using System.ComponentModel;
using System.Globalization;
using Nelibur.ObjectMapper;
using Xunit;

namespace UnitTests.Mappings.TypeConverters
{
    public sealed class ConvertibleTypeMappingTests
    {
        public ConvertibleTypeMappingTests()
        {
            TypeDescriptor.AddAttributes(typeof(Source1), new TypeConverterAttribute(typeof(SourceClassConverter)));
        }

        [Fact]
        public void Map_ConvertibleType_Success()
        {
            TinyMapper.Bind<Source1, Target1>();
            var source = new Source1
            {
                FirstName = "First",
                LastName = "Last"
            };

            var result = TinyMapper.Map<Target1>(source);

            Assert.Equal(string.Format("{0} {1}", source.FirstName, source.LastName), result.FullName);
        }

        public class Source1
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }

        public sealed class SourceClassConverter : TypeConverter
        {
            public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
            {
                return destinationType == typeof(Target1);
            }

            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
            {
                var concreteValue = (Source1)value;
                var result = new Target1
                {
                    FullName = string.Format("{0} {1}", concreteValue.FirstName, concreteValue.LastName)
                };
                return result;
            }
        }

        public class Target1
        {
            public string FullName { get; set; }
        }
    }
}
