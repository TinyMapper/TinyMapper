using System;
using System.ComponentModel;
using System.Globalization;
using Nelibur.ObjectMapper;
using Xunit;

namespace UnitTests
{
    public sealed class ForTests
    {
        public ForTests()
        {
            TypeDescriptor.AddAttributes(typeof(SourceClass), new TypeConverterAttribute(typeof(SourceClassConverter)));
        }

        [Fact]
        public void Test()
        {
        }
    }


    public class TargetClass
    {
        public string FullName { get; set; }
    }


    public class SourceClass
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }


    public sealed class SourceClassConverter : TinyMapperConverter
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
}
