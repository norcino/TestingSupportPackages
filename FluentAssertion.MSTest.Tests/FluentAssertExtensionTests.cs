using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentAssertion.MSTest.Tests
{
    [TestClass]
    public class AssertionExtensionTests
    {
        #region HasSameProperties
        [TestMethod]
        public void HasSameProperties_should_ignore_different_fields()
        {
            var p1 = new SutPoco
            {
                BoolField = Any.Bool(),
                IntField = Any.Int(),
                LongField = Any.Long(),
                StringField = Any.String(),
                DoubleField = Any.Double(),
                DateTimeField = Any.DateTime()
            };

            var p2 = new SutPoco
            {
                BoolField = Any.Bool(),
                IntField = Any.Int(),
                LongField = Any.Long(),
                StringField = Any.String(),
                DoubleField = Any.Double(),
                DateTimeField = Any.DateTime()
            };

            Assert.That.This(p1).HasSameProperties(p2);
        }

        [TestMethod]
        public void HasSameProperties_should_compare_properties_accepting_same_values()
        {
            var p1 = new SutPoco
            {
                BoolField = Any.Bool(),
                IntField = Any.Int(),
                LongField = Any.Long(),
                StringField = Any.String(),
                DoubleField = Any.Double(),
                DateTimeField = Any.DateTime()
            };

            var p2 = new SutPoco
            {
                BoolField = p1.BoolField,
                IntField = p1.IntField,
                LongField = p1.LongField,
                StringField = p1.StringField,
                DoubleField = p1.DoubleField,
                DateTimeField = p1.DateTimeField
            };

            Assert.That.Invoking(() => Assert.That.This(p1).HasSameProperties(p2)).DoesNotThrowException();
        }

        [TestMethod]
        public void HasSameProperties_should_compare_properties_intercepting_different_boolean_values()
        {
            var p1 = new SutPoco
            {
                BoolProperty = Any.Bool(),
                IntProperty = Any.Int(),
                LongProperty = Any.Long(),
                StringProperty = Any.String(),
                DoubleProperty = Any.Double(),
                DateTimeProperty = Any.DateTime()
            };

            var p2 = new SutPoco
            {
                BoolProperty = !p1.BoolProperty,
                IntProperty = p1.IntProperty,
                LongProperty = p1.LongProperty,
                StringProperty = p1.StringProperty,
                DoubleProperty = p1.DoubleProperty,
                DateTimeProperty = p1.DateTimeProperty
            };

            Assert.That
                .Invoking(() => 
                    Assert.That.This(p1).HasSameProperties(p2))
                .Throws<AssertFailedException>($"Assert.AreEqual failed. " + 
                    $"Expected:<{p1.BoolProperty}>. Actual:<{p2.BoolProperty}>. "+
                    $"Property 'BoolProperty' of type MSTest.FluentAssertion.Tests.SutPoco has value {p1.BoolProperty} " + 
                    $"but was expected {p2.BoolProperty}");
        }

        [TestMethod]
        public void HasSameProperties_should_compare_properties_intercepting_different_integer_values()
        {
            var p1 = new SutPoco
            {
                BoolProperty = Any.Bool(),
                IntProperty = Any.Int(),
                LongProperty = Any.Long(),
                StringProperty = Any.String(),
                DoubleProperty = Any.Double(),
                DateTimeProperty = Any.DateTime()
            };

            var p2 = new SutPoco
            {
                BoolProperty = p1.BoolProperty,
                IntProperty = p1.IntProperty + 1,
                LongProperty = p1.LongProperty,
                StringProperty = p1.StringProperty,
                DoubleProperty = p1.DoubleProperty,
                DateTimeProperty = p1.DateTimeProperty
            };

            Assert.That
                .Invoking(() =>
                    Assert.That.This(p1).HasSameProperties(p2))
                .Throws<AssertFailedException>($"Assert.AreEqual failed. " +
                    $"Expected:<{p1.IntProperty}>. Actual:<{p2.IntProperty}>. " +
                    $"Property 'IntProperty' of type MSTest.FluentAssertion.Tests.SutPoco has value {p1.IntProperty} " +
                    $"but was expected {p2.IntProperty}");
        }
        #endregion

        [TestMethod]
        public void Usage_sample_verify_properties()
        {
            var p1 = new SutPoco
            {
                BoolProperty = Any.Bool(),
                IntProperty = Any.Int(),
                LongProperty = Any.Long(),
                StringProperty = Any.String()
            };

            var p2 = new SutPoco
            {
                BoolProperty = Any.Bool(),
                IntProperty = Any.Int(),
                LongProperty = Any.Long(),
                StringProperty = Any.String()
            };

            var result = new SutClass().Add(p1, p2);
            
            Assert
                .That
                .This(result)
                .Has(r => r.StringProperty == p1.StringProperty + p2.StringProperty)
                .And()
                .Has(r => r.LongProperty == p1.LongProperty + p2.LongProperty)
                .And()
                .Has(r => r.IntProperty == p1.IntProperty + p2.IntProperty)
                .And()
                .Has(r => r.BoolProperty == (p1.BoolProperty && p2.BoolProperty));
        }
    }
}
