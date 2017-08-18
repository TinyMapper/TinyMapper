using System;
using System.Collections.Generic;
using Nelibur.ObjectMapper;
using Nelibur.ObjectMapper.Reflection;
using Xunit;

namespace UnitTests
{
    public sealed class ForTests
    {
//        [Fact(Skip = "ForTests")]
        [Fact]
        public void Test()
        {

//            TinyMapper.Bind<NestedPoco, NestedPocoDTO>(config =>
//            {
//                config.Bind(target => target.NestedObjects, typeof(List<NestedPoco>));
//            });

            var root = new Node
            {
                Id = "1",
                Next = new Node
                {
                    Id = "2",
                    Next = new Node
                    {
                        Id = "3",
                        Child = new[]
                        {
                            new Node
                            {
                                Id = "Child Child 1"
                            }
                        }

                    }
                },
                Child = new[]
                {
                    new Node
                    {
                        Id = "Child 1"
                    }
                }
            };

            TinyMapper.Bind<Node, Node>();
            DynamicAssemblyBuilder.Get().Save();

            TinyMapper.Map<Node, Node>(root);

        }

    }

    public class Node
    {
        public string Id;
        public Node Next;
        public Node[] Child;
    }

    public class NestedPocoDTO : NestedPoco
    {

    }

    public class NestedPoco : SimplePoco
    {
        public List<NestedPoco> NestedObjects { get; set; }

        public NestedPoco()
        {
            NestedObjects = new List<NestedPoco>();
        }
    }

    public class SimplePoco
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool Enabled { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string Town { get; set; }
        public DateTime BirthDate { get; set; }
        public string PhoneNumber { get; set; }
    }

}
