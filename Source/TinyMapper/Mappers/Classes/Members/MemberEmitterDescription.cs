using System;
using Nelibur.Mapper.CodeGenerators.Emitters;
using Nelibur.Mapper.Core.DataStructures;
using Nelibur.Mapper.Core.Extensions;
using Nelibur.Mapper.Mappers.Caches;

namespace Nelibur.Mapper.Mappers.Classes.Members
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
