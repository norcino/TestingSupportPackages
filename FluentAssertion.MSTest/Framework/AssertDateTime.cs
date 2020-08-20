using System;

namespace FluentAssertion.MSTest.Framework
{
    /// <summary>
    /// Represent a single DateTime being subject of an assertion
    /// </summary>
    public class AssertDateTime : AssertObject<DateTime>
    {
        public AssertDateTime(DateTime subject) : base(subject) { }
    }
}