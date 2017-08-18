using Nelibur.ObjectMapper;
using Xunit;

namespace UnitTests.Mappings
{
    public class MapWithCircularReferences
    {
        [Fact]
        public void Map_Node_CircularReferences_Success()
        {
            var source = new Node
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
                                Id = " 123 1"
                            },
                            new Node
                            {
                                Id = "123 2"
                            }
                        }

                    }
                },
                Child = new[]
                {
                    new Node
                    {
                        Id = "1 1"
                    },
                    new Node
                    {
                        Id = "1 2"
                    }
                }
            };

            TinyMapper.Bind<Node, Node>();

            var target = TinyMapper.Map<Node, Node>(source);

            Assert.Equal(source.Id, target.Id);
            Assert.Equal(source.Next.Id, target.Next.Id);
            Assert.Equal(source.Next.Next.Id, target.Next.Next.Id);
            Assert.Equal(source.Next.Next.Id, target.Next.Next.Id);

            Assert.Equal(source.Next.Next.Child, target.Next.Next.Child);

            Assert.Equal(source.Child, target.Child);
        }

    }

    public class Node
    {
        public string Id;
        public Node Next;
        public Node[] Child;

        protected bool Equals(Node other)
        {
            return string.Equals(Id, other.Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != this.GetType())
                return false;
            return Equals((Node)obj);
        }

        public override int GetHashCode()
        {
            return (Id != null ? Id.GetHashCode() : 0);
        }
    }
}
