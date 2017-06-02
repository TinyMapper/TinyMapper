using Nelibur.ObjectMapper;
using Nelibur.ObjectMapper.Mappers;
using Xunit;

namespace UnitTests.Mappings.Polymorphic
{
    public class PolymorphicTests
    {
        [Fact]
        public void SimpleCustomHeirarchyMappingTest()
        {
            // Arrange
            SourceB source = new SourceB
            {
                FirstName = "John",
                LastName = "Doe",
                Age = 37
            };

            TinyMapper.Bind<SourceA, TargetA>(n => n.Bind(from => from.FirstName, to => to.Name));

            // Act
            var target = TinyMapper.Map<TargetA>(source);

            // Assert
            Assert.NotNull(target);
            Assert.Same(source.FirstName, target.Name);
        }

        [Fact]
        public void SimpleCustomHeirarchyMappingTest2()
        {
            // Arrange
            SourceB source = new SourceB
            {
                FirstName = "John",
                LastName = "Doe",
                Age = 37
            };

            TargetB target = new TargetB();
            TinyMapper.Bind<SourceA, TargetA>(n => n.Bind(from => from.FirstName, to => to.Name));

            // Act
            TinyMapper.Map<SourceA, TargetA>(source, target);

            // Assert
            Assert.NotNull(target);
            Assert.Same(source.FirstName, target.Name);
        }

        [Fact]
        public void SimpleCustomInterfaceMappingTest()
        {
            // Arrange
            SourceB source = new SourceB
            {
                FirstName = "John",
                LastName = "Doe",
                Age = 37
            };

            TinyMapper.Bind<ISource, TargetA>(n => n.Bind(from => from.FirstName, to => to.Name));

            // Act
            var target = TinyMapper.Map<TargetA>(source);

            // Assert
            Assert.NotNull(target);
            Assert.Same(source.FirstName, target.Name);
        }

        [Fact]
        public void ReversePolymorphicShouldFailWithException()
        {
            // Arrange
            TinyMapper.Config(n => n.EnableAutoBinding = false);

            SourceB source = new SourceB
            {
                FirstName = "John",
                LastName = "Doe",
                Age = 37
            };

            // Act / Assert
            Assert.Throws<MappingException>(() => TinyMapper.Map<TargetB>(source));
        }

        [Fact]
        public void DisablePolymorphicMappingTest()
        {
            // Arrange
            TinyMapper.Config(n => { n.EnablePolymorphicMapping = false; });

            SourceB source = new SourceB
            {
                FirstName = "John",
                LastName = "Doe",
                Age = 37
            };

            // This binding will be ignored because it will not be recognised as a binding for these types
            TinyMapper.Bind<SourceA, TargetA>(n => n.Bind(from => from.FirstName, to => to.Name));

            // Act
            var target = TinyMapper.Map<TargetA>(source);

            // Assert
            Assert.NotEqual(source.FirstName, target.Name);
        }
    }


    public class SourceA
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }


    public class SourceB : SourceA, ISource
    {
        public int Age { get; set; }
    }


    public interface ISource
    {
        int Age { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
    }


    public class TargetA
    {
        public string Name { get; set; }
    }


    public class TargetB : TargetA
    {
        public int Age { get; set; }
    }
}
