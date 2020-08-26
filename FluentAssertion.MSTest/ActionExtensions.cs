using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using FluentAssertion.MSTest.Framework;

namespace FluentAssertion.MSTest
{
    /// <summary>
    /// Add fluent assertion extensions to MSTest Assert.That of Action
    /// </summary>
    public static class ActionExtensions
    {
        #region Entry point keywords
        public static AssertAction Invoking(this Assert assert, Action action)
        {
            return new AssertAction(action);
        }
        #endregion

        #region Assertions
        public static AssertAction ExecutesInLessThan(this AssertAction assertAction, long milliseconds, string message = null)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            assertAction.Invoke();
            stopwatch.Stop();

            var elapsed = stopwatch.ElapsedMilliseconds;
            if (elapsed > milliseconds)
                Assert.Fail(message ?? $"Expected to exceute in less than {milliseconds}ms but took {elapsed}ms");

            return assertAction;
        }

        public static AssertAction DoesNotThrowException(this AssertAction assertAction, string message = null)
        {
            try
            {
                assertAction.Invoke();
            }
            catch (Exception e)
            {
                Assert.Fail(message ?? $"Expected to execute action without exceptions but '{e.GetType()}', with error '{e.Message}' was thrown instead");
            }
            return assertAction;
        }

        public static AssertAction Throws(this AssertAction assertAction, string message = null)
        {
            try
            {
                assertAction.Invoke();
            }
            catch (Exception)
            {
                return assertAction;
            }

            Assert.Fail(message ?? "Action was expected to throw an exception");

            return assertAction;
        }

        public static AssertAction Throws<T>(this AssertAction assertAction, string expectedExceptionMessage = null, string message = null) where T : Exception
        {
            try
            {
                assertAction.Invoke();
            }
            catch (T e)
            {
                if (expectedExceptionMessage == null) return assertAction;

                if (e.Message == expectedExceptionMessage)
                    return assertAction;
                else
                    Assert.Fail(message ?? $"Expected error message to be: [{expectedExceptionMessage}]\r\nBut was: [{e.Message}]");
            }

            Assert.Fail(message ?? "The Action was expected to throw an exception");
            return assertAction;
        }
        #endregion
    }
}