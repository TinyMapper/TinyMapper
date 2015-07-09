using System;
using System.ComponentModel;
using System.Globalization;
using Xunit;

namespace UnitTests.Snippets.TypeConverters
{
    public sealed class YesNoConverterSnippet
    {
        public YesNoConverterSnippet()
        {
            TypeDescriptor.AddAttributes(typeof(SourceClass), new TypeConverterAttribute(typeof(YesNoTypeConverter)));
        }

        [Fact]
        public void Converter()
        {
            TypeConverter typeConverter = TypeDescriptor.GetConverter(typeof(SourceClass));
            var source = new SourceClass
            {
                Yes = "Y",
                No = "N"
            };

            var target = (TargetClass)typeConverter.ConvertFrom(source);
            Assert.True(target.Yes);
            Assert.False(target.No);
        }

        private class SourceClass
        {
            public string No { get; set; }
            public string Yes { get; set; }
        }

        private class TargetClass
        {
            public bool No { get; set; }
            public bool Yes { get; set; }
        }

        private sealed class YesNoTypeConverter : TypeConverter
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
                    Yes = ConvertYesNo(concreteValue.Yes),
                    No = ConvertYesNo(concreteValue.No)
                };
                return result;
            }

            private static bool ConvertYesNo(string yesNoValue)
            {
                return string.Equals(yesNoValue, "Y", StringComparison.OrdinalIgnoreCase);
            }
        }
    }
}
