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

        private static MemberInfo[] GetPublicMembers(Type type)
        {
            BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
            PropertyInfo[] properties = type.GetProperties(flags);
            FieldInfo[] fields = type.GetFields(flags);
            MemberInfo[] members = new MemberInfo[properties.Length + fields.Length];
            properties.CopyTo(members, 0);
            fields.CopyTo(members, properties.Length);
            return members;
        }

        private static List<MemberInfo> GetSourceMembers(Type sourceType)
        {
            var result = new List<MemberInfo>();

            MemberInfo[] members = GetPublicMembers(sourceType);
            foreach (MemberInfo member in members)
            {
                if (member.IsProperty())
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

            MemberInfo[] members = GetPublicMembers(targetType);
            foreach (MemberInfo member in members)
            {
                if (member.IsProperty())
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

        private string GetTargetName(
            Option<BindingConfig> bindingConfig,
            TypePair typePair,
            MemberInfo sourceMember,
            Dictionary<string, string> targetBindings)
        {
            Option<string> targetName;
            List<BindAttribute> binds = sourceMember.GetAttributes<BindAttribute>();
            BindAttribute bind = binds.FirstOrDefault(x => x.TargetType.IsNull());
            if (bind.IsNull())
            {
                bind = binds.FirstOrDefault(x => typePair.Target.IsAssignableFrom(x.TargetType));
            }
            if (bind.IsNotNull())
            {
                targetName = new Option<string>(bind.MemberName);
            }
            else
            {
                targetName = bindingConfig.Map(x => x.GetBindField(sourceMember.Name));
                if (targetName.HasNoValue)
                {
                    string targetMemberName;
                    if (targetBindings.TryGetValue(sourceMember.Name, out targetMemberName))
                    {
                        targetName = new Option<string>(targetMemberName);
                    }
                    else
                    {
                        targetName = new Option<string>(sourceMember.Name);
                    }
                }
            }
            return targetName.Value;
        }

        private Dictionary<string, string> GetTest(TypePair typePair, List<MemberInfo> targetMembers)
        {
            var result = new Dictionary<string, string>();
            foreach (MemberInfo member in targetMembers)
            {
                Option<BindAttribute> bindAttribute = member.GetAttribute<BindAttribute>();
                if (bindAttribute.HasNoValue)
                {
                    continue;
                }

                if (bindAttribute.Value.TargetType.IsNull() || typePair.Source.IsAssignableFrom(bindAttribute.Value.TargetType))
                {
                    result[bindAttribute.Value.MemberName] = member.Name;
                }
            }
            return result;
        }

        private bool IsIgnore(Option<BindingConfig> bindingConfig, TypePair typePair, MemberInfo sourceMember)
        {
            List<IgnoreAttribute> ignores = sourceMember.GetAttributes<IgnoreAttribute>();
            if (ignores.Any(x => x.TargetType.IsNull()))
            {
                return true;
            }
            if (ignores.FirstOrDefault(x => typePair.Target.IsAssignableFrom(x.TargetType)).IsNotNull())
            {
                return true;
            }
            return bindingConfig.Map(x => x.IsIgnoreSourceField(sourceMember.Name)).Value;
        }

        private List<MappingMember> ParseMappingTypes(TypePair typePair)
        {
            var result = new List<MappingMember>();

            List<MemberInfo> sourceMembers = GetSourceMembers(typePair.Source);
            List<MemberInfo> targetMembers = GetTargetMembers(typePair.Target);

            Dictionary<string, string> targetBindings = GetTest(typePair, targetMembers);

            Option<BindingConfig> bindingConfig = _config.GetBindingConfig(typePair);

            foreach (MemberInfo sourceMember in sourceMembers)
            {
                if (IsIgnore(bindingConfig, typePair, sourceMember))
                {
                    continue;
                }

                string targetName = GetTargetName(bindingConfig, typePair, sourceMember, targetBindings);
                MemberInfo targetMember = targetMembers.FirstOrDefault(x => _config.NameMatching(targetName, x.Name));

                if (targetMember.IsNull())
                {
                    continue;
                }
                Option<Type> concreteBindingType = bindingConfig.Map(x => x.GetBindType(targetName));
                if (concreteBindingType.HasValue)
                {
                    var mappingTypePair = new TypePair(sourceMember.GetMemberType(), concreteBindingType.Value);
                    result.Add(new MappingMember(sourceMember, targetMember, mappingTypePair));
                }
                else
                {
                    result.Add(new MappingMember(sourceMember, targetMember));
                }
            }
            return result;
        }
    }
}
