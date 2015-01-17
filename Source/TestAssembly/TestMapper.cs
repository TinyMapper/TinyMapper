using TinyMapper.Mappers;

namespace TestAssembly
{
    public class TestMapper : Mapper
    {
        internal override object CreateTargetInstanceCore()
        {
            return new Class2();
        }

        internal override object MapMembersCore(object source, object target)
        {
            var class1 = (Class1)source;
            var class2 = (Class2)target;
            class2.Field = class1.Field;
            return class2;
        }
    }


    public class Class1
    {
        public int Field;
    }


    public class Class2
    {
        public int Field;
    }
}
