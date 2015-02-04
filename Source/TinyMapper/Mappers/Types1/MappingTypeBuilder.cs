using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Nelibur.Mapper.Core.DataStructures;
using Nelibur.Mapper.Core.Extensions;
using Nelibur.Mapper.Mappers.Types1.Members;
using Nelibur.Mapper.TypeConverters;

namespace Nelibur.Mapper.Mappers.Types1
{
    internal sealed class MappingTypeBuilder
    {
        public static MappingType Build(TypePair typePair)
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

        private static bool IsPrimitiveMember(TypePair typePair)
        {
            return PrimitiveTypeConverter.IsSupported(typePair);
        }

        private static bool Match(string valueA, string valueB)
        {
            return string.Equals(valueA, valueB, StringComparison.Ordinal);
        }

        private static MappingType ParseMappingTypes(TypePair typePair)
        {
            var mappingType = new MappingType(typePair);

            List<MemberInfo> sourceMembers = GetSourceMembers(typePair.Source);
            List<MemberInfo> targetMembers = GetTargetMembers(typePair.Target);

            foreach (MemberInfo targetMember in targetMembers)
            {
                MemberInfo sourceMember = sourceMembers.FirstOrDefault(x => Match(x.Name, targetMember.Name));
                if (sourceMember.IsNull())
                {
                    continue;
                }
                var mappingPair = new TypePair(sourceMember.GetMemberType(), targetMember.GetMemberType());
                if (IsPrimitiveMember(mappingPair))
                {
                    MappingMember primitive = new PrimitiveMappingMember(sourceMember, targetMember);
                    mappingType.Members.Add(primitive);
                }
                else
                {
                    MappingMember complex = new ComplexMappingMember(sourceMember, targetMember);
                    mappingType.Members.Add(complex);
                }
            }
            return mappingType;
        }
    }
}
