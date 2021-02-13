using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using FluentAssertion.MSTest;
using FluentAssertion.MSTest.Tests;
using System.Collections;
using AnonymousData;

namespace MSTest.FluentAssertions.Tests
{
    [TestClass]
    public class AssertCollectionAssertionsTests
    {
        [TestMethod]
        public void AreSameAs_should_succeed_when_collection_has_Same_elements()
        {
            var element1 = Any.Of<SutPoco>();
            var element2 = Any.Of<SutPoco>();
            var element3 = Any.Of<SutPoco>();
            var pocos = new List<SutPoco> { element1, element2, element3 };
            var sameElement1 = element1.Clone();
            var sameElement2 = element2.Clone();
            var sameElement3 = element3.Clone();
            var otherPocos = new List<SutPoco> { sameElement1, sameElement2, sameElement3 };
            Assert.That.These(pocos).AreSameAs(otherPocos);
        }

        [TestMethod]
        public void Contains_should_succeed_when_object_item_exists_in_collection()
        {
            var expectedElement = Any.Of<SutPoco>();
            var collection = new List<SutPoco> { expectedElement, Any.Of<SutPoco>(), Any.Of<SutPoco>() };

            Assert.That.These(collection).Contains(expectedElement);
        }

        [TestMethod]
        public void Contains_should_fail_when_object_item_exists_in_collection2()
        {
            var expectedElement = Any.Of<SutPoco>();
            var collection = new List<SutPoco> { expectedElement.Clone(), Any.Of<SutPoco>(), Any.Of<SutPoco>() };

            Assert.That.These(collection).Contains(expectedElement);
        }

        [TestMethod]
        public void Contains_should_fail_when_object_element_not_contained_in_the_collection()
        {
            var missingElement = Any.Of<SutPoco>();
            var collection = new List<SutPoco> { Any.Of<SutPoco>(), Any.Of<SutPoco>() };

            try
            {
                Assert.That.These(collection).Contains(missingElement);
            }
            catch (AssertFailedException e)
            {
                Assert.AreEqual(
                    "Assert.Fail failed. The collection did not contain the expected element",
                    e.Message);
            }
        }

        [TestMethod]
        public void Contains_should_succeed_when_item_exists_in_collection()
        {
            var expectedString = Any.String();
            var collection = new List<string> { expectedString, Any.String(), Any.String() };

            Assert.That.These(collection).Contains(expectedString);
        }

        [TestMethod]
        public void Contains_should_fail_when_element_not_contained_in_the_collection()
        {
            var missingString = Any.String();
            var collection = new List<string> { Any.String(), Any.String() };

            try
            {
                Assert.That.These(collection).Contains(missingString);
            }
            catch (AssertFailedException e)
            {
                Assert.AreEqual(
                    "Assert.Fail failed. The collection did not contain the expected element",
                    e.Message);
            }
        }
    }
}
