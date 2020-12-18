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

        public static AssertEachItem<T> AndEachElement<T>(this AssertCollection<T> assertCollection)
        {
            return new AssertEachItem<T>(assertCollection);
        }

        public static AssertEachItem<T> And<T, TP>(this AssertEachItemProperty<T, TP> assertProperty)
        {
            return assertProperty.AssertEachItemObject;
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
            Assert.IsTrue(assertCollection.Collection.Any(assertions), $"Expected to contain at least one item matching the assertion");
            return assertCollection;
        }

        public static AssertCollection<T> DoesNotContain<T>(this AssertCollection<T> assertCollection, Func<T, bool> assertions)
        {
            Assert.IsFalse(assertCollection.Collection.Any(assertions), $"Expected to not contain any item matching the assertion");
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

        #region Each Item Assertions
        public static AssertEachItem<T> And<T>(this AssertEachItem<T> assertEachItem)
        {
            return assertEachItem;
        }
        
        public static AssertEachItem<T> IsOfType<T>(this AssertEachItem<T> assertEachItem, Type type, string message = null)
        {
            int iterationIndex = 1;

            foreach (var collectionItem in assertEachItem.RootCollection.Collection)
            {
                if (collectionItem.GetType() != typeof(T))
                {
                    Assert.Fail(message ?? $"Collection expected to have all items of type '{type.Name}' but the item [{iterationIndex++}] was '{collectionItem.GetType().Name}'");
                    return assertEachItem;
                }
            }

            return assertEachItem;
        }

        public static AssertEachItemProperty<T, TP> HasProperty<T, TP>(this AssertEachItem<T> assertEachItem, Expression<Func<T, TP>> p)
        {
            return new AssertEachItemProperty<T, TP>(assertEachItem, p);
        }

        public static AssertEachItem<T> HasNull<T, P>(this AssertEachItem<T> assertEachItem, Expression<Func<T, P>> property, string message = null)
        {
            var propertyInfo = GetMemberInfoFromExpression(property);

            foreach (var collectionItem in assertEachItem.RootCollection.Collection)
            {
                var value = property.Compile()(collectionItem);
                Assert.IsNull(value, message ?? $"Expected property '{propertyInfo.Name}' value null but it was '{value}'");
            }
            return assertEachItem;
        }

        public static AssertEachItem<T> HasNonNull<T, P>(this AssertEachItem<T> assertEachItem, Expression<Func<T, P>> property, string message = null)
        {
            var propertyInfo = GetMemberInfoFromExpression(property);

            foreach (var collectionItem in assertEachItem.RootCollection.Collection)
            {
                var value = property.Compile()(collectionItem);
                Assert.IsNotNull(value, message ?? $"Expected property '{propertyInfo.Name}' value to be not null but it was");
            }
            return assertEachItem;
        }
        
        public static AssertEachItem<T> HasNonDefault<T, P>(this AssertEachItem<T> assertEachItem, Expression<Func<T, P>> property, string message = null)
        {
            foreach (var collectionItem in assertEachItem.RootCollection.Collection)
            {
                var propertyInfo = GetMemberInfoFromExpression(property);
                var value = property.Compile()(collectionItem);
                if (value.Equals(default(P)))
                {
                    Assert.Fail(message ?? $"Expected property '{propertyInfo.Name}' value [{value}] not to be the default value [{default(P)}]");
                }
            }
            return assertEachItem;
        }

        public static AssertEachItem<T> HasDefault<T, P>(this AssertEachItem<T> assertEachItem, Expression<Func<T, P>> property, string message = null)
        {
            foreach (var collectionItem in assertEachItem.RootCollection.Collection)
            {
                var propertyInfo = GetMemberInfoFromExpression(property);
                var value = property.Compile()(collectionItem);
                if (!value.Equals(default(P)))
                {
                    Assert.Fail(message ?? $"Expected property '{propertyInfo.Name}' value [{value}] to be the default value [{default(P)}]");
                }
            }
            return assertEachItem;
        }        
        #endregion

        #region Each Item Property Assertions
        public static AssertEachItemProperty<T, TP> WithValue<T, TP>(this AssertEachItemProperty<T, TP> assertProperty, TP value)
        {
            int iterationIndex = 1;

            foreach (var collectionItem in assertProperty.AssertEachItemObject.RootCollection.Collection)
            {
                var actualValue = assertProperty.PropertyExpression.Compile()(collectionItem);
                Assert.AreEqual(value, actualValue, $"The value [{actualValue}] of the property '{GetMemberName(assertProperty.PropertyExpression.Body)}' was expected to be equal to [{value}] but for item [{iterationIndex++}] wasn't");
            }
            return assertProperty;
        }

        public static AssertEachItemProperty<T, string> WhichValueStartsWith<T>(this AssertEachItemProperty<T, string> assertProperty, string value)
        {
            int iterationIndex = 1;

            foreach (var collectionItem in assertProperty.AssertEachItemObject.RootCollection.Collection)
            {
                var actualValue = assertProperty.PropertyExpression.Compile()(collectionItem);
                Assert.IsTrue(actualValue.StartsWith(value), $"The value [{actualValue}] of the property '{GetMemberName(assertProperty.PropertyExpression.Body)}' was expected to start with [{value}] but for item [{iterationIndex++}] wasn't");
            }
            return assertProperty;
        }
        #endregion

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

        /// <summary>
        /// Get the Member name from the expression passed in
        /// </summary>
        /// <param name="expression"></param>
        /// <returns>Member Name</returns>
        private static string GetMemberName(Expression expression)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.MemberAccess:
                    return ((MemberExpression)expression).Member.Name;
                case ExpressionType.Convert:
                    return GetMemberName(((UnaryExpression)expression).Operand);
                default:
                    throw new NotSupportedException(expression.NodeType.ToString());
            }
        }
        #endregion
    }
}
