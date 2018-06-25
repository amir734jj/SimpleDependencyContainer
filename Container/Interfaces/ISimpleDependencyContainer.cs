using System;
using container.Builders;
using container.Models;

namespace container.Interfaces
{
    public interface ISimpleDependencyContainer
    {
        T GetInstance<T>();

        object GetInstance(Type type);

        IDependency GetDependency(Func<IDependency, bool> filter);
        
        ISimpleDependencyContainer Register(Func<IDependencyBuilder, IDependency> dependency);

        ISimpleDependencyContainer Remove<T>(IDependency dependency);
    }
}