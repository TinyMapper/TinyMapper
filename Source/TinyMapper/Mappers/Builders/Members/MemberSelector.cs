using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TinyMapper.Configs;
using TinyMapper.DataStructures;
using TinyMapper.Nelibur.Sword.Extensions;

namespace TinyMapper.Mappers.Builders.Members
{
    internal sealed class MemberSelector
    {
        private readonly MapConfig _config = new MapConfig();
        private readonly Type _sourceType;
        private readonly Type _targetType;

        public MemberSelector(TypePair typePair)
        {
            _sourceType = typePair.Source;
            _targetType = typePair.Target;
        }

        internal List<SimpleMappingMember> GetMappingMembers()
        {
            List<MemberInfo> sourceMembers = GetSourceMembers(_sourceType);
            List<MemberInfo> targetMembers = GetTargetMembers(_targetType);

            return GetMappingMembers(sourceMembers, targetMembers, _config.Match);
        }

        private static List<MemberInfo> GetPublicMembers(Type type)
        {
            return type.GetMembers(BindingFlags.Instance | BindingFlags.Public)
                       .Where(x => x.MemberType == MemberTypes.Property || x.MemberType == MemberTypes.Field)
                       .ToList();
        }

        private List<SimpleMappingMember> GetMappingMembers(List<MemberInfo> source, List<MemberInfo> target,
            Func<string, string, bool> memberMatcher)
        {
            var result = new List<SimpleMappingMember>();

            foreach (MemberInfo targetMember in target)
            {
                MemberInfo sourceMember = source.FirstOrDefault(x => memberMatcher(x.Name, targetMember.Name));
                if (sourceMember.IsNull())
                {
                    continue;
                }
                var mappingMember = new SimpleMappingMember(sourceMember, targetMember);
                result.Add(mappingMember);
            }
            return result;
        }

        private List<MemberInfo> GetSourceMembers(Type sourceType)
        {
            var result = new List<MemberInfo>();

            List<MemberInfo> members = GetPublicMembers(sourceType);
            foreach (MemberInfo member in members)
            {
                if (member.MemberType == MemberTypes.Property)
                {
                    MethodInfo method = ((PropertyInfo)member).GetGetMethod();
                    if (method.IsNull())
                    {
                        continue;
                    }
                }
                result.Add(member);
            }
            return result;
        }

        private List<MemberInfo> GetTargetMembers(Type targetType)
        {
            var result = new List<MemberInfo>();

            List<MemberInfo> members = GetPublicMembers(targetType);
            foreach (MemberInfo member in members)
            {
                if (member.MemberType == MemberTypes.Property)
                {
                    MethodInfo method = ((PropertyInfo)member).GetSetMethod();
                    if (method.IsNull() || method.GetParameters().Length != 1)
                    {
                        continue;
                    }
                }
                result.Add(member);
            }
            return result;
        }
    }
}
