using FluentAssertion.MSTest.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace FluentAssertion.MSTest
{
    public static class AssertObjectInCollectionExtensions
    {
        #region Starting Assertion Words        
        public static AssertObjectInCollection<T> FirstElement<T>(this AssertCollection<T> assertCollection)
        {
            return new AssertObjectInCollection<T>(assertCollection, assertCollection.Collection.FirstOrDefault());
        }

        public static AssertObjectInCollection<T> SecondElement<T>(this AssertCollection<T> assertCollection)
        {
            return new AssertObjectInCollection<T>(assertCollection,
                assertCollection.Collection.ElementAt(1));
        }

        public static AssertObjectInCollection<T> ThirdElement<T>(this AssertCollection<T> assertCollection)
        {
            return new AssertObjectInCollection<T>(assertCollection,
                assertCollection.Collection.ElementAt(2));
        }

        public static AssertObjectInCollection<T> ForthElement<T>(this AssertCollection<T> assertCollection)
        {
            return new AssertObjectInCollection<T>(assertCollection,
                assertCollection.Collection.ElementAt(3));
        }

        public static AssertObjectInCollection<T> FifthElement<T>(this AssertCollection<T> assertCollection)
        {
            return new AssertObjectInCollection<T>(assertCollection,
                assertCollection.Collection.ElementAt(4));
        }
        
        public static AssertObjectInCollection<T> ElementIndex<T>(this AssertCollection<T> assertCollection, int index)
        {
            return new AssertObjectInCollection<T>(assertCollection,
                assertCollection.Collection.ElementAt(index));
        }
        
        public static AssertObjectInCollection<T> ElementAt<T>(this AssertCollection<T> assertCollection, int index)
        {
            return new AssertObjectInCollection<T>(assertCollection,
                assertCollection.Collection.ElementAt(index));
        }

        public static AssertObjectInCollection<T> LastElement<T>(this AssertCollection<T> assertCollection)
        {
            return new AssertObjectInCollection<T>(assertCollection, 
                assertCollection.Collection.ElementAt(assertCollection.Collection.Count - 1));
        }
        #endregion

        #region Chaining Assertion Words
        /// <summary>
        /// Allows to concatenante multiple assertions to the same test subject making the test reading more fluent.
        /// This has no functional value.
        /// </summary>
        /// <typeparam name="T">Type of the object under test</typeparam>
        /// <param name="assertObject">Single object assertion class</param>
        /// <returns>AssertObjectInCollection</returns>
        public static AssertCollection<T> And<T>(this AssertObjectInCollection<T> assertObject)
        {
            return assertObject.AssertCollection;
        }
        #endregion

        #region Assertions
        /// <summary>
        /// Verifies that the test subject has the desired value
        /// </summary>
        /// <typeparam name="T">Type of the object under test</typeparam>
        /// <param name="assertObject">Single object assertion class</param>
        /// <param name="value">Expected value to match</param>
        /// <param name="message">Custom error message</param>
        /// <returns>AssertObjectInCollection</returns>
        public static AssertObjectInCollection<T> HasValue<T>(this AssertObjectInCollection<T> assertObject, object value, string message = null)
        {
            Assert.AreEqual(assertObject.Element, value, message ?? $"Expected value [{value}], but was [{assertObject.Element}]");
            return assertObject;
        }

        /// <summary>
        /// Verifies that the test subject is not null
        /// </summary>
        /// <typeparam name="T">Type of the object under test</typeparam>
        /// <param name="assertObject">Single object assertion class</param>
        /// <param name="message">Custom error message</param>
        /// <returns>AssertObjectInCollection</returns>
        public static AssertObjectInCollection<T> IsNotNull<T>(this AssertObjectInCollection<T> assertObject, string message = null)
        {
            Assert.IsNotNull(assertObject.Element, message ?? "Expected not to be null");
            return assertObject;
        }

        /// <summary>
        /// Verifies that the test subject is true
        /// </summary>
        /// <param name="assertObject">Single object assertion class</param>
        /// <param name="message">Custom error message</param>
        /// <returns>AssertObjectInCollection</returns>
        public static AssertObjectInCollection<bool> IsTrue(this AssertObjectInCollection<bool> assertObject, string message = null)
        {
            Assert.IsTrue(assertObject.Element, message ?? "Expected to true, but was false");
            return assertObject;
        }

        /// <summary>
        /// Verifies that the test subject is false
        /// </summary>
        /// <param name="assertObject">Single object assertion class</param>
        /// <param name="message">Custom error message</param>
        /// <returns>AssertObjectInCollection</returns>
        public static AssertObjectInCollection<bool> IsFalse(this AssertObjectInCollection<bool> assertObject, string message = null)
        {
            Assert.IsTrue(assertObject.Element, message ?? "Expected to false, but was true");
            return assertObject;
        }

        /// <summary>
        /// Verifies that the test subject is greather then the specified value
        /// </summary>
        /// <typeparam name="T">Type of the object under test</typeparam>
        /// <param name="assertObject">Single object assertion class</param>
        /// <param name="comparingvalue"></param>
        /// <param name="message">Custom error message</param>
        /// <returns>AssertObjectInCollection</returns>
        public static AssertObjectInCollection<T> IsGreatherThen<T>(this AssertObjectInCollection<T> assertObject, T comparingvalue, string message = null)
            where T : IComparable<T>
        {
            if (assertObject.Element.CompareTo(comparingvalue) < 0)
                Assert.Fail(message ?? $"Expected value to be greater then [{comparingvalue}], but was [{assertObject.Element}]");

            return assertObject;
        }

        /// <summary>
        /// Verifies that the test subject is greather or equal then the specified value
        /// </summary>
        /// <typeparam name="T">Type of the object under test</typeparam>
        /// <param name="assertObject">Single object assertion class</param>
        /// <param name="comparingvalue"></param>
        /// <param name="message">Custom error message</param>
        /// <returns>AssertObjectInCollection</returns>
        public static AssertObjectInCollection<T> IsGreatherEqualThen<T>(this AssertObjectInCollection<T> assertObject, T comparingvalue, string message = null)
            where T : IComparable<T>
        {
            if (assertObject.Element.CompareTo(comparingvalue) < 0)
                Assert.Fail(message ?? $"Expected value to be greater equal then [{comparingvalue}], but was [{assertObject.Element}]");

            return assertObject;
        }

        /// <summary>
        /// Verifies that the test subject is lesser then the specified value
        /// </summary>
        /// <typeparam name="T">Type of the object under test</typeparam>
        /// <param name="assertObject">Single object assertion class</param>
        /// <param name="comparingvalue"></param>
        /// <param name="message">Custom error message</param>
        /// <returns>AssertObjectInCollection</returns>
        public static AssertObjectInCollection<T> IsLesserThen<T>(this AssertObjectInCollection<T> assertObject, T comparingvalue, string message = null)
            where T : IComparable<T>
        {
            if (assertObject.Element.CompareTo(comparingvalue) > 0)
                Assert.Fail(message ?? $"Expected value to be lesser then [{comparingvalue}], but was [{assertObject.Element}]");

            return assertObject;
        }

        /// <summary>
        /// Verifies that the test subject is lesser or equal then the specified value
        /// </summary>
        /// <typeparam name="T">Type of the object under test</typeparam>
        /// <param name="assertObject">Single object assertion class</param>
        /// <param name="comparingvalue"></param>
        /// <param name="message">Custom error message</param>
        /// <returns>AssertObjectInCollection</returns>
        public static AssertObjectInCollection<T> IsLesserEqualThen<T>(this AssertObjectInCollection<T> assertObject, T comparingvalue, string message = null)
            where T : IComparable<T>
        {
            if (assertObject.Element.CompareTo(comparingvalue) > 0)
                Assert.Fail(message ?? $"Expected value to be lesser or equal then [{comparingvalue}], but was [{assertObject.Element}]");

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
        /// <returns>AssertObjectInCollection</returns>
        public static AssertObjectInCollection<T> IsBetween<T>(this AssertObjectInCollection<T> assertObject, T from, T to, string message = null)
            where T : IComparable<T>
        {
            if (assertObject.Element.CompareTo(from) < 0 || assertObject.Element.CompareTo(to) > 0)
                Assert.Fail(message ?? $"Expected value in the range bewteen [{from}] and [{to}], but was [{assertObject.Element}]");

            return assertObject;
        }

        /// <summary>
        /// Verify that two objects have the same properties, this will ignore the comparison for objects and collections
        /// </summary>
        /// <param name="assertObject">Assert object</param>
        /// <param name="comparedObject">Object to be compared with</param>
        /// <param name="exclusions">String array with the list of properties to not compare</param>
        public static AssertObjectInCollection<T> HasSameProperties<T>(this AssertObjectInCollection<T> assertObject, object comparedObject, params string[] exclusions)
        {
            Assert.IsNotNull(assertObject.Element);
            Assert.IsNotNull(comparedObject);

            foreach (var propertyInfo in assertObject.Element.GetType().GetProperties())
            {
                // Exclude properties
                if (exclusions != null && ((IList)exclusions).Contains(propertyInfo.Name)) continue;

                // Ignore Objects and Collections
                if (propertyInfo.PropertyType.GetTypeInfo().IsValueType || propertyInfo.PropertyType == typeof(string))
                {
                    var objectValue = assertObject.Element.GetType().GetProperty(propertyInfo.Name).GetValue(assertObject.Element, null);
                    var comparedObjectValue = comparedObject.GetType().GetProperty(propertyInfo.Name).GetValue(comparedObject, null);

                    if (objectValue is DateTime)
                    {
                        TimeSpan difference = (DateTime)objectValue - (DateTime)comparedObjectValue;
                        Assert.IsTrue(difference < TimeSpan.FromSeconds(1),
                            $"Expected Property '{propertyInfo.Name}' of type DateTime to have value value [{comparedObjectValue}] but was [{objectValue}]");
                        continue;
                    }

                    Assert.AreEqual(objectValue, comparedObjectValue, $"Expected Property '{propertyInfo.Name}' of type {assertObject.Element.GetType()} to have value [{comparedObjectValue}] but was [{objectValue}]");
                }
            }
            return assertObject;
        }

        public static SamePropertyObject<T> Except<T>(this SamePropertyObject<T> assertObject, Expression<Func<T, object>> excludedProperty)
        {
            assertObject.Exclusions.Add(excludedProperty);
            return assertObject;
        }

        public static SamePropertyObject<T> Except<T>(this AssertObjectInCollection<T> assertObject, Expression<Func<T, object>> excludedProperty)
        {
            var assertSamePropertyObject = new SamePropertyObject<T>(assertObject.Element);
            assertSamePropertyObject.Exclusions.Add(excludedProperty);
            return assertSamePropertyObject;
        }

        //public static AssertObjectInCollection<T> HasSameProperties<T>(this SamePropertyObject<T> assertObject, T comparedObject)
        //{
        //    assertObject.ExpectedObject = comparedObject;
        //    return new AssertObjectInCollection<T>(assertObject., assertObject.ComparedObject);
        //}

        public static AssertObjectInCollection<T> Has<T>(this AssertObjectInCollection<T> assertObject, Func<T, bool> assertions)
        {
            Assert.IsTrue(assertions(assertObject.Element));
            return assertObject;
        }

        public static AssertObjectInCollection<T> HasNonDefault<T, P>(this AssertObjectInCollection<T> assertObject, Expression<Func<T, P>> property, string message = null)
        {
            var propertyInfo = GetMemberInfoFromExpression(property);
            var value = property.Compile()(assertObject.Element);
            if (value.Equals(default(P)))
            {
                Assert.Fail(message ?? $"Expected property '{propertyInfo.Name}' value [{value}] not to be the default value [{default(P)}]");
            }
            return assertObject;
        }

        public static AssertObjectInCollection<T> HasNull<T, P>(this AssertObjectInCollection<T> assertObject, Expression<Func<T, P>> member, string message = null) where P : new()
        {
            var memberInfo = GetMemberInfoFromExpression(member);

            Assert.IsNull(member.Compile()(assertObject.Element), message ?? $"Expected to have null property '{memberInfo.Name}', but it wasn't");
            return assertObject;
        }

        public static AssertObjectInCollection<T> HasNonNull<T, P>(this AssertObjectInCollection<T> assertObject, Expression<Func<T, P>> member, string message = null) where P : new()
        {
            var memberInfo = GetMemberInfoFromExpression(member);

            Assert.IsNotNull(member.Compile()(assertObject.Element), message ?? $"Expected property '{memberInfo.Name}' to not be null but it was");
            return assertObject;
        }

        public static AssertObjectInCollection<T> HasNonEmpty<T, P>(this AssertObjectInCollection<T> assertObject, Expression<Func<T, P>> member, string message = null) where P : IEnumerable
        {
            var memberInfo = GetMemberInfoFromExpression(member);
            var enumerable = (member.Compile()(assertObject.Element) as IEnumerable);

            foreach (var e in enumerable)
            {
                return assertObject;
            }
            Assert.Fail(message ?? $"Expected member '{memberInfo.Name}' to not be empty, but it was");

            return assertObject;
        }

        public static AssertObjectInCollection<T> HasCount<T, P>(this AssertObjectInCollection<T> assertObject, Expression<Func<T, P>> member, int count, string message = null) where P : IEnumerable
        {
            var memberInfo = GetMemberInfoFromExpression(member);
            var enumerable = (member.Compile()(assertObject.Element) as IEnumerable);
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
