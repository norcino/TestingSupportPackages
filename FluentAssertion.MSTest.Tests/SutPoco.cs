using System;
using System.Collections.Generic;

namespace FluentAssertion.MSTest.Tests
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

        internal SutPoco Clone()
        {
            return new SutPoco
            {
                AnotherPocoField = this.AnotherPocoField,
                MorePocosField = this.MorePocosField,
                BoolField = this.BoolField,
                IntField = this.IntField,
                LongField = this.LongField,
                StringField = this.StringField,
                DecimalField = this.DecimalField,
                DoubleField = this.DoubleField,
                FloatField = this.FloatField,
                CharField = this.CharField,
                DateTimeField = this.DateTimeField,
                TimeSpanField = this.TimeSpanField,
                AnotherPocoProperty = this.AnotherPocoProperty,
                MorePocosProperty = this.MorePocosProperty,
                BoolProperty = this.BoolProperty,
                IntProperty = this.IntProperty,
                LongProperty = this.LongProperty,
                StringProperty = this.StringProperty,
                DecimalProperty = this.DecimalProperty,
                DoubleProperty = this.DoubleProperty,
                FloatProperty = this.FloatProperty,
                CharProperty = this.CharProperty,
                DateTimeProperty = this.DateTimeProperty,
                TimeSpanProperty = this.TimeSpanProperty
            };
        }
    }
#pragma warning restore 0649
}