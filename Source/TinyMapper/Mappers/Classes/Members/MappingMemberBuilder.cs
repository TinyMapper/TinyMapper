using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Nelibur.ObjectMapper.Core.DataStructures;
using Nelibur.ObjectMapper.Core.Extensions;

namespace Nelibur.ObjectMapper.Mappers.Classes.Members
{
    internal sealed class MappingMemberBuilder
    {
        public static List<MappingMember> Build(TypePair typePair)
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

        private static List<MappingMember> ParseMappingTypes(TypePair typePair)
        {
            var result = new List<MappingMember>();

            List<MemberInfo> sourceMembers = GetSourceMembers(typePair.Source);
            List<MemberInfo> targetMembers = GetTargetMembers(typePair.Target);

            foreach (MemberInfo targetMember in targetMembers)
            {
                MemberInfo sourceMember = sourceMembers.FirstOrDefault(x => Match(x.Name, targetMember.Name));
                if (sourceMember.IsNull())
                {
                    continue;
                }
                result.Add(new MappingMember(sourceMember, targetMember));
            }
            return result;
        }
    }
}
