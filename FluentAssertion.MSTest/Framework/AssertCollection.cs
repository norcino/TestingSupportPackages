using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;

namespace FluentAssertion.MSTest.Framework
{
    /// <summary>
    /// Class used to create fluent assertions for enumerables.
    /// To access the inner collection containing a copy of the enumerable under test, use the property Collection.
    /// Note that the collection is immutable and the original collection will never be changed
    /// </summary>
    /// <typeparam name="T">Generic type for the collection</typeparam>
    public class AssertCollection<T>
    {
        public readonly ReadOnlyCollection<T> Collection;

        public AssertCollection(IEnumerable<T> enumerable)
        {
            Collection = new ReadOnlyCollection<T>(enumerable.ToList());
        }
    }
}
