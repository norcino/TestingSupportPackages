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
        public void DoesNotThrowException_should_not_fail_when_exception_is_thrown()
        {
            var sut = new SutClass();
            var @int = Any.Int();

            Assert.That.Invoking(() => @int = sut.Increment(@int, throwException: false)).DoesNotThrowException();
        }

        [TestMethod]
        public void IsQuickerThan_should_fail_when_action_takes_longer_than_expected_milliseconds()
        {
            var sut = new SutClass();
            var i = 0;
            try
            {
                Assert.That.Invoking(() => i = sut.Increment(i, sleepFor: 100)).ExecutesInLessThan(100);
            }
            catch (AssertFailedException e)
            {
                Assert.IsTrue(e.Message.Contains("Assert.Fail failed. Expected to exceute in less than 100ms but took "));
            }
        }

        [TestMethod]
        public void IsQuickerThan_should_not_fail_when_action_takes_less_than_expected_milliseconds()
        {
            var sut = new SutClass();
            var i = 0;
            
            Assert.That.Invoking(() => i = sut.Increment(i, sleepFor: 0)).ExecutesInLessThan(100);
        }
    }
}