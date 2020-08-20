using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using FluentAssertion.MSTest;

namespace MSTest.FluentAssertions.Tests
{
    [TestClass]
    public class AssertCollectionAssertionsTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            var typedCollection = new List<string>();

           // Assert.That.This(typedCollection).IsOfType(typeof(string));
        }
    }
}
