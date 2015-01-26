using System;

namespace TinyMapper.TypeConverters
{
    internal interface ITypeConverter
    {
        object Convert(object source);
    }
}
