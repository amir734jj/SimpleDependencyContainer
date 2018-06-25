using System;
using System.Collections.Generic;
using container.Interfaces;

namespace container.Models
{
    public class Dependency : IDependency
    {
        public Type Type { get; set; }
        
        public string Name { get; set; }
        
        public object[] Args { get; set; }
        
        public bool Singleton { get; set; }

        /// <summary>
        /// Equals
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(Dependency other)
        {
            return Type == other.Type && string.Equals(Name, other.Name) && Equals(Args, other.Args) && Singleton == other.Singleton;
        }

        /// <summary>
        /// Equals
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Dependency) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Type != null ? Type.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Args != null ? Args.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Singleton.GetHashCode();
                return hashCode;
            }
        }
    }
}