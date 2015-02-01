using TinyMappers.Mappers;

namespace TestAssembly
{
    internal class TestMapper : Mapper
    {
        private readonly A _a = new A { Id = 1 };
        private readonly B _b = new B();

        public TestMapper()
        {
            _b.Id = _a.Id;
        }

        protected override object MapCore(object source, object target)
        {
            return null;
        }
    }


    public class A
    {
        public int Id { get; set; }
    }


    public class B
    {
        public int Id { get; set; }
    }
}
