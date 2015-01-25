using TinyMapper.CodeGenerators.Emitters;
using TinyMapper.Nelibur.Sword.DataStructures;
using TinyMapper.Nelibur.Sword.Extensions;

namespace TinyMapper.Mappers.Classes.Members
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
