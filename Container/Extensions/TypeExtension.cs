using System;
using System.Collections.Generic;
using System.Linq;

namespace container.Extensions
{
    /// <summary>
    /// type extensions
    /// </summary>
    public static class TypeExtension
    {
        /// <summary>
        /// Returns true if type if system type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsSystemType(this Type type) => type.Namespace != null && type.Namespace.StartsWith("System");

        /// <summary>
        /// Instantiates an object given dynamically defined type
        /// </summary>
        /// <param name="type"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static object Instantiate(this Type type, params object[] args) => type.IsClass ? Activator.CreateInstance(type, args) : throw new ArgumentException($"Cannot Instantiate Interface: {type.Name}");
    }
}