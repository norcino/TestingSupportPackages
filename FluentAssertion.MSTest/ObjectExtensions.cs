﻿using FluentAssertion.MSTest.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Linq.Expressions;
using System.Reflection;

namespace FluentAssertion.MSTest
{
    public static class ObjectExtensions
    {
        #region Starting Assertion Words
        /// <summary>
        /// Starting word to begin the assertions on a single object
        /// </summary>
        /// <typeparam name="T">Type of the object under test</typeparam>
        /// <param name="assert">Extended MS Assert class</param>
        /// <param name="subject">Subject under test</param>
        /// <returns>AssertObject</returns>
        public static AssertObject<T> This<T>(this Assert assert, T subject)
        {
            return new AssertObject<T>(subject);
        }

        public static AssertObject<T> The<T>(this Assert assert, T subject)
        {
            return new AssertObject<T>(subject);
        }
        #endregion

        #region Chaining Assertion Words
        /// <summary>
        /// Allows to concatenante multiple assertions to the same test subject making the test reading more fluent.
        /// This has no functional value.
        /// </summary>
        /// <typeparam name="T">Type of the object under test</typeparam>
        /// <param name="assertObject">Single object assertion class</param>
        /// <returns>AssertObject</returns>
        public static AssertObject<T> And<T>(this AssertObject<T> assertObject)
        {
            return assertObject;
        }
        #endregion

        public static AssertObject<TN> As<TN>(this AssertObject<object> assertObject) where TN : class
        {
            return new AssertObject<TN>(assertObject.Object as TN);
        }

        #region Assertions
        /// <summary>
        /// Verifies that the test subject has the desired value
        /// </summary>
        /// <typeparam name="T">Type of the object under test</typeparam>
        /// <param name="assertObject">Single object assertion class</param>
        /// <param name="value">Expected value to match</param>
        /// <param name="message">Custom error message</param>
        /// <returns>AssertObject</returns>
        public static AssertObject<T> HasValue<T>(this AssertObject<T> assertObject, object value, string message = null)
        {
            Assert.AreEqual(assertObject.Object, value, message ?? $"Expected value [{value}], but was [{assertObject.Object}]");
            return assertObject;
        }

        /// <summary>
        /// Verifies that the test subject is not null
        /// </summary>
        /// <typeparam name="T">Type of the object under test</typeparam>
        /// <param name="assertObject">Single object assertion class</param>
        /// <param name="message">Custom error message</param>
        /// <returns>AssertObject</returns>
        public static AssertObject<T> IsNotNull<T>(this AssertObject<T> assertObject, string message = null)
        {
            Assert.IsNotNull(assertObject.Object, message ?? "Expected not to be null");
            return assertObject;
        }

        public static AssertObject<T> IsNull<T>(this AssertObject<T> assertObject, string message = null)
        {
            Assert.IsNull(assertObject.Object, message ?? "Expected to be null");
            return assertObject;
        }

        /// <summary>
        /// Verifies that the test subject is true
        /// </summary>
        /// <param name="assertObject">Single object assertion class</param>
        /// <param name="message">Custom error message</param>
        /// <returns>AssertObject</returns>
        public static AssertObject<bool> IsTrue(this AssertObject<bool> assertObject, string message = null)
        {
            Assert.IsTrue(assertObject.Object, message ?? "Expected to true, but was false");
            return assertObject;
        }

        /// <summary>
        /// Verifies that the test subject is false
        /// </summary>
        /// <param name="assertObject">Single object assertion class</param>
        /// <param name="message">Custom error message</param>
        /// <returns>AssertObject</returns>
        public static AssertObject<bool> IsFalse(this AssertObject<bool> assertObject, string message = null)
        {
            Assert.IsFalse(assertObject.Object, message ?? "Expected to false, but was true");
            return assertObject;
        }

