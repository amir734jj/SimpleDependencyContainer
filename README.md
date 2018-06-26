# SimpleDependencyContainer

Very Simple C# Dependency Container, Includes features such as:
1- Dependency resolver with Singletons options
2- Conventional dependency resolving using both interfaces or concrete classes
3- Constructor dependency injection
4- Assembly scanning

It does not (yet ...):
1- Resolve recursive dependencies
2- `Lazy<T>` or `Func<T>` support as does `StructureMap`

Container interface:

```csharp
// Initialize the IoC container
var container = SimpleDependencyContainer.New()
    // Register a dependency
    .RegisterDependency(_ => _.SetType(typeof(FlatModelSource))
        // Set Args needed to be used to pass to it's constructor in order to initialize
        .SetArgs(new object[] {123, "Test"})
        // Dependency is Singleton hence initialize it once
        .SetSingleton(true)
        // Complete the IDependency builder
        .Compile())
    // For constructor injected parameter of type interface IDummyInterface use concrete class DummyClass
    .AddMap<IDummyInterface, DummyClass>()
    // Scan assembly to populate the interface/class mapper with more key/values
    .Scan("Some project name's assembly");

// Resolve an instance of type ModelWithInterface
var instance = container.Resolve<ModelWithInterface>();
```
