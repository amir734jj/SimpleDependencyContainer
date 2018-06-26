using System;
using System.Collections.Generic;
using container.Builders;

namespace container.Interfaces
{
    public interface IDependencyBuilder
    {
        IDependencyBuilder SetType(Type type);

        IDependencyBuilder SetType<T>();
        
        IDependencyBuilder SetName(string name);
       
        IDependencyBuilder SetArgs(object[] args);

        IDependencyBuilder SetSingleton(bool singleton);
        
        IDependency Compile();
        
        IDependencyBuilder FromDependency(IDependency dependency);
    }
}