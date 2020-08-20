using FluentAssertion.MSTest.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace FluentAssertion.MSTest
{
    public static class EnumerableExtensions
    {
        #region Starting Assertion Words
        public static AssertCollection<T> All<T>(this Assert assert, ICollection<T> collection)
        {
            return new AssertCollection<T>(collection);
        }

        public static AssertCollection<T> These<T>(this Assert assert, ICollection<T> collection)
        {
            return new AssertCollection<T>(collection);
        }
        #endregion

        #region Chaining Assertion Words
        public static AssertCollection<T> And<T>(this AssertCollection<T> assertCollection)
        {
            return assertCollection;
        }

        public static AssertCollection<T> AndEachElement<T>(this AssertCollection<T> assertCollection)
        {
            return assertCollection;
        }        
        #endregion

        public static AssertCollection<T> IsNotNull<T>(this AssertCollection<T> assertCollection)
        {
            Assert.IsNotNull(assertCollection.Collection);
            return assertCollection;
        }

        public static AssertCollection<T> IsNotNullOrEmpty<T>(this AssertCollection<T> assertCollection)
        {
            Assert.IsNotNull(assertCollection.Collection, "The collection is null");
            Assert.IsTrue(assertCollection.Collection.Any(), "The collection is empty");
            return assertCollection;
        }

        /// <summary>
        /// Make sure that each element of a collection matches the specified criteria in the function
        /// </summary>
        /// <typeparam name="T">Generic type for the collection</typeparam>
        /// <param name="assertCollection"></param>
        /// <param name="assertions">Function which must return true to succeed validation</param>
        /// <returns></returns>
        public static AssertCollection<T> Are<T>(this AssertCollection<T> assertCollection, Func<T, bool> assertions)
        {
            Assert.IsTrue(assertCollection.Collection.All(assertions));
            return assertCollection;
        }

        /// <summary>
        /// Make sure that at least one element of a collection matches the specified criteria in the function
        /// </summary>
        /// <typeparam name="T">Generic type for the collection</typeparam>
        /// <param name="assertCollection"></param>
        /// <param name="assertions">Function which must return true to succeed validation</param>
        /// <returns></returns>
        public static AssertCollection<T> Contains<T>(this AssertCollection<T> assertCollection, Func<T, bool> assertions)
        {
            Assert.IsTrue(assertCollection.Collection.Any(assertions));
            return assertCollection;
        }

        public static AssertCollection<T> Have<T>(this AssertCollection<T> assertCollection, Func<T, bool> assertions)
        {
            return Are(assertCollection, assertions);
        }

        public static AssertCollection<T> HaveCount<T>(this AssertCollection<T> assertCollection, int expectedCount, string message = null)
        {
            if (expectedCount != assertCollection.Collection.Count)
                throw new AssertFailedException(message ?? $"Expected [{expectedCount}] elements, but the collection had [{assertCollection.Collection.Count}]");
            return assertCollection;
        }
               
        public static AssertCollection<T> HasNonNull<T, P>(this AssertCollection<T> assertCollection, Expression<Func<T, P>> member, string message = null) where P : new()
        {
            var memberInfo = GetMemberInfoFromExpression(member);
            int iterationIndex = 1;

            foreach (var collectionItem in assertCollection.Collection)
            {
                Assert.IsNotNull(member.Compile()(collectionItem), message ?? $"Expected property '{memberInfo.Name}' to not be null but for the item [{iterationIndex++}] of the collection was null");
            }

            return assertCollection;
        }

        public static AssertCollection<T> HasNull<T, P>(this AssertCollection<T> assertCollection, Expression<Func<T, P>> member, string message = null) where P : new()
        {
            var memberInfo = GetMemberInfoFromExpression(member);
            int iterationIndex = 1;

            foreach (var collectionItem in assertCollection.Collection)
            {
                var value = member.Compile()(collectionItem);
                Assert.IsNull(value, message ?? $"Expected property '{memberInfo.Name}' to be null but for the item [{iterationIndex++}] of the collection it was [{value}]");
            }

            return assertCollection;
        }

        public static AssertCollection<T> HasNonDefault<T, P>(this AssertCollection<T> assertCollection, Expression<Func<T, P>> member, string message = null)
        {
            var memberInfo = GetMemberInfoFromExpression(member);
            int iterationIndex = 1;

            foreach (var collectionItem in assertCollection.Collection)
            {
                var value = member.Compile()(collectionItem);

                if (value.Equals(default(P)))
                {
                    Assert.Fail(message ?? $"Expected property '{memberInfo.Name}' value ({value}) not to be the default value ({default(P)}), but for the item [{iterationIndex++}] of the collection it was");
                }
            }           
            
            return assertCollection;
        }

        #region Helpers
        private static MemberInfo GetMemberInfoFromExpression<T, P>(Expression<Func<T, P>> member)
        {
            var memberInfo = ((MemberExpression)member.Body)?.Member as MemberInfo;
            if (memberInfo == null)
            {
                throw new ArgumentException("The lambda expression 'member' should point to a valid Property or Field");
            }
            return memberInfo;
        }
        #endregion
    }
}
