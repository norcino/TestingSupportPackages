using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace FluentAssertion.MSTest.Framework
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SamePropertyObject<T>
    {
        public readonly T ComparedObject;
        public T ExpectedObject;
        public List<Expression<Func<T, object>>> Exclusions;

        public SamePropertyObject(T subject)
        {
            ComparedObject = subject;
            Exclusions = new List<Expression<Func<T, object>>>();
        }
    }
}