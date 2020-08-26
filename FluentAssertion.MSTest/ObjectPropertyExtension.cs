using FluentAssertion.MSTest.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace FluentAssertion.MSTest
{
    public static class ObjectPropertyExtension
    {
        #region Starting Assertion Words
        public static AssertObjectProperty<T,P> HasProperty<T,P>(this AssertObject<T> assertObject, Expression<Func<T,P>> property)
        {
            var propertyInfo = GetMemberInfoFromExpression(property);
            var value = property.Compile()(assertObject.Object);
            return new AssertObjectProperty<T, P>(assertObject, value);
        }
        #endregion

        #region Chaining Assertion Words
        public static AssertObjectProperty<T,P> AndIt<T,P>(this AssertObjectProperty<T,P> assertObjectProperty)
        {
            return assertObjectProperty;
        }
        #endregion

        #region Back to Object context Words
        public static AssertObject<T> And<T, P>(this AssertObjectProperty<T, P> assertObjectProperty)
        {
            return assertObjectProperty.AssertObject;
        }
        #endregion

        public static AssertObjectProperty<T,P> HasType<T,P>(this AssertObjectProperty<T,P> assertObjectProperty, Type type, string message = null)
        {
            if (assertObjectProperty.Property.GetType() == type )
            {
                return assertObjectProperty;
            }
            throw new AssertFailedException(message ?? $"Expected type to be derived of [{type.Name}] but was [{typeof(P).Name}]");
        }

        public static AssertObjectProperty<T, P> WithType<T, P>(this AssertObjectProperty<T, P> assertObjectProperty, Type type, string message = null)
        {
            if (assertObjectProperty.Property.GetType() == type)
            {
                return assertObjectProperty;
            }
            throw new AssertFailedException(message ?? $"Expected type to be derived of [{type.Name}] but was [{typeof(P).Name}]");
        }

        public static AssertObjectProperty<T,P> HasValue<T,P,O>(this AssertObjectProperty<T,P> assertObjectProperty, O value, string message = null)
        {
            Assert.AreEqual(value, assertObjectProperty.Property, message ?? $"Expected value [{value}], but was [{assertObjectProperty.Property}]");
            return assertObjectProperty;
        }

        public static AssertObjectProperty<T, P> WithValue<T, P, O>(this AssertObjectProperty<T, P> assertObjectProperty, O value, string message = null)
        {
            Assert.AreEqual(value, assertObjectProperty.Property, message ?? $"Expected value [{value}], but was [{assertObjectProperty.Property}]");
            return assertObjectProperty;
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
