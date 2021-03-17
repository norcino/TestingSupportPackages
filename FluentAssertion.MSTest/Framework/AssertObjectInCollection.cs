using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;

namespace FluentAssertion.MSTest.Framework
{
    public class AssertObjectInCollection<T>
    {
        public readonly ReadOnlyCollection<T> Collection;
        public readonly T Element;

        public AssertObjectInCollection(AssertCollection<T> enumerable, T element)
        {
            Collection = new ReadOnlyCollection<T>(enumerable.Collection);
            Element = element;
        }
    }
}
