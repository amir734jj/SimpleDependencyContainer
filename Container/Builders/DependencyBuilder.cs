using System;
using System.Collections.Generic;
using container.Interfaces;
using container.Models;

namespace container.Builders
{
    public class DependencyBuilder: BaseBuilder<DependencyBuilder>, IDependencyBuilder
    {
        /// <summary>
        /// Struct to hold builder info
        /// </summary>
        private class DependencyStruct: IDependency
        {
            public Type Type { get; set;  }
            
            public string Name { get; set; }
           
            public object[] Args { get; set; }

            public bool Singleton { get; set; }

            public DependencyStruct()
            {
                Type = null;
                Name = null;
                Args = null;
                // By default all dependencies are singleton
                Singleton = true;
            }
            
            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="type"></param>
            /// <param name="name"></param>
            /// <param name="args"></param>
            /// <param name="singleton"></param>
            public DependencyStruct(Type type, string name, object[] args, bool singleton): this()
            {
                Type = type;
                Name = name;
                Args = args;
                Singleton = singleton;
            }
        }

        /// <summary>
        /// Initialize the struct to hold builder info
        /// </summary>
        private DependencyStruct _dependencyStruct = new DependencyStruct();

        /// <summary>
        /// Sets dependency type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public IDependencyBuilder SetType(Type type) => Run(this, () => _dependencyStruct.Type = type);
        
        /// <summary>
        /// Sets dependency name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IDependencyBuilder SetName(string name) => Run(this, () => _dependencyStruct.Name = name);

        /// <summary>
        /// Sets dependency args
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public IDependencyBuilder SetArgs(object[] args) => Run(this, () => _dependencyStruct.Args = args);

        /// <summary>
        /// Sets dependency singleton
        /// </summary>
        /// <param name="singleton"></param>
        /// <returns></returns>
        public IDependencyBuilder SetSingleton(bool singleton) => Run(this, () => _dependencyStruct.Singleton = singleton);
        
        /// <summary>
        /// Returns IDependency
        /// </summary>
        /// <returns></returns>
        public IDependency Compile() => Run(new Dependency
        {
            Name = !string.IsNullOrEmpty(_dependencyStruct.Name) ? _dependencyStruct.Name : _dependencyStruct.Type.Name,
            Args = _dependencyStruct.Args,
            Type = _dependencyStruct.Type,
            Singleton = _dependencyStruct.Singleton
        }, () => Validate(_dependencyStruct));

        /// <summary>
        /// Returns 
        /// </summary>
        /// <param name="dependency"></param>
        /// <returns></returns>
        public IDependencyBuilder FromDependency(IDependency dependency) => Run(this, () => _dependencyStruct = new DependencyStruct(dependency.Type, dependency.Name, dependency.Args, dependency.Singleton));
        
        /// <summary>
        /// Validate dependency
        /// </summary>
        /// <param name="dependencyStruct"></param>
        /// <exception cref="ArgumentException"></exception>
        private static void Validate(DependencyStruct dependencyStruct)
        {
            if (dependencyStruct.Type == null)
            {
                throw new ArgumentException("Type of dependency cannot be null");
            }
        }
    }
}