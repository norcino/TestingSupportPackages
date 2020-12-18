using System;
using System.Linq.Expressions;

namespace FluentAssertion.MSTest.Framework
{
    public class AssertEachItemProperty<T, TP>
    {        
        public readonly AssertEachItem<T> AssertEachItemObject;
        public readonly Expression<Func<T, TP>> PropertyExpression;

        public AssertEachItemProperty(AssertEachItem<T> assertEachItem, Expression<Func<T, TP>> propertyExpression)
        {
            AssertEachItemObject = assertEachItem;
            PropertyExpression = propertyExpression;
        }
    }
}