        /// <summary>
        /// Verifies that the test subject is greather then the specified value
        /// </summary>
        /// <typeparam name="T">Type of the object under test</typeparam>
        /// <param name="assertObject">Single object assertion class</param>
        /// <param name="comparingvalue"></param>
        /// <param name="message">Custom error message</param>
        /// <returns>AssertObject</returns>
        public static AssertObject<T> IsGreatherThen<T>(this AssertObject<T> assertObject, T comparingvalue, string message = null)
            where T : IComparable<T>
        {
            if (assertObject.Object.CompareTo(comparingvalue) < 0)
                Assert.Fail(message ?? $"Expected value to be greater then [{comparingvalue}], but was [{assertObject.Object}]");

            return assertObject;
        }

        /// <summary>
        /// Verifies that the test subject is greather or equal then the specified value
        /// </summary>
        /// <typeparam name="T">Type of the object under test</typeparam>
        /// <param name="assertObject">Single object assertion class</param>
        /// <param name="comparingvalue"></param>
        /// <param name="message">Custom error message</param>
        /// <returns>AssertObject</returns>
        public static AssertObject<T> IsGreatherEqualThen<T>(this AssertObject<T> assertObject, T comparingvalue, string message = null)
            where T : IComparable<T>
        {
            if (assertObject.Object.CompareTo(comparingvalue) < 0)
                Assert.Fail(message ?? $"Expected value to be greater equal then [{comparingvalue}], but was [{assertObject.Object}]");

            return assertObject;
        }

        /// <summary>
        /// Verifies that the test subject is lesser then the specified value
        /// </summary>
        /// <typeparam name="T">Type of the object under test</typeparam>
        /// <param name="assertObject">Single object assertion class</param>
        /// <param name="comparingvalue"></param>
        /// <param name="message">Custom error message</param>
        /// <returns>AssertObject</returns>
        public static AssertObject<T> IsLesserThen<T>(this AssertObject<T> assertObject, T comparingvalue, string message = null)
            where T : IComparable<T>
        {
            if (assertObject.Object.CompareTo(comparingvalue) > 0)
                Assert.Fail(message ?? $"Expected value to be lesser then [{comparingvalue}], but was [{assertObject.Object}]");

            return assertObject;
        }

        /// <summary>
        /// Verifies that the test subject is lesser or equal then the specified value
        /// </summary>
        /// <typeparam name="T">Type of the object under test</typeparam>
        /// <param name="assertObject">Single object assertion class</param>
        /// <param name="comparingvalue"></param>
        /// <param name="message">Custom error message</param>
        /// <returns>AssertObject</returns>
        public static AssertObject<T> IsLesserEqualThen<T>(this AssertObject<T> assertObject, T comparingvalue, string message = null)
            where T : IComparable<T>
        {
            if (assertObject.Object.CompareTo(comparingvalue) > 0)
                Assert.Fail(message ?? $"Expected value to be lesser or equal then [{comparingvalue}], but was [{assertObject.Object}]");

            return assertObject;
        }

        /// <summary>
        /// Verifies if the value is beween the two values of the parameters including begin and end
        /// </summary>
        /// <typeparam name="T">Type of the object under test</typeparam>
        /// <param name="assertObject">Single object assertion class</param>
        /// <param name="from">Beginning of the range</param>
        /// <param name="to">End of the range</param>
        /// <param name="message">Custom error message</param>
        /// <returns>AssertObject</returns>
        public static AssertObject<T> IsBetween<T>(this AssertObject<T> assertObject, T from, T to, string message = null)
            where T : IComparable<T>
        {
            if (assertObject.Object.CompareTo(from) < 0 || assertObject.Object.CompareTo(to) > 0)
                Assert.Fail(message ?? $"Expected value in the range bewteen [{from}] and [{to}], but was [{assertObject.Object}]");

            return assertObject;
        }

