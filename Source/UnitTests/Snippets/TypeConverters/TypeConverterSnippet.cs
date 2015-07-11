using System;
using System.ComponentModel;
using System.Globalization;
using Xunit;

namespace UnitTests.Snippets.TypeConverters
{
    public sealed class TypeConverterSnippet
    {
        public TypeConverterSnippet()
        {
            TypeDescriptor.AddAttributes(typeof(SourceClass), new TypeConverterAttribute(typeof(SourceClassConverter)));
        }

        [Fact]
        public void Converter()
        {
            TypeConverter typeConverter = TypeDescriptor.GetConverter(typeof(SourceClass));
            var source = new SourceClass
            {
                FirstName = "First",
                LastName = "Last"
            };
            var target = (TargetClass)typeConverter.ConvertFrom(source);
            Assert.Equal(string.Format("{0} {1}", source.FirstName, source.LastName), target.FullName);
        }

        private class SourceClass
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }

        private sealed class SourceClassConverter : TypeConverter
        {
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                return sourceType == typeof(SourceClass);
            }

            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                var concreteValue = (SourceClass)value;
                var result = new TargetClass
                {
                    FullName = string.Format("{0} {1}", concreteValue.FirstName, concreteValue.LastName)
                };
                return result;
            }
        }

        private class TargetClass
        {
            public string FullName { get; set; }
        }
    }
}
