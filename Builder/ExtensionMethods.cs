using System.Collections.Generic;
using System.Linq;

namespace Builder
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Helper method that allows to convert an object into a generic List<>
        /// </summary>
        /// <typeparam name="TE">Type of the object</typeparam>
        /// <param name="singleObject">Instance of the object</param>
        /// <returns>List containing only the given object</returns>
        public static List<TE> AsList<TE>(this TE singleObject)
        {
            return new List<TE> { singleObject };
        }

        /// <summary>
        /// Helper method that allows to convert an object into an IQueryable
        /// </summary>
        /// <typeparam name="TE">Type of the object</typeparam>
        /// <param name="singleObject">Instance of the object</param>
        /// <returns>IQueryable containing the given object</returns>
        public static IQueryable<TE> AsQueryable<TE>(this TE singleObject)
        {
            return new List<TE> { singleObject }.AsQueryable<TE>();
        }
    }
}
