using System;
using Nelibur.ObjectMapper;
using Nelibur.ObjectMapper.Mappers;
using Xunit;

namespace UnitTests.Mappings.Polymorphic
{
    public sealed class PolymorphicTests
    {
        [Fact]
        public void SimpleCustomHeirarchyMappingTest()
        {
            // Arrange
            var source = new SourceB
            {
                FirstName = "John",
                LastName = "Doe",
                Age = 37
            };

            TinyMapper.Bind<SourceA, TargetA>(x => x.Bind(from => from.FirstName, to => to.Name));

            // Act
            var target = TinyMapper.Map<TargetA>(source);

            // Assert
            Assert.NotNull(target);
            Assert.Equal(source.FirstName, target.Name);
        }

        [Fact]
        public void SimpleCustomHeirarchyMappingTest2()
        {
            // Arrange
            var source = new SourceB
            {
                FirstName = "John",
                LastName = "Doe",
                Age = 37
            };

            var target = new TargetB();
            TinyMapper.Bind<SourceA, TargetA>(x => x.Bind(from => from.FirstName, to => to.Name));

            // Act
            TinyMapper.Map<SourceA, TargetA>(source, target);

            // Assert
            Assert.NotNull(target);
            Assert.Equal(source.FirstName, target.Name);
            Assert.Equal(0, target.Age);
        }

        [Fact]
        public void SimpleCustomInterfaceMappingTest()
        {
            // Arrange
            var source = new SourceB
            {
                FirstName = "John",
                LastName = "Doe",
                Age = 37
            };

            TinyMapper.Bind<ISource, TargetA>(x => x.Bind(from => from.FirstName, to => to.Name));

            // Act
            var target = TinyMapper.Map<TargetA>(source);

            // Assert
            Assert.NotNull(target);
            Assert.Equal(source.FirstName, target.Name);
        }

        [Fact]
        public void ReversePolymorphicShouldFailWithException()
        {
            // Arrange
            TinyMapper.Config(x => x.EnableAutoBinding = false);

            var source = new SourceB
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
            TinyMapper.Config(x => { x.EnablePolymorphicMapping = false; x.EnableAutoBinding = true; });

            var source = new SourceB
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


    public sealed class SourceB : SourceA, ISource
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

    public sealed class TargetB : TargetA
    {
        public int Age { get; set; }
    }
}
