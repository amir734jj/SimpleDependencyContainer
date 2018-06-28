using System;

namespace Container.Tests.Models
{
    public class ResolvableCircularDependency
    {
        public ResolvableCircularDependencyLazy ResolvableCircularDependencyLazy { get; }
        
        public ResolvableCircularDependency(ResolvableCircularDependencyLazy resolvableCircularDependencyLazy)
        {
            ResolvableCircularDependencyLazy = resolvableCircularDependencyLazy;
        }
    }
    
    public class ResolvableCircularDependencyLazy
    {
        public Lazy<ResolvableCircularDependency> ResolvableCircularDependency { get; }

        public ResolvableCircularDependencyLazy(Lazy<ResolvableCircularDependency> resolvableCircularDependency)
        {
            ResolvableCircularDependency = resolvableCircularDependency;
        }
    }
    
    public class ResolvableCircularDependencyFunc
    {
        public Func<ResolvableCircularDependency> ResolvableCircularDependency { get; }

        public ResolvableCircularDependencyFunc(Func<ResolvableCircularDependency> resolvableCircularDependency)
        {
            ResolvableCircularDependency = resolvableCircularDependency;
        }
    }
}