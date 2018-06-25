using System;
using System.Collections.Generic;

namespace container.Interfaces
{
    public interface IDependency
    {
        Type Type { get; }

        string Name { get; }
        
        object[] Args { get; }
        
        bool Singleton { get; }
    }
}