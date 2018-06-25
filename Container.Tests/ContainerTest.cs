using AutoFixture;
using container;
using container.Builders;
using Container.Tests.Models;
using Xunit;

namespace Container.Tests
{
    public class ContainerTest
    {
        private readonly Fixture _fixture;

        public ContainerTest()
        {
            _fixture = new Fixture();
        }
        
        [Fact]
        public void Test__GetInstance()
        {
            // Arrange
            var expected = _fixture.Create<FlatModelSource>();
            var dependency = DependencyBuilder.New().SetType(typeof(FlatModelSource))
                .SetArgs(new object[] { expected.Name, expected.Age, expected.DateOfBith })
                .Compile();

            var container = SimpleDependencyContainer.New().Register(_ => dependency);

            // Act
            var flatModelSource = container.GetInstance<FlatModelSource>();

            // Assert
            Assert.Equal(expected, flatModelSource);
        }
        
        [Fact]
        public void Test__GetInstanceSingleton()
        {
            // Arrange
            var expected = _fixture.Create<FlatModelSource>();
            var dependency = DependencyBuilder.New().SetType(typeof(FlatModelSource))
                .SetArgs(new object[] { expected.Name, expected.Age, expected.DateOfBith })
                .SetSingleton(true)
                .Compile();

            var container = SimpleDependencyContainer.New().Register(_ => dependency);

            // Act
            var flatModelSource = container.GetInstance<FlatModelSource>();
            flatModelSource.Name = "Test";

            // Assert
            Assert.Equal("Test", container.GetInstance<FlatModelSource>().Name);
        }
        
        [Fact]
        public void Test__RegisterInstance()
        {
            // Arrange
            var expected = _fixture.Create<FlatModelSource>();
            var container = SimpleDependencyContainer.New().RegisterInstance(expected);

            // Act
            var flatModelSource = container.GetInstance<FlatModelSource>();

            // Assert
            Assert.Equal(expected, flatModelSource);
        }
    }
}