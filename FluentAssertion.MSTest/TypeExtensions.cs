using FluentAssertion.MSTest.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace FluentAssertion.MSTest
{
    /// <summary>
    /// Add fluent assertion extensions to MSTest Assert.That for Type
    /// </summary>
    public static class TypeExtensions
    {
        #region Types
        /// <summary>
        /// Verify that the object has the desired type
        /// </summary>
        /// <typeparam name="T">Type of the object</typeparam>
        /// <typeparam name="O">Expected Object type to verify</typeparam>
        /// <param name="assertObject">Assert object</param>
        public static AssertObject<O> IsDerivedFrom<O>(this AssertObject<O> assertObject, Type type, string message = null)
        {
            if (assertObject.Object.GetType().IsAssignableFrom(type))
            {
                return assertObject;
            }
            throw new AssertFailedException(message ?? $"Expected type to be derived of {type.Name} but was {assertObject.GetType()}");
        }

        /// <summary>
        /// Verify that the object has the desired type
        /// </summary>
        /// <typeparam name="T">Type of the object</typeparam>
        /// <typeparam name="O">Expected Object type to verify</typeparam>
        /// <param name="assertObject">Assert object</param>
        public static AssertObject<O> Is<O>(this AssertObject<O> assertObject, Type type, string message = null)
        {
            if (assertObject.Object.GetType() == type)
            {
                return assertObject;
            }

            throw new AssertFailedException(message ?? $"Expected type {type.Name} but was {assertObject.GetType()}");
        }
        #endregion
    }
}