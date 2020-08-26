using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace FluentAssertion.MSTest.Tests
{
    [TestClass]
    public class AssertCollectionAssertionsTests
    {
        #region HaveCount
        [TestMethod]
        public void HaveCount_should_fail_when_expected_value_on_empty_Collection()
        {
            var sutClasses = new List<SutClass>();

            Assert.That.Testing(() => Assert.That.These(sutClasses).HaveCount(1))
                .FailsAs("Expected [1] elements, but the collection had [0]");
        }

        [TestMethod]
        public void HaveCount_should_fail_when_expected_value_does_not_match_Collection_count()
        {
            var sutClasses = new List<SutClass> {
                new SutClass {
                    IntProperty = Any.Int(),
                    StringProperty = Any.String()
                }
            };

            Assert.That.Testing(() => Assert.That.These(sutClasses).HaveCount(5))
                .FailsAs("Expected [5] elements, but the collection had [1]");
        }

        [TestMethod]
        public void HaveCount_should_succeed_when_list_is_empty_and_expected_count_is_zero()
        {
            var sutClasses = new List<SutClass>();
            Assert.That.These(sutClasses).HaveCount(0);
        }

        [TestMethod]
        public void HaveCount_should_succeed_when_list_has_one_item_and_expected_count_is_one()
        {
            var sutClasses = new List<SutClass> {
                new SutClass {
                    IntProperty = Any.Int(),
                    StringProperty = Any.String()
                }
            };

            Assert.That.These(sutClasses).HaveCount(1);
        }

        [TestMethod]
        public void HaveCount_should_succeed_when_list_has_number_if_items_same_as_expected_count_value()
        {
            var sutClasses = new List<SutClass> {
                new SutClass(),
                new SutClass(),
                new SutClass(),
                new SutClass(),
                new SutClass(),
                new SutClass(),
                new SutClass(),
                new SutClass()
            };

            Assert.That.These(sutClasses).HaveCount(sutClasses.Count);
        }
        #endregion

        #region IsNotNullOrEmpty
        [TestMethod]
        public void IsNotNullOrEmpty_should_fail_when_Collection_is_empty()
        {
            var sutClasses = new List<SutClass>();

            Assert.That.Testing(() => Assert.That.These(sutClasses).AreNotNullOrEmpty())
                .FailsAs("Collection expected to not be empty, but it was");
        }

        [TestMethod]
        public void IsNotNullOrEmpty_should_fail_when_Collection_is_null()
        {
            List<SutClass> sutClasses = null;

            Assert.That.Testing(() => Assert.That.These(sutClasses).AreNotNullOrEmpty())
                .FailsAs("Collection expected to not be null, but it was");
        }
        #endregion

        [TestMethod]
        public void AssertCollection_should_support_and_concatenation_with_multiple_assertions()
        {
            var sutClasses = new List<SutClass>
            {
                new SutClass
                {
                    IntProperty = Any.Int(),
                    StringProperty = Any.String()
                },
                new SutClass
                {
                    IntProperty = Any.Int(),
                    StringProperty = Any.String()
                }
            };

            Assert.That.These(sutClasses)
                .AreNotNullOrEmpty()
                .And()
                .HaveCount(sutClasses.Count);
        }

        //[TestMethod]
        //public void AssertCollection_should_support_and_concatenation_with_multiple_assertions()
        //{
        //    var sutClasses = new List<SutClass>
        //    {
        //        new SutClass
        //        {
        //            IntProperty = Any.Int(),
        //            StringProperty = Any.String()
        //        },
        //        new SutClass
        //        {
        //            IntProperty = Any.Int(),
        //            StringProperty = Any.String()
        //        }                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               
        //    };

        //    Assert.That.These(sutClasses)
        //        .HaveCount(sutClasses.Count)
        //        .And()
        //        .FirstElement().IsNotNull()
        //            .AndThisElement()
        //            .Has(c => c.StringProperty == sutClasses.First().StringProperty)
        //        .And()
        //        .SecondElement().HasSameProperties(sutClasses[1]);
        //}
    }
}
