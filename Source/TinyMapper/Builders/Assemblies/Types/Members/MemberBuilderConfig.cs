using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection.Emit;
using TinyMapper.CodeGenerators;
using TinyMapper.Nelibur.Sword.Extensions;

namespace TinyMapper.Builders.Assemblies.Types.Members
{
    internal sealed class MemberBuilderConfig : IMemberBuilderConfig
    {
        private MemberBuilderConfig()
        {
        }

        public CodeGenerator CodeGenerator { get; set; }
        public LocalBuilder LocalSource { get; set; }
        public LocalBuilder LocalTarget { get; set; }

        public static IMemberBuilderConfig Create()
        {
            return new MemberBuilderConfig();
        }

        public IMemberBuilderConfig Configure(Action<IMemberBuilderConfig> action)
        {
            action(this);
            return this;
        }

        public MemberBuilder CreateMemberBuilder()
        {
            Validate();
            return new MemberBuilder(this);
        }

        private void Validate()
        {
            var nullCheck = new List<object>
            {
                LocalSource, LocalTarget, CodeGenerator
            };

            if (nullCheck.Any(x => x.IsNull()))
            {
                throw new ConfigurationErrorsException();
            }
        }
    }
}
