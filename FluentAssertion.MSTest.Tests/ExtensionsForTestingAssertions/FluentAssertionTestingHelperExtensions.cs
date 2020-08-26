using FluentAssertion.MSTest.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;

namespace FluentAssertion.MSTest.Tests
{
    public static class FluentAssertionTestingHelperExtensions
    {
        public static AssertAction Testing(this Assert assert, Action action)
        {
            return new AssertAction(action);
        }

        public static void FailsAs(this AssertAction assertAction, string expectedMessage)
        {
            try
            {
                assertAction.Invoke();
            }
            catch (TargetInvocationException invocationException)
            {
                Assert.That.This(invocationException)
                    .HasProperty(e => e.InnerException)
                        .WithType(typeof(AssertFailedException))
                    .And()
                    .HasProperty(e => e.InnerException.Message)
                    .HasValue(expectedMessage);
                return;
            } catch(AssertFailedException assertException)
            {
                Assert.That.This(assertException)                    
                    .HasProperty(e => e.Message)
                    .WithValue(expectedMessage);
                return;
            }

            Assert.Fail("Test execution did not fail as expected");            
        }
    }
}