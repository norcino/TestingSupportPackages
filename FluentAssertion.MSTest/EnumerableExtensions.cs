using FluentAssertion.MSTest.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FluentAssertion.MSTest
{
    public static class EnumerableExtensions
    {
        #region Starting Assertion Words
        public static AssertCollection<T> These<T>(this Assert assert, ICollection<T> collection)
        {
            return new AssertCollection<T>(collection);
        }

        public static AssertCollection<T> This<TC, T>(this Assert assert, ICollection<T> collection) where TC : ICollection<T>
        {
            return new AssertCollection<T>(collection);
        }
        #endregion
        public static AssertCollection<T> AreSameAs<T>(this AssertCollection<T> assertCollection, ICollection<T> otherCollection)
        {
            if (assertCollection.Collection == null && otherCollection != null)
                Assert.Fail("The collection was expected to be a null collection but it wasn't");
            
            if (assertCollection.Collection != null && otherCollection == null)
                Assert.Fail("The collection was expected to be Equivalent but the collection was null");
            
            if (assertCollection.Collection.Count != otherCollection.Count)
                Assert.Fail($"The collection was expected to contain {assertCollection.Collection.Count} elements but contained {otherCollection.Count}");

            foreach(var item in assertCollection.Collection)
            {
                //foreach (var comparedItem in otherCollection)
                //{
                //    foreach (var propertyInfo in assertObject.Object.GetType().GetProperties())
                //    {
                //        // Exclude properties
                //        if (exclusions != null && ((IList)exclusions).Contains(propertyInfo.Name)) continue;

                //        // Ignore Objects and Collections
                //        if (propertyInfo.PropertyType.GetTypeInfo().IsValueType || propertyInfo.PropertyType == typeof(string))
                //        {
                //            var objectValue = assertObject.Object.GetType().GetProperty(propertyInfo.Name).GetValue(assertObject.Object, null);
                //            var comparedObjectValue = comparedObject.GetType().GetProperty(propertyInfo.Name).GetValue(comparedObject, null);

                //            if (objectValue is DateTime)
                //            {
                //                TimeSpan difference = (DateTime)objectValue - (DateTime)comparedObjectValue;
                //                Assert.IsTrue(difference < TimeSpan.FromSeconds(1),
                //                    $"Expected Property '{propertyInfo.Name}' of type DateTime to have value [{objectValue}] but was [{comparedObjectValue}]");
                //                continue;
                //            }

                //            Assert.AreEqual(objectValue, comparedObjectValue, $"Expected Property '{propertyInfo.Name}' of type {assertObject.Object.GetType()} to have value [{objectValue}] but was [{comparedObjectValue}]");
                //        }
                //    }

                //    foreach (var fieldInfo in assertObject.Object.GetType().GetFields())
                //    {
                //        // Exclude properties
                //        if (exclusions != null && ((IList)exclusions).Contains(fieldInfo.Name)) continue;

                //        // Ignore Objects and Collections
                //        if (fieldInfo.FieldType.GetTypeInfo().IsValueType || fieldInfo.FieldType == typeof(string))
                //        {
                //            var objectValue = assertObject.Object.GetType().GetProperty(fieldInfo.Name).GetValue(assertObject.Object, null);
                //            var comparedObjectValue = comparedObject.GetType().GetProperty(fieldInfo.Name).GetValue(comparedObject, null);

                //            if (objectValue is DateTime)
                //            {
                //                TimeSpan difference = (DateTime)objectValue - (DateTime)comparedObjectValue;
                //                Assert.IsTrue(difference < TimeSpan.FromSeconds(1),
                //                    $"Expected Field '{fieldInfo.Name}' of type DateTime to have value [{objectValue}] but was [{comparedObjectValue}]");
                //                continue;
                //            }

                //            Assert.AreEqual(objectValue, comparedObjectValue, $"Expected Field '{fieldInfo.Name}' of type {assertObject.Object.GetType()} to have value [{objectValue}] but was [{comparedObjectValue}]");
                //        }
                //    }
                //}
                //if(!otherCollection.Contains(item))
                //{

                //    Assert.Fail($"The collection contains an item but this was not available found in the compared collection:\r\n{JsonSerializer.Serialize(item)}");
                //}
            }

            return assertCollection;
        }

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

        public static string AreSameObjects<T>(T subject, T comparedObject, int comparisonDepth = 0)
        {            
            // if there is a nesting higher then 100, skip further depth and stop the comparison
            if (comparisonDepth > 100) return null;

            foreach (var propertyInfo in typeof(T).GetProperties())
            {
                var objectValue = propertyInfo.GetValue(subject, null);
                var comparedObjectValue = propertyInfo.GetValue(comparedObject, null);
                // Value Types and strings
                if (propertyInfo.PropertyType.GetTypeInfo().IsValueType || propertyInfo.PropertyType == typeof(string))
                {
                    if (objectValue is DateTime)
                    {
                        TimeSpan difference = (DateTime)objectValue - (DateTime)comparedObjectValue;
                        if (!(difference < TimeSpan.FromSeconds(1)))
                            return $"Expected Property '{propertyInfo.Name}' of type DateTime to have value [{objectValue}] but was [{comparedObjectValue}]";
                        continue;
                    }

                    if (objectValue != comparedObjectValue)
                        return $"Expected Property '{propertyInfo.Name}' of type {objectValue.GetType()} to have value [{objectValue}] but was [{comparedObjectValue}]";
                }
                else if (typeof(IEnumerable).IsAssignableFrom(propertyInfo.PropertyType))
                {
                    // The property value is an enumerable, so check each element contained                    
                    // TODO
                }
                else
                {
                    // If the property is a reference object recursively handle hit
                    var result = AreSameObjects(objectValue, comparedObjectValue, comparisonDepth + 1);
                    if (result != null) return result;
                }
            }

            // The compared items are the same
            return null;
        }

        public static AssertCollection<T> Contains<T>(this AssertCollection<T> assertCollection, T element)
        {
            //foreach (var currentItem in assertCollection.Collection)
            //{
            //    foreach (var propertyInfo in typeof(T).GetProperties())
            //    {
            //        // Ignore Objects and Collections
            //        if (propertyInfo.PropertyType.GetTypeInfo().IsValueType || propertyInfo.PropertyType == typeof(string))
            //        {
            //            var objectValue = propertyInfo.GetValue(element, null);
            //            var comparedObjectValue = propertyInfo.GetValue(element, null);

            //            if (objectValue is DateTime)
            //            {
            //                TimeSpan difference = (DateTime)objectValue - (DateTime)comparedObjectValue;
            //                Assert.IsTrue(difference < TimeSpan.FromSeconds(1),
            //                    $"Expected Property '{propertyInfo.Name}' of type DateTime to have value [{objectValue}] but was [{comparedObjectValue}]");
            //                continue;
            //            }

            //            Assert.AreEqual(objectValue, comparedObjectValue, $"Expected Property '{propertyInfo.Name}' of type {assertObject.Object.GetType()} to have value [{objectValue}] but was [{comparedObjectValue}]");
            //        }
            //    }

            //    foreach (var fieldInfo in assertObject.Object.GetType().GetFields())
            //    {
            //        // Exclude properties
            //        if (exclusions != null && ((IList)exclusions).Contains(fieldInfo.Name)) continue;

            //        // Ignore Objects and Collections
            //        if (fieldInfo.FieldType.GetTypeInfo().IsValueType || fieldInfo.FieldType == typeof(string))
            //        {
            //            var objectValue = assertObject.Object.GetType().GetProperty(fieldInfo.Name).GetValue(assertObject.Object, null);
            //            var comparedObjectValue = comparedObject.GetType().GetProperty(fieldInfo.Name).GetValue(comparedObject, null);

            //            if (objectValue is DateTime)
            //            {
            //                TimeSpan difference = (DateTime)objectValue - (DateTime)comparedObjectValue;
            //                Assert.IsTrue(difference < TimeSpan.FromSeconds(1),
            //                    $"Expected Field '{fieldInfo.Name}' of type DateTime to have value [{objectValue}] but was [{comparedObjectValue}]");
            //                continue;
            //            }

            //            Assert.AreEqual(objectValue, comparedObjectValue, $"Expected Field '{fieldInfo.Name}' of type {assertObject.Object.GetType()} to have value [{objectValue}] but was [{comparedObjectValue}]");
            //        }
            //        if (!assertCollection.Collection.Contains(element))
            //        {
            //            Assert.Fail($"The collection did not contain the expected element");
            //        }
            //    }
            //}
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

        public static AssertEachItem<T> HasEmpty<T, P>(this AssertEachItem<T> assertEachItem, Expression<Func<T, P>> property, string message = null)
        {
            var propertyInfo = GetMemberInfoFromExpression(property);

            foreach (var collectionItem in assertEachItem.RootCollection.Collection)
            {
                var enumerable = (property.Compile()(collectionItem) as IEnumerable);
                foreach (var e in enumerable)
                {
                    Assert.Fail(message ?? $"Expected member '{propertyInfo.Name}' to be empty, but it wasn't");
                }
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
