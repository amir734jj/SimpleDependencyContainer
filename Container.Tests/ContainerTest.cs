using System;
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
            var container = SimpleDependencyContainer.New()
                .RegisterDependency(x => x.SetType(typeof(FlatModelSource))
                .SetArgs(new object[] { expected.Name, expected.Age, expected.DateOfBith })
                .Compile());

            // Act
            var flatModelSource = container.Resolve<FlatModelSource>();

            // Assert
            Assert.Equal(expected, flatModelSource);
        }
        
        [Fact]
        public void Test__GetInstanceSingleton()
        {
            // Arrange
            var expected = _fixture.Create<FlatModelSource>();
            var container = SimpleDependencyContainer.New()
                .RegisterDependency(_ => _.SetType(typeof(FlatModelSource))
                .SetArgs(new object[] { expected.Name, expected.Age, expected.DateOfBith })
                .SetSingleton(true)
                .Compile());

            // Act
            var flatModelSource = container.Resolve<FlatModelSource>();
            flatModelSource.Name = "Test";

            // Assert
            Assert.Equal("Test", container.Resolve<FlatModelSource>().Name);
        }
        
        [Fact]
        public void Test__RegisterInstance()
        {
            // Arrange
            var expected = _fixture.Create<FlatModelSource>();
            var container = SimpleDependencyContainer.New().RegisterInstance(expected);

            // Act
            var flatModelSource = container.Resolve<FlatModelSource>();

            // Assert
            Assert.Equal(expected, flatModelSource);
        }

        [Fact]
        public void Test__TypeMap()
        {
            // Arrange
            var container = SimpleDependencyContainer.New().AddMap<IDummyInterface, DummyClass>();
            
            // Act
            var model = container.Resolve<ModelWithInterface>();
            
            // Assert
            Assert.NotNull(model);
        }

        [Fact]
        public void Test__CircularDependency()
        {
            // Arrange
            var container = SimpleDependencyContainer.New().AddMap<ICircularDependency, CircularDependency>();

            // Act
            var exception = Record.Exception(() => container.Resolve<CircularDependency>());

            // Assert
            Assert.IsType<ArgumentException>(exception);
        }

        [Fact]
        public void Test__ResolvableCircularDependency()
        {
            // Arrange
            var container = SimpleDependencyContainer.New();
            
            // Act
            var simple = container.Resolve<ResolvableCircularDependency>();
            var lazy = container.Resolve<ResolvableCircularDependencyLazy>();
            var func = container.Resolve<ResolvableCircularDependencyFunc>();
            
            // Assert
            Assert.NotNull(simple);
            Assert.NotNull(func.ResolvableCircularDependency());
            Assert.NotNull(lazy.ResolvableCircularDependency.Value);
        }
    }
}