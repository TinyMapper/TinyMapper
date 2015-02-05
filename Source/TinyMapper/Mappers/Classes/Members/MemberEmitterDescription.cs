using System;
using Nelibur.ObjectMapper.CodeGenerators.Emitters;
using Nelibur.ObjectMapper.Core.DataStructures;
using Nelibur.ObjectMapper.Core.Extensions;
using Nelibur.ObjectMapper.Mappers.Caches;

namespace Nelibur.ObjectMapper.Mappers.Classes.Members
{
    internal sealed class MemberEmitterDescription
    {
        public MemberEmitterDescription(IEmitter emitter, MapperCache mappers)
        {
            Emitter = emitter;
            MapperCache = new Option<MapperCache>(mappers, mappers.IsEmpty);
        }

        public IEmitter Emitter { get; private set; }
        public Option<MapperCache> MapperCache { get; private set; }

        public void AddMapper(MapperCache value)
        {
            MapperCache = value.ToOption();
        }
    }
}
