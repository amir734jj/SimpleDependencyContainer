# SimpleDependencyContainer

### Very Simple C# Dependency Container, Includes features such as:
1. Dependency resolver with Singletons options
2. Conventional dependency resolving using both interfaces or concrete classes
3. Constructor dependency injection
4. Assembly scanning

### It does not (yet ...):
1. Resolve recursive dependencies
2. `Lazy<T>` or `Func<T>` support as does `StructureMap`

### How does it work?
This library will recursively resolve type and constructor parameters until it reaches a type that is a System type or its namespace starts with `System`, then at this point it requires an instance to be manually registered before it can continue any further. This is the same strategy behind all IoCs.

### Examples:

```csharp
// Initialize the IoC container
var container = SimpleDependencyContainer.New()
    // Register a dependency
    .RegisterDependency(_ => _.SetType<FlatModelSource>()
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
