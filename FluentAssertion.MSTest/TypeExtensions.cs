using FluentAssertion.MSTest.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
        public static void IsDerivedFrom<T, O>(this AssertObject<O> assertObject, string message = null)
        {
            if (assertObject.Object is T)
            {
                return;
            }
            throw new AssertFailedException(message ?? $"Expected type to be derived of {typeof(T)} but was {assertObject.GetType()}");
        }

        /// <summary>
        /// Verify that the object has the desired type
        /// </summary>
        /// <typeparam name="T">Type of the object</typeparam>
        /// <typeparam name="O">Expected Object type to verify</typeparam>
        /// <param name="assertObject">Assert object</param>
        public static void Is<T, O>(this AssertObject<O> assertObject, string message = null)
        {
            if (assertObject.Object is T)
            {
                return;
            }
            throw new AssertFailedException(message ?? $"Expected type {typeof(T)} but was {assertObject.GetType()}");
        }
        #endregion
    }
}