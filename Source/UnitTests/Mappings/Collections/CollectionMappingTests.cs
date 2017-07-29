using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Nelibur.ObjectMapper;
using Nelibur.ObjectMapper.Reflection;
using Xunit;

namespace UnitTests.Mappings.Collections
{
    public sealed class CollectionMappingTests
    {
        [Fact]
        public void Map_Collections_Success()
        {
            TinyMapper.Bind<Source1, Target1>();

            var source = new Source1
            {
                Items = new List<Item1>
                {
                    new Item1
                    {
                        Int = 1,
                        String = "2",
                        List = new List<int> { 1, 2, 3 },
                        Bools = new[] { true, false }
                    },
                    new Item1
                    {
                        Int = 2,
                        String = "3",
                        List = new List<int> { 2, 3 },
                        Bools = new[] { false, false }
                    }
                }
            };

            var actual = TinyMapper.Map<Target1>(source);

            Assert.Equal(source.Items.Count, actual.Items.Count);
            Assert.Equal(source.Items1, actual.Items1);

            for (int i = 0; i < source.Items.Count; i++)
            {
                Item1 expectedItem = source.Items[i];
                Item2 actualItem = actual.Items[i];

                Assert.Equal(expectedItem.Bools, actualItem.Bools);
                Assert.Equal(expectedItem.Int, actualItem.Int);
                Assert.Equal(expectedItem.List, actualItem.List);
                Assert.Equal(expectedItem.String, actualItem.String);
            }
        }

        [Fact]
        public void Map_DifferentCollections_Success()
        {
            TinyMapper.Bind<Person, PersonDto>();

            var source = new Person
            {
                Contacts = new List<Contact>
                {
                    new Contact
                    {
                        Int = 1,
                        String = "2"
                    }
                }
            };

            var actual = TinyMapper.Map<PersonDto>(source);

            Assert.Equal(source.Contacts.Count, actual.Contacts.Count);
            for (int i = 0; i < source.Contacts.Count; i++)
            {
                Contact expectedItem = source.Contacts[i];
                ContactDto actualItem = actual.Contacts[i];

                Assert.Equal(expectedItem.Int, actualItem.Int);
                Assert.Equal(expectedItem.String, actualItem.String);
            }
        }

        [Fact]
        public void Map_InterfaceToCollection_Success()
        {
            TinyMapper.Bind<Source3, Target3>();

            var source = new Source3
            {
                List = new List<int> { 1, 2, 3 }
            };

            var actual = TinyMapper.Map<Target3>(source);
            Assert.Equal(source.List, actual.List);
        }

        [Fact]
        public void Map_NullCollection_Success()
        {
            var source = new Source2
            {
                Int = 1
            };

            var actual = TinyMapper.Map<Target2>(source);

            Assert.Equal(source.Ints, actual.Ints);
            Assert.Equal(source.Int, actual.Int);
        }

        [Fact]
        public void Map_IEnumerable_T_Success()
        {
            var target = new PersonComplex
            {
                Emails = new[] {"none1@none.com", "none2@none.com"},
                FirstName = "John",
                LastName = "Doe"
            };

            TinyMapper.Bind<PersonComplex, PersonComplexTarget>();

            var actual = TinyMapper.Map<PersonComplexTarget>(target);

            Assert.Equal(target.FirstName, actual.FirstName);
            Assert.Equal(target.LastName, actual.LastName);
            Assert.Equal(target.Emails, actual.Emails);
        }

        [Fact]
        public void Map_IEnumerable_Success()
        {
            var target = new PersonComplex
            {
                Emails = new[] { "none1@none.com", "none2@none.com" },
                FirstName = "John",
                LastName = "Doe"
            };

            TinyMapper.Bind<PersonComplex, PersonComplexTarget2>();

            var actual = TinyMapper.Map<PersonComplexTarget2>(target);

            Assert.Equal(target.FirstName, actual.FirstName);
            Assert.Equal(target.LastName, actual.LastName);
            Assert.Equal(target.Emails, actual.Emails.Cast<string>());
        }

        [Fact]
        public void Map_To_IEnumerable_T_Success()
        {
            var target = new PersonComplex3
            {
                Emails = new ArrayList(new [] { "none1@none.com", "none2@none.com" }),
                FirstName = "John",
                LastName = "Doe"
            };

            TinyMapper.Bind<PersonComplex3, PersonComplexTarget3>();

            var actual = TinyMapper.Map<PersonComplexTarget3>(target);

            Assert.Equal(target.FirstName, actual.FirstName);
            Assert.Equal(target.LastName, actual.LastName);
            Assert.Equal(target.Emails.Cast<string>(), actual.Emails);
        }

        [Fact]
        public void Map_To_IEnumerable_Success()
        {
            var target = new PersonComplex
            {
                Emails = new[] { "none1@none.com", "none2@none.com" },
                FirstName = "John",
                LastName = "Doe"
            };

            TinyMapper.Bind<PersonComplex, PersonComplexTarget4>();

            var actual = TinyMapper.Map<PersonComplexTarget4>(target);

            Assert.Equal(target.FirstName, actual.FirstName);
            Assert.Equal(target.LastName, actual.LastName);
            Assert.Equal(target.Emails, actual.Emails.Cast<string>());
        }

        public class Contact
        {
            public int Int { get; set; }
            public string String { get; set; }
        }

        public class ContactDto
        {
            public int Int { get; set; }
            public string String { get; set; }
        }

        public sealed class Item1
        {
            public bool[] Bools { get; set; }
            public int Int { get; set; }
            public List<int> List { get; set; }
            public string String { get; set; }
        }

        public sealed class Item2
        {
            public bool[] Bools { get; set; }
            public int Int { get; set; }
            public List<int> List { get; set; }
            public string String { get; set; }
        }

        public class Person
        {
            public List<Contact> Contacts { get; set; }
        }

        public class PersonDto
        {
            public List<ContactDto> Contacts { get; set; }
        }

        public class PersonComplex
        {
            public IEnumerable<string> Emails { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }

        public class PersonComplexTarget
        {
            public IList<string> Emails { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }

        public class PersonComplexTarget4
        {
            public IEnumerable Emails { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }

        public class PersonComplexTarget2
        {
            public ArrayList Emails { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }

        public class PersonComplex3
        {
            public ArrayList Emails { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }

        public class PersonComplexTarget3
        {
            public IEnumerable<string> Emails { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }

        public class Source1
        {
            public IList<Item1> Items { get; set; }
            public List<Item1> Items1 { get; set; }
        }

        public class Source2
        {
            public int Int { get; set; }
            public List<int> Ints { get; set; }
        }

        public class Source3
        {
            public IReadOnlyList<int> List { get; set; }
        }

        public class Target1
        {
            public List<Item2> Items { get; set; }
            public List<Item1> Items1 { get; set; }
        }

        public class Target2
        {
            public int Int { get; set; }
            public List<int> Ints { get; set; }
        }

        public class Target3
        {
            public List<int> List { get; set; }
        }
    }
}
