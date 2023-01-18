using FluentAssertion.MSTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnonymousData.Tests
{
    public class ClassForTesting
    {
        public int IntProperty { get; set; }
        public DateTime DateTimeProperty { get; set; }
        public string StringField;
        public TimeSpan TimeSpanField;
        public ForTesting EnumField;
        public List<string> List;
        public object Object;
        public StringBuilder StringBuilder;
        public SortedList SortedList;
        public ConcurrentQueue<int> ConcurrentList;
        public Dictionary<string, int> Dictionary;
        public List<ClassForTesting> RecursiveList;
        public ClassForTesting[] RecursiveArray;
        public string[] StringArray;
        public ClassForTesting SameObject;
        public DateTime? NullableDateTime { get; set; }
        public int? NullableInt { get; set; }
        public double? NullableDouble { get; set; }
        public ForTesting? NullableEnum { get; set; }
        public TimeSpan? NullableTimeSpan { get; set; }
        public Guid? NullableGuid { get; set; }
    }

    public enum ForTesting
    {
        Yes = 0,
        Just = 100,
        For = -1,
        Testing
    }

    [TestClass]
    public class AnyTests
    {
        #region Of T
        [TestMethod]
        public void Of_should_return_instance_of_object()
        {
            var generatedType = Any.Of<object>();
            Assert.IsNotNull(generatedType);
        }

        [TestMethod]
        public void Of_should_return_instance_of_int()
        {
            var generatedType = Any.Of<int>();
            Assert.That.This(generatedType).IsNotNull().And().HasNonDefaultValue();
        }

        [TestMethod]
        public void Of_should_return_instance_of_string()
        {
            var generatedType = Any.Of<string>();
            Assert.That.This(generatedType).IsNotNull().And().HasNonDefaultValue();
        }

        [TestMethod]
        public void Of_should_return_instance_of_List_of_string()
        {
            var generatedType = Any.Of<List<string>>();
            Assert.That.These(generatedType).IsNotNullOrEmpty();
        }

        [TestMethod]
        public void Of_should_return_instance_of_string_array()
        {
            var generatedType = Any.Of<string[]>();
            Assert.That.These(generatedType).IsNotNullOrEmpty();
        }

        [TestMethod]
        public void Of_should_return_instance_of_Entity_array()
        {
            var generatedType = Any.Of<ClassForTesting[]>();
            Assert.That.These(generatedType).IsNotNullOrEmpty();
        }

        [TestMethod]
        public void Of_should_return_instance_of_Dictionary()
        {
            var generatedType = Any.Of<Dictionary<string, ClassForTesting>>();
            Assert.That.These(generatedType).IsNotNullOrEmpty();
        }

        [TestMethod]
        public void Of_should_return_type_instance_with_random_properties()
        {
            var randomType = Any.Of<ClassForTesting>();

            Assert.That.This(randomType).IsNotNull()
                .And().HasNonDefault(e => e.Object)
                .And().HasNonDefault(e => e.SortedList)
                .And().HasNonDefault(e => e.StringBuilder)
                .And().HasNonDefault(e => e.StringField)
                .And().HasNonDefault(e => e.TimeSpanField)
                .And().HasNonDefault(e => e.SameObject)
                .And().HasNonDefault(e => e.RecursiveList)
                .And().HasNonDefault(e => e.RecursiveArray)
                .And().HasNonDefault(e => e.StringArray)
                .And().HasNonDefault(e => e.List);

            Assert.That.These(randomType.List).IsNotNullOrEmpty();
            Assert.That.These(randomType.RecursiveArray).IsNotNullOrEmpty();
            Assert.That.These(randomType.RecursiveList).IsNotNullOrEmpty();
            Assert.That.These(randomType.StringArray).IsNotNullOrEmpty();
            Assert.That.These(randomType.Dictionary).IsNotNullOrEmpty();

            Assert.That.These(randomType.ConcurrentList).IsNotNull();
        }
        #endregion

        #region Of Type
        [TestMethod]
        public void Of_Type_should_return_instance_of_object()
        {
            var generatedType = Any.Of<object>();
            Assert.IsNotNull(generatedType);
        }

        [TestMethod]
        public void Of_Type_should_return_instance_of_int()
        {
            var generatedType = Any.Of<int>();
            Assert.That.This(generatedType).IsNotNull().And().HasNonDefaultValue();
        }

        [TestMethod]
        public void Of_Type_should_return_instance_of_string()
        {
            var generatedType = Any.Of<string>();
            Assert.That.This(generatedType).IsNotNull().And().HasNonDefaultValue();
        }

        [TestMethod]
        public void Of_Type_should_return_instance_of_List_of_string()
        {
            var generatedType = Any.Of<List<string>>();
            Assert.That.These(generatedType).IsNotNullOrEmpty();
        }

        [TestMethod]
        public void Of_Type_should_return_instance_of_string_array()
        {
            var generatedType = Any.Of<string[]>();
            Assert.That.These(generatedType).IsNotNullOrEmpty();
        }

        [TestMethod]
        public void Of_Type_should_return_instance_of_Entity_array()
        {
            var generatedType = Any.Of<ClassForTesting[]>();
            Assert.That.These(generatedType).IsNotNullOrEmpty();
        }

        [TestMethod]
        public void Of_Type_should_return_instance_of_Dictionary()
        {
            var generatedType = Any.Of<Dictionary<string, ClassForTesting>>();
            Assert.That.These(generatedType).IsNotNullOrEmpty();
        }

        [TestMethod]
        public void Of_Type_should_return_type_instance_with_random_properties()
        {
            var randomType = Any.Of(typeof(ClassForTesting));

            Assert.That.This(randomType).IsNotNull()
                .And().IsDerivedFrom(typeof(ClassForTesting))
                .As<ClassForTesting>()
                .HasNonDefault(e => e.Object)
                .And().HasNonDefault(e => e.SortedList)
                .And().HasNonDefault(e => e.StringBuilder)
                .And().HasNonDefault(e => e.StringField)
                .And().HasNonDefault(e => e.TimeSpanField)
                .And().HasNonDefault(e => e.SameObject)
                .And().HasNonDefault(e => e.RecursiveList)
                .And().HasNonDefault(e => e.RecursiveArray)
                .And().HasNonDefault(e => e.StringArray)
                .And().HasNonDefault(e => e.List)
                .And().HasNonDefault(o => o.NullableDateTime)
                .And().HasNonDefault(o => o.NullableDouble)
                .And().HasNonDefault(o => o.NullableEnum)
                .And().HasNonDefault(o => o.NullableGuid)
                .And().HasNonDefault(o => o.NullableInt)
                .And().HasNonDefault(o => o.NullableTimeSpan);


            var classForTesting = (ClassForTesting)randomType;
            Assert.That.These(classForTesting.List).IsNotNullOrEmpty();
            Assert.That.These(classForTesting.RecursiveArray).IsNotNullOrEmpty();
            Assert.That.These(classForTesting.RecursiveList).IsNotNullOrEmpty();
            Assert.That.These(classForTesting.StringArray).IsNotNullOrEmpty();
            Assert.That.These(classForTesting.Dictionary).IsNotNullOrEmpty();
            Assert.That.These(classForTesting.ConcurrentList).IsNotNull();
        }
        #endregion

        #region In Enum
        [TestMethod]
        public void In_should_retun_random_enumeration_value()
        {
            for (int i = 0; i < 100000; i++)
            {
                var randomValue = Any.In<ForTesting>();
                Assert.IsTrue(ForTesting.For == randomValue ||
                    ForTesting.Just == randomValue ||
                    ForTesting.Testing == randomValue ||
                    ForTesting.Yes == randomValue);
            }
        }
        #endregion

        #region In IEnumerable
        [TestMethod]
        public void In_should_retun_random_element_in_the_list_of_parameters()
        {
            var options = new List<string> { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
            var result = Any.In<string>(options);
            Assert.IsTrue(options.Contains(result));
        }

        [TestMethod]
        public void In_should_retun_random_element_in_the_list_of_parameters_executing_many_times_without_clashing_with_unique()
        {
            var _ = Any.Unique.Int();

            var options = new List<string> { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };

            for (var i = 0; i < 10000; i++)
            {
                var result = Any.In<string>(options);
                Assert.IsTrue(options.Contains(result));
            }
        }

        [TestMethod]
        public void In_should_retun_the_provided_option_if_only_one_available()
        {
            var options = new List<string> { "OnlyOne" };
            var result = Any.In<string>(options);
            Assert.AreEqual(options.First(), result);
        }

        [TestMethod]
        public void In_should_not_throw_if_nothing_is_provided()
        {
            Any.In((IEnumerable<string>)null);
        }
        #endregion

        #region NotIn
        [TestMethod]
        public void NotIn_should_not_return_any_Int_if_in_the_exclusion_list()
        {
            var exclusions = new List<int> { Any.Int(), Any.Int(), Any.Int(), Any.Int(), Any.Int(), Any.Int(), Any.Int(), Any.Int() };

            for (int i = 0; i < 100000; i++)
            {
                Assert.IsFalse(exclusions.Contains(Any.NotIn(exclusions)));
            }
        }

        [TestMethod]
        public void NotIn_should_not_return_any_String_if_in_the_exclusion_list()
        {
            var exclusions = new List<string> { Any.String(), Any.String(), Any.String(), Any.String(), Any.String(), Any.String(), Any.String(), Any.String() };

            for (int i = 0; i < 100000; i++)
            {
                Assert.IsFalse(exclusions.Contains(Any.NotIn(exclusions)));
            }
        }

        [TestMethod]
        public void NotIn_should_not_return_any_DateTime_if_in_the_exclusion_list()
        {
            var exclusions = new List<ForTesting> { ForTesting.Yes, ForTesting.Just, ForTesting.Testing };

            for (int i = 0; i < 1000; i++)
            {
                Assert.IsFalse(Any.NotIn(exclusions) == ForTesting.For);
            }
        }

        [TestMethod]
        public void NotIn_should_not_return_any_Bool_if_in_the_exclusion_list()
        {
            for (int i = 0; i < 10000; i++) {
                Assert.IsFalse(Any.NotIn(new List<bool> { true }));
            }
        }
        #endregion

        #region Of Params
        [TestMethod]
        public void In_should_return_random_element_in_the_parameters()
        {
            var options = new List<string> { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
            var result = Any.In("Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday");
            Assert.IsTrue(options.Contains(result));
        }

        [TestMethod]
        public void In_should_return_the_provided_option_if_only_one_available()
        {
            var result = Any.In("OnlyOption");
            Assert.AreEqual("OnlyOption", result);
        }

        [TestMethod]
        public void Of_should_not_throw_if_nothing_is_provided()
        {
            var result = Any.Of<string>();
        }

        #endregion

        #region RangedInt
        [TestMethod]
        public void Int_should_return_random_integer_within_the_range()
        {
            for (int i = 0; i < 100000; i++)
            {
                var result = Any.Int(minValue: 0, maxValue: 100);
                Assert.IsTrue(result >= 0 && result <= 100);
            }
        }
        #endregion

        #region Int
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        [DataRow(4)]
        [DataRow(5)]
        [DataRow(6)]
        [DataRow(7)]
        [DataRow(8)]
        [DataRow(9)]
        [DataRow(10)]
        [DataTestMethod]
        public void Int_should_allow_to_specify_the_maximum_number_of_digits_the_returned_int_should_be_made_of(int maximumDigits)
        {
            for (int i = 0; i < 1000000; i++)
            {
                var randomInt = Any.Int(maximumDigits);
                Assert.IsTrue(randomInt.ToString().Length <= maximumDigits,
                    $"Expected {maximumDigits} but the value {randomInt} had {randomInt.ToString().Length} digits");
            }
        }
        #endregion

        #region Long
        [TestMethod]
        public void Long_should_allow_to_specify_the_maximum_number_of_digits_the_returned_int_should_be_made_of()
        {
            for (int i = 0; i < 1000000; i++)
            {
                var _ = Any.Long();
            }
        }
        #endregion

        #region String
        [DataRow("d5f43g", 10)]
        [DataRow("4g2g4", 16)]
        [DataRow("Name", 100)]
        [DataRow("Test", 101)]
        [DataRow("wooo", 987)]
        [DataTestMethod]
        public void String_returns_string_with_the_desired_prefix_suffix_length(string prefix, int length)
        {
            var anonymousString = Any.String(prefix, length);
            Assert.IsTrue(anonymousString.StartsWith($"{prefix}"));
            Assert.AreEqual(length, anonymousString.Length);
        }

        [DataRow(0)]
        [DataRow(-10)]
        [DataRow(-1)]
        [DataTestMethod]
        public void String_throws_exception_if_length_is_less_than_one(int length)
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => Any.String(length: length), "length must be greater than zero");
        }

        [DataRow(10)]
        [DataRow(16)]
        [DataRow(100)]
        [DataRow(101)]
        [DataRow(987)]
        [DataTestMethod]
        public void String_returns_string_with_no_prefix_and_desired_length(int length)
        {
            var what = Any.String(length: length);
            var whaaaat = what.Length;
            Assert.AreEqual(length, whaaaat);
        }

        [TestMethod]
        public void UTFString_returns_strings_with_UTF_characters()
        {
            var anonymousString = Any.String(length: 1000, charSet: CharSet.UTF16);

            foreach (var currentChar in anonymousString.ToCharArray())
            {
                if (currentChar > 255) return;
            }

            Assert.Fail("No UTF characters found int the result string");
        }

        [TestMethod]
        public void UTFString_returns_only_valid_UTF18_characters()
        {
            var anonymousString = Any.String(length: 10000, charSet: CharSet.UTF16);
            var transcodedString = Encoding.Unicode.GetString(Encoding.Unicode.GetBytes(anonymousString));
            Assert.AreEqual(anonymousString, transcodedString, "The random string should only contain UTF16 characters and be compatible with econding and decoding");
        }
        #endregion

        #region Email
        [TestMethod]
        public void Email_should_return_a_valid_formatted_email()
        {
            var email = Any.Email();
            try
            {
                var valid = new System.Net.Mail.MailAddress(email);
            }
            catch
            {
                Assert.Fail($"The email generated is not valid: '{email}'");
            }
        }
        #endregion

        #region Uri
        [TestMethod]
        public void Uri_should_return_an_http_url()
        {
            var url = Any.Uri().ToString();
            Assert.IsTrue(url.StartsWith("http://"));
            Assert.IsTrue(url.EndsWith(".any/"));
        }

        [TestMethod]
        public void Uri_should_return_an_url_with_the_given_protocol_when_specified()
        {
            var url = Any.Uri("ftp").ToString();
            Assert.IsTrue(url.StartsWith("ftp://"));
            Assert.IsTrue(url.EndsWith(".any/"));
        }
        #endregion

        #region Url
        [TestMethod]
        public void Url_should_return_an_http_url()
        {
            var url = Any.Uri().ToString();
            Assert.IsTrue(url.StartsWith("http://"));
            Assert.IsTrue(url.EndsWith(".any/"));
        }

        [TestMethod]
        public void Url_should_return_an_url_with_the_given_protocol_when_specified()
        {
            var url = Any.Uri("ftp").ToString();
            Assert.IsTrue(url.StartsWith("ftp://"));
            Assert.IsTrue(url.EndsWith(".any/"));
        }
        #endregion

        #region TimeSpan
        [TestMethod]
        public void Timespan_invoked_passing_zero_throws_exception()
        {
            Assert.ThrowsException<ArgumentException>(() => Any.TimeSpan(0, 0, 0, 0), "Cannot set all limits to zero");
            Assert.ThrowsException<ArgumentException>(() => Any.TimeSpan(maximumDays: 0), "Cannot set all limits to zero");
            Assert.ThrowsException<ArgumentException>(() => Any.TimeSpan(maximumMinutes: 0), "Cannot set all limits to zero");
            Assert.ThrowsException<ArgumentException>(() => Any.TimeSpan(maximumHours: 0), "Cannot set all limits to zero");
            Assert.ThrowsException<ArgumentException>(() => Any.TimeSpan(maximumSeconds: 0), "Cannot set all limits to zero");
        }

        [TestMethod]
        public void Timespan_invoked_with_no_arguments_returns_a_TimeSpan_for_maximum_10_days()
        {
            var tenDaysTimeSpan = new TimeSpan(10, 0, 0, 0);
            for (int i = 0; i < 100000; i++)
            {
                Assert.IsTrue(tenDaysTimeSpan >= Any.TimeSpan());
            }
        }

        [DataRow(100, 0, 0, 0)]
        [DataRow(1, 1, 1, 1)]
        [DataRow(0, 0, 0, 1)]
        [DataRow(0, 0, 5, 0)]
        [DataRow(0, 3, 0, 0)]
        [DataTestMethod]
        public void Timespan_invoked_with_limits_should_return_a_TimeSpan_not_longer_than_specified_limits(int days, int hours, int minutes, int seconds)
        {
            var tenDaysTimeSpan = new TimeSpan(days, hours, minutes, seconds);
            for (int i = 0; i < 1000000; i++)
                Assert.IsTrue(tenDaysTimeSpan >= Any.TimeSpan(days, hours, minutes, seconds));
        }
        #endregion

        #region Unique
        [TestMethod]
        public void Unique_int_should_return_1000_unique_values_using_default_int_generation()
        {
            var generatedInts = new List<int>();

            for (int i = 0; i < 1000; i++)
            {
                var generated = Any.Unique.Int();
                Assert.That.These(generatedInts).DoesNotContain(generated);
                generatedInts.Add(generated);
            }
        }

        [TestMethod]
        public void Unique_int_should_return_5000_unique_values_using_default_int_generation_for_10000_times_resetting()
        {
            for (int iterations = 0; iterations < 10000; iterations++) {
                var generatedInts = new List<int>();

                for (int i = 0; i < 1000; i++)
                {
                    var generated = Any.Unique.Int();
                    Assert.That.These(generatedInts).DoesNotContain(generated);
                    generatedInts.Add(generated);
                }

                Any.ResetUniqueValues();
            }            
        }

        [TestMethod]
        public void Unique_int_should_throw_exception_when_unable_to_find_enough_uniques_after_10000_retries()
        {
            Assert.ThrowsException<Exception>(() =>
            {
                for (int i = 0; i < 10000; i++) { Any.Unique.Int(minValue: 1, maxValue: 10000); }
            }, "Exceeded the number of retry available to find a unique value, use the Unique feature wisely and consider length, ranges and other factors which can quickly lead to exaustion of available values to randomly find.");
        }
        #endregion
    }
}
