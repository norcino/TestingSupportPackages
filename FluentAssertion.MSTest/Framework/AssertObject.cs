using System;

namespace FluentAssertion.MSTest.Framework
{
    /// <summary>
    /// Represent a single object being subject of an assertion
    /// </summary>
    /// <typeparam name="T">Type of the object under test</typeparam>
    public class AssertObject<T>
    {
        public readonly T Object;

        public AssertObject(T subject)
        {
            Object = subject;
        }
    }    
}