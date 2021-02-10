using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
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
        [TestMethod]
        public void Of_should_return_type_instance_with_random_properties()
        {
            for (int i = 0; i < 100000; i++)
            {
                var randomType = Any.Of<ClassForTesting>();

                Assert.IsNotNull(randomType.StringField);
                Assert.IsNotNull(randomType.TimeSpanField);
                Assert.IsNotNull(randomType.DateTimeProperty);
                Assert.IsNull(randomType.Object);
                Assert.IsNull(randomType.List);
            }
        }

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

            foreach(var currentChar in anonymousString.ToCharArray())
            {
                if(currentChar > 255) return;
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
            for(int i = 0; i < 100000; i++)
            {
                Assert.IsTrue(tenDaysTimeSpan >= Any.TimeSpan());
            }
        }

        [DataRow(100,0,0,0)]
        [DataRow(1,1,1,1)]
        [DataRow(0,0,0,1)]
        [DataRow(0,0,5,0)]
        [DataRow(0,3,0,0)]
        [DataTestMethod]
        public void Timespan_invoked_with_limits_should_return_a_TimeSpan_not_longer_than_specified_limits(int days, int hours, int minutes, int seconds)
        {
            var tenDaysTimeSpan = new TimeSpan(days, hours, minutes, seconds);
            for (int i = 0; i < 1000000; i++)
                Assert.IsTrue(tenDaysTimeSpan >= Any.TimeSpan(days, hours, minutes, seconds));
        }
        #endregion
    }
}