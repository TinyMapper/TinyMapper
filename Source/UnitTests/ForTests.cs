using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using Nelibur.ObjectMapper;
using Xunit;

namespace UnitTests
{
    public sealed class ForTests
    {
        [Fact]
        public void Test()
        {
            var source = new Source
            {
                Collection = new List<int> { 1, 2, 3 }
            };

            TinyMapper.Bind<Source, Target>();

            var target = TinyMapper.Map<Target>(source);
        }

        [TypeConverter(typeof(SourceConverter))]
        public sealed class Source
        {
            public ICollection Collection { get; set; }
        }

        public sealed class SourceConverter : TypeConverter
        {
            public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
            {
                return destinationType == typeof(Target);
            }

            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
            {
                var concreteValue = (Source)value;
                var result = new Target
                {
                    Collection = new List<int>((IEnumerable<int>)concreteValue.Collection)
                };
                return result;
            }
        }

        public sealed class Target
        {
            public ICollection Collection { get; set; }
        }
    }
}
