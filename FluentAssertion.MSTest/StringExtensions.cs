using FluentAssertion.MSTest.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;

namespace FluentAssertion.MSTest
{
    public static class StringExtensions
    {
        public static AssertObject<string> StartsWith(this AssertObject<string> assertObject, string prefix, string message = null)
        {
            if(!assertObject.Object.StartsWith(prefix))
            {
                Assert.Fail(message ?? $"The string '{assertObject.Object}' does not start with '{prefix}'");
            }
            return assertObject;
        }

        public static AssertObject<string> StartsWith(this AssertObject<string> assertObject, string prefix, bool ignoreCase, CultureInfo culture, string message = null)
        {
            if (!assertObject.Object.StartsWith(prefix, ignoreCase, culture))
            {
                Assert.Fail(message ?? $"The string '{assertObject.Object}' does not start with '{prefix}'");
            }
            return assertObject;
        }

        public static AssertObject<string> StartsWith(this AssertObject<string> assertObject, string prefix, StringComparison comparisonType, string message = null)
        {
            if (!assertObject.Object.StartsWith(prefix, comparisonType))
            {
                Assert.Fail(message ?? $"The string '{assertObject.Object}' does not start with '{prefix}'");
            }
            return assertObject;
        }
    }
}
