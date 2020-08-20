using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentAssertion.MSTest.Tests
{
    [TestClass]
    public class ActionExtensionsTests
    {
        [TestMethod]
        public void Invoking_should_allow_to_assign_locally_the_the_result_of_invoked_action()
        {
            var sut = new SutClass();
            var index = Any.Int();
            
            Assert.That.Invoking(() => index = sut.Increment(index))
                .DoesNotThrowException()
                .And
                .ExecutesInLessThan(1000);

            Assert.That.This(index).IsGreatherThen(10);
        }

        [TestMethod]
        public void DoesNotThrowException_should_fail_when_exception_is_thrown()
        {
            var sut = new SutClass();
            var i = 0;

            try
            {
                Assert.That.Invoking(() => i = sut.Increment(i, throwException: true))
                    .DoesNotThrowException();
            }catch(AssertFailedException e)
            {
                Assert.AreEqual(
                    "Assert.Fail failed. Expected to execute action without exceptions but 'System.Exception', with error 'Requested to throw Exception' was thrown instead",
                    e.Message);
            }
        }

        [TestMethod]
        public void IsQuickerThan_should_fail_when_action_takes_longer_than_expected()
        {
            var sut = new SutClass();
            var i = 0;
            Assert.That.Invoking(() => i = sut.Increment(i, sleepFor: 100))
                .ExecutesInLessThan(100);
        }
    }
}