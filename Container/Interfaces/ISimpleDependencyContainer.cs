using System;
using System.Reflection;
using container.Builders;
using container.Models;

namespace container.Interfaces
{
    public interface ISimpleDependencyContainer
    {
        /// <summary>
        /// Resolves Type T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T Resolve<T>();

        /// <summary>
        /// Resolves dynamic type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        object Resolve(Type type);

        /// <summary>
        /// Queries dependency container
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        IDependency QueryDependency(Func<IDependency, bool> filter);
        
        /// <summary>
        /// Registers type using IDependencyBuilder
        /// </summary>
        /// <param name="dependency"></param>
        /// <returns></returns>
        ISimpleDependencyContainer RegisterDependency(Func<IDependencyBuilder, IDependency> dependency);

        /// <summary>
        /// Removes dependency
        /// </summary>
        /// <param name="dependency"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        ISimpleDependencyContainer RemoveDependency<T>(IDependency dependency);

        /// <summary>
        /// Scans assembly given assembly name to populate type maps
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        ISimpleDependencyContainer Scan(string assemblyName);

        /// <summary>
        /// Scans assembly given assembly to populate type maps
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        ISimpleDependencyContainer Scan(Assembly assembly);
        
        /// <summary>
        /// Registers an Instance
        /// </summary>
        /// <param name="instance"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        ISimpleDependencyContainer RegisterInstance<T>(T instance);
        
        /// <summary>
        /// Removes an instance
        /// </summary>
        /// <param name="instance"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        ISimpleDependencyContainer RemoveInstance<T>(T instance);
        
        /// <summary>
        /// Adds a type map
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        ISimpleDependencyContainer AddMap(Type source, Type destination);

        /// <summary>
        /// Removes type map
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        ISimpleDependencyContainer RemoveMap(Type source);

        /// <summary>
        /// Adds map given static types
        /// </summary>
        /// <typeparam name="TS"></typeparam>
        /// <typeparam name="TD"></typeparam>
        /// <returns></returns>
        ISimpleDependencyContainer AddMap<TS, TD>();

        /// <summary>
        /// Removes a map given static type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        ISimpleDependencyContainer RemoveMap<T>();
    }
}