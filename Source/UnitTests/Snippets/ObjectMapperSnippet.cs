using System;
using Nelibur.ObjectMapper;
using Nelibur.ObjectMapper.Bindings;

namespace UnitTests.Snippets
{
    public sealed class ObjectMapperSnippet : IObjectMapper
    {
        public void Bind<TSource, TTarget>()
        {
            TinyMapper.Bind<TSource, TTarget>();
        }

        public void Bind<TSource, TTarget>(Action<IBindingConfig<TSource, TTarget>> config)
        {
            TinyMapper.Bind(config);
        }

        public TTarget Map<TSource, TTarget>(TSource source, TTarget target = default(TTarget))
        {
            return TinyMapper.Map(source, target);
        }

        public TTarget Map<TTarget>(object source)
        {
            return TinyMapper.Map<TTarget>(source);
        }
    }

    public interface IObjectMapper
    {
        void Bind<TSource, TTarget>();
        void Bind<TSource, TTarget>(Action<IBindingConfig<TSource, TTarget>> config);
        TTarget Map<TSource, TTarget>(TSource source, TTarget target = default(TTarget));
        TTarget Map<TTarget>(object source);
    }
}
