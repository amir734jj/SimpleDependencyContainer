using System;
using System.Reflection;
using container.Builders;
using container.Models;

namespace container.Interfaces
{
    public interface ISimpleDependencyContainer
    {
        T Resolve<T>();

        object Resolve(Type type);

        IDependency QueryDependency(Func<IDependency, bool> filter);
        
        ISimpleDependencyContainer RegisterDependency(Func<IDependencyBuilder, IDependency> dependency);

        ISimpleDependencyContainer RemoveDependency<T>(IDependency dependency);

        ISimpleDependencyContainer Scan(string assemblyName);

        ISimpleDependencyContainer Scan(Assembly assembly);
        
        ISimpleDependencyContainer RegisterInstance<T>(T instance);
        
        ISimpleDependencyContainer RemoveInstance<T>(T instance);
        
        ISimpleDependencyContainer AddMap(Type source, Type destination);

        ISimpleDependencyContainer RemoveMap(Type source);
    }
}