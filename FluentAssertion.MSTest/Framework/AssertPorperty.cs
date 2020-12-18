using System;

namespace FluentAssertion.MSTest.Framework
{
    /// <summary>
    /// Represent a single property of an AssertObject being subject of an assertion
    /// </summary>
    /// <typeparam name="TO">Type if the parent object of the tested property</typeparam>
    /// <typeparam name="TP">Type of the property under test</typeparam>
    public class AssertPorperty<TO,TP>
    {
        public readonly TP Property;
        public readonly TO ParentObject;

        public AssertPorperty(TO parentObject, TP subject)
        {
            Property = subject;
            ParentObject = parentObject;
        }
    }
}