        /// <summary>
        /// Verify that two objects have the same properties, this will ignore the comparison for objects and collections
        /// </summary>
        /// <param name="assertObject">Assert object</param>
        /// <param name="comparedObject">Object to be compared with</param>
        /// <param name="exclusions">String array with the list of properties to not compare</param>
        public static AssertObject<T> HasSameProperties<T>(this AssertObject<T> assertObject, object comparedObject, params string[] exclusions)
        {
            Assert.IsNotNull(assertObject.Object);
            Assert.IsNotNull(comparedObject);

            foreach (var propertyInfo in assertObject.Object.GetType().GetProperties())
            {
                // Exclude properties
                if (exclusions != null && ((IList)exclusions).Contains(propertyInfo.Name)) continue;

                // Ignore Objects and Collections
                if (propertyInfo.PropertyType.GetTypeInfo().IsValueType || propertyInfo.PropertyType == typeof(string))
                {
                    var objectValue = assertObject.Object.GetType().GetProperty(propertyInfo.Name).GetValue(assertObject.Object, null);
                    var comparedObjectValue = comparedObject.GetType().GetProperty(propertyInfo.Name).GetValue(comparedObject, null);

                    if (objectValue is DateTime)
                    {
                        TimeSpan difference = (DateTime)objectValue - (DateTime)comparedObjectValue;
                        Assert.IsTrue(difference < TimeSpan.FromSeconds(1),
                            $"Expected Property '{propertyInfo.Name}' of type DateTime to have value [{objectValue}] but was [{comparedObjectValue}]");
                        continue;
                    }

                    Assert.AreEqual(objectValue, comparedObjectValue, $"Expected Property '{propertyInfo.Name}' of type {assertObject.Object.GetType()} to have value [{objectValue}] but was [{comparedObjectValue}]");
                }
            }

            foreach (var fieldInfo in assertObject.Object.GetType().GetFields())
            {
                // Exclude properties
                if (exclusions != null && ((IList)exclusions).Contains(fieldInfo.Name)) continue;

                // Ignore Objects and Collections
                if (fieldInfo.FieldType.GetTypeInfo().IsValueType || fieldInfo.FieldType == typeof(string))
                {
                    var objectValue = assertObject.Object.GetType().GetProperty(fieldInfo.Name).GetValue(assertObject.Object, null);
                    var comparedObjectValue = comparedObject.GetType().GetProperty(fieldInfo.Name).GetValue(comparedObject, null);

                    if (objectValue is DateTime)
                    {
                        TimeSpan difference = (DateTime)objectValue - (DateTime)comparedObjectValue;
                        Assert.IsTrue(difference < TimeSpan.FromSeconds(1),
                            $"Expected Field '{fieldInfo.Name}' of type DateTime to have value [{objectValue}] but was [{comparedObjectValue}]");
                        continue;
                    }

                    Assert.AreEqual(objectValue, comparedObjectValue, $"Expected Field '{fieldInfo.Name}' of type {assertObject.Object.GetType()} to have value [{objectValue}] but was [{comparedObjectValue}]");
                }
            }
            return assertObject;
        }

        public static AssertObject<T> Has<T>(this AssertObject<T> assertObject, Func<T, bool> assertions)
        {
            Assert.IsTrue(assertions(assertObject.Object));
            return assertObject;
        }

        public static AssertPorperty<T,TP> HasProperty<T,TP>(this AssertObject<T> assertObject, Expression<Func<T,TP>> p)
        {
            return new AssertPorperty<T,TP>(assertObject.Object, p.Compile()(assertObject.Object));
        }

        public static AssertPorperty<T, TP> WithValue<T, TP>(this AssertPorperty<T, TP> assertProperty, TP value)
        {
            Assert.AreEqual(value, assertProperty.Property, $"Value '{value}' expected to be equal to '{assertProperty.Property}' but wasn't");
            return assertProperty;
        }
        public static AssertPorperty<T, TP> Null<T, TP>(this AssertPorperty<T, TP> assertProperty)
        {
            Assert.IsNull(assertProperty.Property, $"Value expected to be null but was '{assertProperty.Property}'");
            return assertProperty;
        }

        public static AssertPorperty<T, TP> ButAlso<T, TP>(this AssertPorperty<T, TP> assertProperty)
        {
            return assertProperty;
        }

        public static AssertPorperty<T, TP> IsNotNull<T, TP>(this AssertPorperty<T, TP> assertProperty)
        {
            Assert.IsNotNull(assertProperty.Property, $"Property was expected not to be null");
            return assertProperty;
        }

        public static AssertObject<T> And<T,TP>(this AssertPorperty<T,TP> assertProperty)
        {
            return new AssertObject<T>(assertProperty.ParentObject);
        }

