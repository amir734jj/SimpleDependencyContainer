using System;
using System.Collections.Generic;
using System.Linq;
using container.Builders;
using container.Interfaces;

namespace container
{
    public class SimpleDependencyContainer: BaseBuilder<SimpleDependencyContainer>, ISimpleDependencyContainer
    {
        /// <summary>
        /// Default container
        /// </summary>
        public static ISimpleDependencyContainer DefaultContainer = New();

        private readonly Dictionary<IDependency, object> _instances;
        
        private readonly HashSet<IDependency> _container;
        
        public SimpleDependencyContainer()
        {
            // Initialize the container
            _container = new HashSet<IDependency>();

            // Initialize instance container
            _instances = new Dictionary<IDependency, object>();
        }

        /// <summary>
        /// Returns an instance of dependency
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetInstance<T>() => (T) GetInstance(typeof(T));

        /// <summary>
        /// Returns an instance of dependency
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public object GetInstance(Type type)
        {
            var dependency = GetDependency(x => x.Type == type);

            if (dependency == null)
            {
                throw new ArgumentException($"Dependency '{type.Name}' not found.");
            }

            if (_instances.ContainsKey(dependency))
            {
                return _instances[dependency];
            }
            
            var instance = Activator.CreateInstance(dependency.Type, dependency.Args);

            if (dependency.Singleton)
            {
                _instances.Add(dependency, instance);
            }

            return instance;
        }

        /// <summary>
        /// Returns dependency
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public IDependency GetDependency(Func<IDependency, bool> filter) => _container.FirstOrDefault(filter);

        /// <summary>
        /// Registers a dependency
        /// </summary>
        /// <param name="dependency"></param>
        /// <returns></returns>
        public ISimpleDependencyContainer Register(Func<IDependencyBuilder, IDependency> dependency) => Run(this, () => _container.Add(dependency(DependencyBuilder.New())));

        /// <summary>
        /// Registers an instance
        /// </summary>
        /// <param name="instance"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public ISimpleDependencyContainer RegisterInstance<T>(T instance) => Run(this, () => Register(_ => _.SetType(typeof(T)).SetSingleton(true).Compile()), () => _instances.Add(GetDependency(x => x.Type == typeof(T)), instance));
        
        /// <summary>
        /// Removes a dependency
        /// </summary>
        /// <param name="dependency"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public ISimpleDependencyContainer Remove<T>(IDependency dependency) => Run(this, () => _container.Remove(dependency));
    }
}