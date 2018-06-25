using System;
using container.Extensions;

namespace container.Builders
{
    /// <summary>
    /// Base builder, needed so that fields of mappers are re-created during each mapping process
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseBuilder<T> where T : new()
    {
        /// <summary>
        /// Creates a new instance of self
        /// </summary>
        /// <returns></returns>
        public static T New() => new T();

        /// <summary>
        /// Execute the action and return
        /// </summary>
        /// <param name="return"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        protected static TS Run<TS>(TS @return, params Action[] action)
        {
            action.ForEach(x => x());
            
            return @return;
        }
    }
}
