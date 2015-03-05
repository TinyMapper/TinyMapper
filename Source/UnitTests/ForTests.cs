using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Nelibur.ObjectMapper;
using Xunit;

namespace UnitTests
{
    public class ForTests
    {
        [Fact]
        public void Test()
        {
            //            Map();

            TinyMapper.Bind<Class1, Class2>(config =>
            {
                config.Ignore(x => x.Bools);
                config.Ignore(x => x.Field);
            });
        }

        private static string GetPropertyName<T>(Expression<Func<T>> action)
        {
            var expression = (MemberExpression)action.Body;
            string propertyName = expression.Member.Name;
            return propertyName;
        }

        private static void Map()
        {
            TinyMapper.Bind<Class1, Class2>();
            var source = new Class1
            {
                //                Field = 10,
                //                Property = 4
                //                List = new List<int> { 1, 2 }
                //                Class3 = new Class3 { Id = 1 }
                //                Array = new int[] { 0, 1 }
                Bools = new List<bool> { true, false }
            };
            //            DynamicAssemblyBuilder.Get().Save();
            var target = TinyMapper.Map<Class2>(source);
        }

        private static void SiteSample()
        {
            TinyMapper.Bind<Person, PersonDto>();

            var person = new Person
            {
                Id = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe",
                Email = "support@tinymapper.net"
            };

            var personDto = TinyMapper.Map<PersonDto>(person);
        }

        private static void SiteSampleIgnore()
        {
            TinyMapper.Bind<Person, PersonDto>(config =>
            {
                config.Ignore(x => x.Id);
                config.Ignore(x => x.Email);
            });

            var person = new Person
            {
                Id = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe",
                Email = "support@tinymapper.net"
            };

            var personDto = TinyMapper.Map<PersonDto>(person);
        }


        public class Class1
        {
            public int Field;
            //            public int Property { get; set; }
            //            public List<int> List { get; set; }
            //            public Class3 Class3 { get; set; }
            //            public int[] Array { get; set; }
            public List<bool> Bools { get; set; }
        }


        public class Class2
        {
            public int Field;
            //            public int Property { get; set; }
            //            public List<int> List { get; set; }
            //            public Class3 Class3 { get; set; }
            //            public int[] Array { get; set; }
            public List<bool> Bools { get; set; }
        }
    }


    public sealed class Person
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public Guid Id { get; set; }
        public string LastName { get; set; }
    }


    public sealed class PersonDto
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public Guid Id { get; set; }
        public string LastName { get; set; }
    }
}
