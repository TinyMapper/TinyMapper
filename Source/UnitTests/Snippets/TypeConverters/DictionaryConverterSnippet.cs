using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using Nelibur.ObjectMapper;
using Xunit;

namespace UnitTests.Snippets.TypeConverters
{
    public sealed class DictionaryConverterSnippet
    {
        public DictionaryConverterSnippet()
        {
            var converterAttribute = new TypeConverterAttribute(typeof(DictionaryTypeConverter<string, string>));
            TypeDescriptor.AddAttributes(typeof(Dictionary<string, string>), converterAttribute);
        }

        [Fact]
        public void Converter()
        {
            TinyMapper.Bind<SourceClass, TargetClass>();

            var source = new SourceClass
            {
                Dictionary = new Dictionary<string, string> { { "key", "Value" } }
            };

            var target = TinyMapper.Map<TargetClass>(source);

            Assert.Equal(source.Dictionary, target.Dictionary);
        }

        public class SourceClass
        {
            public Dictionary<string, string> Dictionary { get; set; }
        }

        public class TargetClass
        {
            public Dictionary<string, string> Dictionary { get; set; }
        }

        private sealed class DictionaryTypeConverter<TKey, TValue> : TypeConverter
        {
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                return sourceType == typeof(Dictionary<TKey, TValue>);
            }

            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                var concreteValue = (Dictionary<TKey, TValue>)value;
                var result = new Dictionary<TKey, TValue>();

                foreach (KeyValuePair<TKey, TValue> pair in concreteValue)
                {
                    result[pair.Key] = pair.Value;
                }

                return result;
            }
        }
    }
}
