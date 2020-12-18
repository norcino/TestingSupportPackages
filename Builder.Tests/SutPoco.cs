using System;
using System.Collections.Generic;

namespace Builder.Tests
{
#pragma warning disable 0649
    internal class SutPoco
    {
        public SutPoco AnotherPocoField;
        public List<SutPoco> MorePocosField;
        public bool BoolField;
        public int IntField;
        public long LongField;
        public string StringField;
        public decimal DecimalField;
        public double DoubleField;
        public float FloatField;
        public char CharField;
        public DateTime DateTimeField;
        public TimeSpan TimeSpanField;
        public SutEnum EnumField;

        public SutPoco AnotherPocoProperty { get; set; }
        public List<SutPoco> MorePocosProperty { get; set; }
        public bool BoolProperty { get; set; }
        public int IntProperty { get; set; }
        public long LongProperty { get; set; }
        public string StringProperty { get; set; }
        public decimal DecimalProperty { get; set; }
        public double DoubleProperty { get; set; }
        public float FloatProperty { get; set; }
        public char CharProperty { get; set; }
        public DateTime DateTimeProperty { get; set; }
        public TimeSpan TimeSpanProperty { get; set; }
        public SutEnum EnumProperty { get; set; }
    }
#pragma warning restore 0649
}