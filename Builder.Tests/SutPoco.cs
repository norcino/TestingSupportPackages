using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace Builder.Tests
{
#pragma warning disable 0649
    internal class SutPoco
    {
        public SutPoco AnotherPocoField;
        public List<SutPoco> MorePocosField;
        public bool BoolField;

        [Key]
        public int IntField;
        public long LongField;

        [MaxLength(100)]
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
        public Guid GuidProperty { get; set; }
        public MailAddress EmailProperty { get; set; }
        public Uri UriProperty { get; set; }
        public string GetOnlyProperty { get; }
        public string PrivateSetProperty { get; private set; }

        public DateTime? NullableDateTime { get; set; }
        public int? NullableInt { get; set; }
        public double? NullableDouble { get; set; }
        public SutEnum? NullableEnum { get; set; }
        public TimeSpan? NullableTimeSpan { get; set; }
        public Guid? NullableGuid { get; set; }
    }
#pragma warning restore 0649
}
