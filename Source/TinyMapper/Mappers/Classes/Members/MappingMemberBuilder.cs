using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Nelibur.ObjectMapper.Bindings;
using Nelibur.ObjectMapper.Core.DataStructures;
using Nelibur.ObjectMapper.Core.Extensions;

namespace Nelibur.ObjectMapper.Mappers.Classes.Members
{
    internal sealed class MappingMemberBuilder
    {
        private readonly IMapperBuilderConfig _config;

        public MappingMemberBuilder(IMapperBuilderConfig config)
        {
            _config = config;
        }

        public List<MappingMember> Build(TypePair typePair)
        {
            return ParseMappingTypes(typePair);
        }

        private static List<MemberInfo> GetPublicMembers(Type type)
        {
            return type.GetMembers(BindingFlags.Instance | BindingFlags.Public)
                       .Where(x => x.MemberType == MemberTypes.Property || x.MemberType == MemberTypes.Field)
                       .ToList();
        }

        private static List<MemberInfo> GetSourceMembers(Type sourceType)
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

        private static List<MemberInfo> GetTargetMembers(Type targetType)
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

        private static bool Match(string valueA, string valueB)
        {
            return string.Equals(valueA, valueB, StringComparison.Ordinal);
        }

        private List<MappingMember> ParseMappingTypes(TypePair typePair)
        {
            var result = new List<MappingMember>();

            List<MemberInfo> sourceMembers = GetSourceMembers(typePair.Source);
            List<MemberInfo> targetMembers = GetTargetMembers(typePair.Target);

            Dictionary<string, string> map = new Dictionary<string, string>();
            foreach (var targetMember in targetMembers)
            {
                var attributes = targetMember.GetCustomAttributes();
                BindAttribute bind = attributes.FirstOrDefault(x => x is BindAttribute) as BindAttribute;
                if (bind.IsNotNull())
                {
                    map.Add(bind.Name, targetMember.Name);
                }
            }

            Option<BindingConfig> bindingConfig = _config.GetBindingConfig(typePair);

            foreach (MemberInfo sourceMember in sourceMembers)
            {
                var attributes = sourceMember.GetCustomAttributes();
                if (attributes.FirstOrDefault(x => x is IgnoreAttribute).IsNotNull())
                {
                    continue;
                }
                if (bindingConfig.Map(x => x.IsIgnoreField(sourceMember.Name)).Value)
                {
                    continue;
                }

                Option<string> targetName;
                BindAttribute bind = attributes.FirstOrDefault(x => x is BindAttribute) as BindAttribute;
                if (bind.IsNotNull())
                {
                    targetName = new Option<string>(bind.Name);
                }
                else
                {
                    targetName = bindingConfig.Map(x => x.GetBindField(sourceMember.Name));
                    if (targetName.HasNoValue)
                    {
                        string targetMemberName;
                        if (map.TryGetValue(sourceMember.Name, out targetMemberName))
                        {
                            targetName = new Option<string>(targetMemberName);
                        }
                        else
                        {
                            targetName = new Option<string>(sourceMember.Name);
                        }
                    }
                }

                MemberInfo targetMember = targetMembers.FirstOrDefault(x => Match(x.Name, targetName.Value));
                if (targetMember.IsNull())
                {
                    continue;
                }
                result.Add(new MappingMember(sourceMember, targetMember));
            }
            return result;
        }
    }
}
