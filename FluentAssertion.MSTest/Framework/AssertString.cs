using System;

namespace FluentAssertion.MSTest.Framework
{
    /// <summary>
    /// Represent a single string being subject of an assertion
    /// </summary>
    public class AssertString : AssertObject<string>
    {
        public AssertString(string subject) : base(subject) { }
    }
}

// TODO
// - Contains
// - IsEqualTo (ignore case option)