        public static AssertObject<T> HasNonDefault<T, P>(this AssertObject<T> assertObject, Expression<Func<T, P>> property, string message = null)
        {
            var propertyInfo = GetMemberInfoFromExpression(property);
            var value = property.Compile()(assertObject.Object);
            if (value.Equals(default(P)))
            {
                Assert.Fail(message ?? $"Expected property '{propertyInfo.Name}' value [{value}] not to be the default value [{default(P)}]");
            }
            return assertObject;
        }

        public static AssertObject<T> HasDefault<T, P>(this AssertObject<T> assertObject, Expression<Func<T, P>> property, string message = null)
        {
            var propertyInfo = GetMemberInfoFromExpression(property);
            var value = property.Compile()(assertObject.Object);

            if(default(P) == null)
            {
                if (value != null)
                    Assert.Fail(message ?? $"Expected property '{propertyInfo.Name}' value [{value}] to have the default value [{default(P)}]");

                return assertObject;
            }
            
            if (!value.Equals(default(P)))
            {
                Assert.Fail(message ?? $"Expected property '{propertyInfo.Name}' value [{value}] to have the default value [{default(P)}]");
            }
            return assertObject;
        }

        public static AssertObject<T> HasDefaultValue<T>(this AssertObject<T> assertObject, string message = null)
        {            
            if (!assertObject.Object.Equals(default(T)))
            {                
                    Assert.Fail(message ?? $"Expected to have default value [{default(T)}] but found [{assertObject.Object}]");

                return assertObject;
            }
            return assertObject;
        }

        public static AssertObject<T> HasNonDefaultValue<T>(this AssertObject<T> assertObject, string message = null)
        {
            if (assertObject.Object.Equals(default(T)))
            {
                Assert.Fail(message ?? $"Expected not to have default value but found [{default(T)}]");

                return assertObject;
            }
            return assertObject;
        }

        public static AssertObject<T> HasNull<T, P>(this AssertObject<T> assertObject, Expression<Func<T, P>> member, string message = null)
        {
            var memberInfo = GetMemberInfoFromExpression(member);

            Assert.IsNull(member.Compile()(assertObject.Object), message ?? $"Expected to have null property '{memberInfo.Name}', but it wasn't");
            return assertObject;
        }

        public static AssertObject<T> HasNonNull<T, P>(this AssertObject<T> assertObject, Expression<Func<T, P>> member, string message = null)
        {
            var memberInfo = GetMemberInfoFromExpression(member);

            Assert.IsNotNull(member.Compile()(assertObject.Object), message ?? $"Expected property '{memberInfo.Name}' to not be null but it was");
            return assertObject;
        }

        public static AssertObject<T> HasNonEmpty<T, P>(this AssertObject<T> assertObject, Expression<Func<T, P>> member, string message = null) where P : IEnumerable
        {
            var memberInfo = GetMemberInfoFromExpression(member);
            var enumerable = (member.Compile()(assertObject.Object) as IEnumerable);

            foreach (var e in enumerable)
            {
                return assertObject;
            }

            Assert.Fail(message ?? $"Expected member '{memberInfo.Name}' to not be empty, but it was");
            return assertObject;
        }

        public static AssertObject<T> HasEmpty<T, P>(this AssertObject<T> assertObject, Expression<Func<T, P>> member, string message = null) where P : IEnumerable
        {
            var memberInfo = GetMemberInfoFromExpression(member);
            var enumerable = (member.Compile()(assertObject.Object) as IEnumerable);

            foreach (var e in enumerable)
            {
                Assert.Fail(message ?? $"Expected member '{memberInfo.Name}' to be empty, but it wasn't");
            }
            return assertObject;
        }

        public static AssertObject<T> HasCount<T, P>(this AssertObject<T> assertObject, Expression<Func<T, P>> member, int count, string message = null) where P : IEnumerable
        {
            var memberInfo = GetMemberInfoFromExpression(member);
            var enumerable = (member.Compile()(assertObject.Object) as IEnumerable);
            var resultCount = 0;
            foreach (var e in enumerable)
            {
                resultCount++;
            }

            if (resultCount != count)
                Assert.Fail(message ?? $"Expected member '{memberInfo.Name}' to have count [{count}] but had [{resultCount}]");

            return assertObject;
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
        #endregion
    }
}
