using System;

namespace Container.Tests.Models
{
    public class FlatModelSource
    {
        public string Name { get; set; }

        public int Age { get; set; }

        public DateTime DateOfBith { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="age"></param>
        /// <param name="dateOfBith"></param>
        public FlatModelSource(string name, int age, DateTime dateOfBith)
        {
            Name = name;
            Age = age;
            DateOfBith = dateOfBith;
        }

        /// <summary>
        /// Auto generated
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        private bool Equals(FlatModelSource other)
        {
            return string.Equals(Name, other.Name) && Age == other.Age && DateOfBith.Equals(other.DateOfBith);
        }

        /// <summary>
        /// Auto generated
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == this.GetType() && Equals((FlatModelSource) obj);
        }

        /// <summary>
        /// HashCode not implemented
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => throw new NotImplementedException();
    }
}