using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using container.Builders;
using container.Extensions;
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

        private readonly Dictionary<Type, Type> _maps;
        
        public SimpleDependencyContainer()
        {
            // Initialize the container
            _container = new HashSet<IDependency>();

            // Initialize instance container
            _instances = new Dictionary<IDependency, object>();
            
            // Initialuze type map interface -> class
            _maps = new Dictionary<Type, Type>();
        }

        /// <summary>
        /// Lazy resolves a type, returns a Func that when invoked will return the intended object
        /// </summary>
        /// <param name="genericType"></param>
        /// <returns></returns>
        private object LazyResolve(Type genericType)
        {            
            // Create expression body
            var body = Expression.Call(
                // Instance
                Expression.Constant(this),
                // MethodInfo
                typeof(SimpleDependencyContainer).GetMethod("Resolve", new[] { typeof(Type) }),
                // Argument IN of method call
                Expression.Constant(genericType)
            );

            // Cast Object to intended type
            var fullBody = Expression.Convert(body, genericType);
            
            // Create generic Func
            var delegateType = typeof(Func<>).MakeGenericType(genericType);
            
            // Create lambda expression that does not take any arguments IN given type and body
            var lambda = Expression.Lambda(delegateType, fullBody);
            
            // Convert lambda to Func
            return lambda.Compile();
        }

        /// <summary>
        /// Resolves a dependency
        /// </summary>
        /// <param name="type"></param>
        /// <param name="alreadyResolvedList"></param>
        /// <returns></returns>
        private object Resolve(Type type, List<Type> alreadyResolvedList)
        {
            // Initialize already resolved list
            alreadyResolvedList = alreadyResolvedList ?? new List<Type>();

            // Check for circular dependency
            if (alreadyResolvedList.Contains(type))
            {
                throw new ArgumentException("Circular Dependency Detected.");
            }

            // Add current type to resolved dependencies
            alreadyResolvedList.Add(type);

            // Try to resolve a type if it is in type map (i.e. interface/class map)
            type = ResolveType(type);
            
            // Convert type to dependency
            var dependency = TypeToDependency(type);
            
            // Check if we already have the instance
            if (_instances.ContainsKey(dependency))
            {
                return _instances[dependency];
            }
            
            // Test whethe type is generic
            if (type.IsGenericType && type.GetGenericArguments().Length == 1)
            {
                // try to get the generic parameter of type
                var genericType = type.GetGenericArguments().First();

                // if type is a Func then user is probably trying to resolve circular dependency
                if (type.GetGenericTypeDefinition() == typeof(Func<>))
                {
                    return LazyResolve(genericType);
                }

                // if type is a Lazy, then it is a similar situation
                if (type.GetGenericTypeDefinition() == typeof(Lazy<>))
                {
                    return typeof(Lazy<>).InstantiateGeneric(genericType, LazyResolve(genericType));
                }
            }        
            
            // We cannot instantiate system type undless explicitly registered
            if (type.IsSystemType())
            {
                throw new ArgumentException($"Cannot instantiate system type: {type.Name}");
            }

            
            // Get constructor info
            var constructorInfo = type.GetConstructors().FirstOrDefault(x => x.GetParameters().Any());

            // If constructor does not have any parameters then just instantiate the type
            if (constructorInfo == null)
            {
                // Instantiate type
                var instance = type.Instantiate();
                
                // Register instance of it is singleton
                if (dependency.Singleton) _instances.Add(dependency, instance);

                return instance;
            }
            else
            {
                // Get arguments of constructor and try to resolve them
                var args = dependency.Args ?? constructorInfo.GetParameters()
                               // Get types of constructor parameters
                               .Select(x => x.ParameterType)
                               // Resolve the type or convert interface to class because we cannot instantiate an interface
                               .Select(ResolveType)
                               // Recursively resolve the type
                               .Select(x => Resolve(x, alreadyResolvedList))
                               // Convert to object array
                               .ToArray();

                // Instantiate the type with resolved parameters
                var instance = dependency.Type.Instantiate(args);

                // Register instance if it is singleton
                if (dependency.Singleton) _instances.Add(dependency, instance);

                // Return the instance
                return instance;
            }
        }

        /// <summary>
        /// Resolve the type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Resolve<T>() => (T) Resolve(typeof(T));

        /// <summary>
        /// Resolves a dependency
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public object Resolve(Type type) => Resolve(type, null);

        /// <summary>
        /// Converts type to dependency
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private IDependency TypeToDependency(Type type) => QueryDependency(x => x.Type == type) ?? Run(() => TypeToDependency(type), () => RegisterDependency(x => x.SetType(type).Compile()));

        /// <summary>
        /// Tries to resolve a type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private Type ResolveType(Type type) => _maps.ContainsKey(type) ? _maps[type] : type;
        
        /// <summary>
        /// Returns dependency
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public IDependency QueryDependency(Func<IDependency, bool> filter) => _container.FirstOrDefault(filter);

        /// <summary>
        /// Registers a dependency
        /// </summary>
        /// <param name="dependency"></param>
        /// <returns></returns>
        public ISimpleDependencyContainer RegisterDependency(Func<IDependencyBuilder, IDependency> dependency) => Run(this, () => _container.Add(dependency(DependencyBuilder.New())));

        /// <summary>
        /// Removes a dependency
        /// </summary>
        /// <param name="dependency"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public ISimpleDependencyContainer RemoveDependency<T>(IDependency dependency) => Run(this, () => _container.Remove(dependency));

        /// <summary>
        /// Registers an instance
        /// </summary>
        /// <param name="instance"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public ISimpleDependencyContainer RegisterInstance<T>(T instance) => Run(this, () => _instances.Add(TypeToDependency(typeof(T)), instance));
        
        /// <inheritdoc />
        /// <summary>
        /// Removes an instance
        /// </summary>
        /// <param name="instance"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public ISimpleDependencyContainer RemoveInstance<T>(T instance) => Run(this, () => _instances.Remove(TypeToDependency(typeof(T))));

        /// <summary>
        /// Scans assembly 
        /// </summary>
        /// <returns></returns>
        public ISimpleDependencyContainer Scan(string assemblyName) => Scan(Assembly.Load(assemblyName));
        
        /// <inheritdoc />
        /// <summary>
        /// Scans assembly 
        /// </summary>
        /// <returns></returns>
        public ISimpleDependencyContainer Scan(Assembly assembly) => Run(this, () =>
        {
            var types = assembly.GetTypes();

            types.Where(x => x.IsInterface).ToDictionary(x => x, x => types.Where(y => y.IsClass)
                // Get matching class type
                .FirstOrDefault(x.IsAssignableFrom))
                // Remove interfaces without concrete implementation
                .Where(x => x.Value != null)
                // Add the type map to dictionary
                .ForEach(x => _maps.Add(x.Key, x.Value));
        });

        /// <inheritdoc />
        /// <summary>
        /// Adds a type map
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        public ISimpleDependencyContainer AddMap(Type source, Type destination) => Run(this, () => _maps.Add(source, destination));

        /// <inheritdoc />
        /// <summary>
        /// Tries to remove a type
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public ISimpleDependencyContainer RemoveMap(Type source) => Run(this, () => _maps.Remove(source));

        /// <summary>
        /// Adds map given static types
        /// </summary>
        /// <typeparam name="TS"></typeparam>
        /// <typeparam name="TD"></typeparam>
        /// <returns></returns>
        public ISimpleDependencyContainer AddMap<TS, TD>() => AddMap(typeof(TS), typeof(TD));

        /// <summary>
        /// Removes a map given static type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public ISimpleDependencyContainer RemoveMap<T>() => RemoveMap(typeof(T));
    }
}
