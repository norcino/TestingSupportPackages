using System;
using System.Collections.Generic;

namespace Builder.Tests
{
    internal class ImmutableObject
    {
        public ImmutableObject AnotherPocoProperty { get; }
        public List<ImmutableObject> MorePocosProperty { get; }
        public bool BoolProperty { get; }
        public int IntProperty { get; }
        public long LongProperty { get; }
        public string StringProperty { get; }
        public decimal DecimalProperty { get; }
        public double DoubleProperty { get; }
        public float FloatProperty { get; }
        public char CharProperty { get; }
        public DateTime DateTimeProperty { get; }
        public TimeSpan TimeSpanProperty { get; }
        public SutEnum EnumProperty { get; }

        public ImmutableObject(ImmutableObject anotherPocoProperty,
            List<ImmutableObject> morePocosProperty,
            bool boolProperty,
            int intProperty,
            long longProperty,
            string stringProperty,
            decimal decimalProperty,
            double doubleProperty,
            float floatProperty,
            char charProperty,
            DateTime dateTimeProperty,
            TimeSpan timeSpanProperty,
            SutEnum enumProperty)
        {
            AnotherPocoProperty = anotherPocoProperty;
            MorePocosProperty = morePocosProperty;
            BoolProperty = boolProperty;
            IntProperty = intProperty;
            LongProperty = longProperty;
            StringProperty = stringProperty;
            DecimalProperty = decimalProperty;
            DoubleProperty = doubleProperty;
            FloatProperty = floatProperty;
            CharProperty = charProperty;
            DateTimeProperty = dateTimeProperty;
            TimeSpanProperty = timeSpanProperty;
            EnumProperty = enumProperty;
        }        
    }
}
