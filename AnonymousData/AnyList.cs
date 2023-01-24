using System;
using System.Collections.Generic;
using System.Linq;

namespace AnonymousData
{
    public class AnyList
    {
        private readonly int NumberOfElements;

        public AnyList(int quantity)
        {
            NumberOfElements = quantity;
        }

        /// <summary>
        /// Generate a list of random number is generated with the specified maximum length of digits
        /// </summary>
        /// <example>
        /// Get a random integer including zero
        /// </example>
        /// <code>
        /// var expectedResult = Any.Int();
        /// </code>
        /// <example>
        /// Get a random integer of maximum 4 digits, excluding zero
        /// </example>
        /// <code>
        /// var expectedResult = Any.Int(4, false);
        /// </code>
        /// <example>
        /// Get a random integer with sign
        /// </example>
        /// <code>
        /// var expectedResult = Any.Int(onlyPositive: false);
        /// </code>
        /// <param name="maxLength">Maximum number of digits</param>
        /// <param name="allowZero">True by default allows zero as result</param>
        /// <param name="onlyPositive">True by default allows only postive integers including zero</param>
        /// <param name="minValue">Minimum value of the range</param>
        /// <param name="maxValue">Maximum value of the range (This has priority over lenght)</param>
        /// <returns>Random integer number</returns>
        public List<int> Int(int maxLength = 5, bool allowZero = true, bool onlyPositive = true, int? minValue = null, int? maxValue = null)
        {
            var result = new List<int>();
            for (int i = 0; i < NumberOfElements; i++)
            {
                result.Add(Any.Int(maxLength, allowZero, onlyPositive, minValue, maxValue));
            }
            return result;
        }

        /// <summary>
        /// Generate a list of string with an optional prefix and a random suffix of the desired length
        /// </summary>
        /// <example>
        /// Get a random string
        /// </example>
        /// <code>
        /// var expectedResult = Any.String();
        /// </code>
        /// <example>
        /// Get a random string of lenght 10
        /// </example>
        /// <code>
        /// var expectedResult = Any.String(10);
        /// </code>
        /// <example>
        /// Get a random string of lenght 10 with prefix Name_
        /// </example>
        /// <code>
        /// var expectedResult = Any.String(10, "Name_");
        /// </code>
        /// <example>
        /// Get a random UTF-16 string, long 10 characters
        /// </example>
        /// <code>
        /// var expectedResult = Any.String(length: 10, utf: true);
        /// </code>
        /// <param name="length">Length of the random part of the string</param>
        /// <param name="prefix">Prefix of the generated string</param>
        /// <param name="charSet">Character set to be used for the string generation</param>
        /// <returns>Randomly generated string</returns>
        public List<string> String(string prefix = "", int length = 15, CharSet charSet = CharSet.Alphanumeric)
        {
            var result = new List<string>();
            for (int i = 0; i < NumberOfElements; i++)
            {
                result.Add(Any.String(prefix, length, charSet));
            }
            return result;
        }

        /// <summary>
        /// Generate a list of random boolean value
        /// </summary>
        /// <example>
        /// Get a boolean value
        /// </example>
        /// <code>
        /// var expectedResult = Any.Bool();
        /// </code>
        /// <returns>Random boolean value</returns>
        public List<bool> Bool()
        {
            var result = new List<bool>();
            for (int i = 0; i < NumberOfElements; i++)
            {
                result.Add(Any.Bool());
            }
            return result;
        }

        /// <summary>
        /// Generate a list of random signed byte value
        /// </summary>
        /// <example>
        /// Get a random signed byte
        /// </example>
        /// <code>
        /// var expectedResult = Any.Byte();
        /// </code>
        /// <returns>Random signed byte value</returns>
        public List<sbyte> SByte()
        {
            var result = new List<sbyte>();
            for (int i = 0; i < NumberOfElements; i++)
            {
                result.Add(Any.SByte());
            }
            return result;
        }

        /// <summary>
        /// Generate a list of random unsigned byte value
        /// </summary>
        /// <example>
        /// Get a random signed byte
        /// </example>
        /// <code>
        /// var expectedResult = Any.Byte();
        /// </code>
        /// <returns>Random signed byte value</returns>
        public List<byte> Byte()
        {
            var result = new List<byte>();
            for (int i = 0; i<NumberOfElements; i++)
            {
                result.Add(Any.Byte());
            }
            return result;
        }

        /// <summary>
        /// Generate a list of random short value
        /// </summary>
        /// <example>
        /// Get a random Short
        /// </example>
        /// <code>
        /// var expectedResult = Any.Short();
        /// </code>
        /// <returns>Random short value</returns>
        public List<short> Short(short? minValue = null, short? maxValue = null)
        {
            var result = new List<short>();
            for (int i = 0; i < NumberOfElements; i++)
            {
                result.Add(Any.Short(minValue, maxValue));
            }
            return result;
        }

        /// <summary>
        /// Generate a random long value
        /// </summary>
        /// <param name="onlyPositive">True by default allows only postive long including zero</param>
        /// <returns>Random long value</returns>
        public List<long> Long(bool onlyPositive = true)
        {
            var result = new List<long>();
            for (int i = 0; i < NumberOfElements; i++)
            {
                result.Add(Any.Long(onlyPositive));
            }
            return result;
        }

        /// <summary>
        /// Generate a list of random double with integer part made of 4 digits and decimal part made of 2 digits.
        /// The number of digits for integer and decimal part can be customized.
        /// </summary>
        /// <param name="integerLenght">Number of digits of the integer part of the result</param>
        /// <param name="decimalLenght">Number of digits of the decimal part of the result</param>
        /// <returns>Double value</returns>
        public List<double> Double(int integerLenght = 4, int decimalLenght = 2)
        {
            var result = new List<double>();
            for (int i = 0; i < NumberOfElements; i++)
            {
                result.Add(Any.Double(integerLenght, decimalLenght));
            }
            return result;
        }

        /// <summary>
        /// Generate a list of random decimal with integer part made of up to 4 digits and decimal part made of 2 digits.
        /// The number of digits for integer and decimal part can be customized.
        /// </summary>
        /// <param name="integerLenght">Number of digits of the integer part of the result</param>
        /// <param name="decimalLenght">Number of digits of the decimal part of the result</param>
        /// </summary>
        /// <returns>Decimal value</returns>
        public List<decimal> Decimal(int integerLenght = 4, int decimalLenght = 2)
        {
            var result = new List<decimal>();
            for (int i = 0; i < NumberOfElements; i++)
            {
                result.Add(Any.Decimal(integerLenght, decimalLenght));
            }
            return result;
        }

        /// <summary>
        /// Generate a list of random float with integer part made of 4 digits and decimal part made of 2 digits.
        /// The number of digits for integer and decimal part can be customized.
        /// </summary>
        /// <param name="integerLenght">Number of digits of the integer part of the result</param>
        /// <param name="decimalLenght">Number of digits of the decimal part of the result</param>
        /// </summary>
        /// <returns>Float value</returns>
        public List<float> Float(int integerLenght = 4, int decimalLenght = 2)
        {
            var result = new List<float>();
            for (int i = 0; i<NumberOfElements; i++)
            {
                result.Add(Any.Float(integerLenght, decimalLenght));
            }
            return result;
        }

        /// <summary>
        /// Generate a list of random TimeSpan of maximum 1000 days.
        /// The maximum number of days, hours, minutes and seconds can be customised.
        /// </summary>
        /// <example>
        /// Get a random timespan
        /// </example>
        /// <code>
        /// var expectedResult = Any.TimeSpan();
        /// </code>
        /// <example>
        /// Get a random timespan of maximum 5 minutes
        /// </example>
        /// <code>
        /// var expectedResult = Any.TimeSpan(0, 0, 5);
        /// </code>
        /// <param name="maximumDays"></param>
        /// <returns></returns>
        public List<TimeSpan> TimeSpan(int? maximumDays = null, int? maximumHours = null, int? maximumMinutes = null, int? maximumSeconds = null)
        {
            var result = new List<TimeSpan>();
            for (int i = 0; i < NumberOfElements; i++)
            {
                result.Add(Any.TimeSpan(maximumDays, maximumHours, maximumMinutes, maximumSeconds));
            }
            return result;
        }

        /// <summary>
        /// Generate alist of random date time in the future, it is possible to request a date time in the past.
        /// </summary>
        /// <example>
        /// Get a random datetime in the future
        /// </example>
        /// <code>
        /// var expectedResult = Any.DateTime();
        /// </code>
        /// <example>
        /// Get a random datetime in the past
        /// </example>
        /// <code>
        /// var expectedResult = Any.DateTime(false);
        /// </code>
        /// <example>
        /// Get a random datetime in the future but before 2090/12/31
        /// </example>
        /// <code>
        /// var expectedResult = Any.DateTime(new DateTime(2090,12,31));
        /// </code>
        /// <param name="future">Optional boolean value, default value true will generate a date in the future</param>
        /// <param name="limit">Set a limit date and time either in the past or the future</param>
        /// <returns>Random DateTime</returns>
        public List<DateTime> DateTime(bool future = true, DateTime? limit = null)
        {
            var result = new List<DateTime>();
            for (int i = 0; i < NumberOfElements; i++)
            {
                result.Add(Any.DateTime(future, limit));
            }
            return result;
        }

        /// <summary>
        /// Generate a list of random UTF-16 Character
        /// </summary>
        /// <example>
        /// Get a random char
        /// </example>
        /// <code>
        /// var expectedResult = Any.Char();
        /// </code>
        /// <param name="charSet">Character set used for the character generation</param>
        /// <returns>Random character</returns>
        public List<char> Char(CharSet charSet = CharSet.Alphanumeric)
        {
            var result = new List<char>();
            for (int i = 0; i < NumberOfElements; i++)
            {
                result.Add(Any.Char(charSet));
            }
            return result;
        }

        /// <summary>
        /// Get a list of values randomly selected from an Enumeration
        /// </summary>
        /// <typeparam name="T">Enumeration type</typeparam>
        /// <returns>A value of the enumeration</returns>
        public List<T> In<T>() where T : Enum
        {
            var result = new List<T>();
            for (int i = 0; i < NumberOfElements; i++)
            {
                result.Add(Any.In<T>());
            }
            return result;
        }

        /// <summary>
        /// Gets an enumeration of objects from which one will be randomly selected
        /// </summary>
        /// <typeparam name="T">Type of objects to be seleted</typeparam>
        /// <param name="options">List of options to select from</param>
        /// <returns>A randomly selected option</returns>
        public List<T> In<T>(IEnumerable<T> options)
        {
            var result = new List<T>();
            for (int i = 0; i < NumberOfElements; i++)
            {
                result.Add(Any.In<T>(options));
            }
            return result;
        }

        /// <summary>
        /// Gets a list of objects each eslected from a list of parameters
        /// </summary>
        /// <typeparam name="T">Type of objects to be seleted</typeparam>
        /// <param name="options">List of options to select from</param>
        /// <returns>A randomly selected option</returns>
        public List<T> In<T>(params T[] options)
        {
            return In(options.AsEnumerable());
        }

        /// <summary>
        /// Generates a list of random Email
        /// </summary>
        /// <returns>Well formatted email</returns>
        public List<string> Email()
        {
            var result = new List<string>();
            for (int i = 0; i < NumberOfElements; i++)
            {
                result.Add(Any.Email());
            }
            return result;
        }

        /// <summary>
        /// Generates a list of random Uri
        /// </summary>
        /// <param name="protocol">Desired protocol to be used, by default is http</param>
        /// <returns>Rando Uri</returns>
        public List<Uri> Uri(string protocol = "http")
        {
            var result = new List<Uri>();
            for (int i = 0; i < NumberOfElements; i++)
            {
                result.Add(Any.Uri(protocol));
            }
            return result;
        }

        /// <summary>
        /// Generates a list of random Url
        /// </summary>
        /// <param name="protocol">Desired protocol to be used, by default is http</param>
        /// <returns>Rando Uri</returns>
        public List<string> Url(string protocol = "http")
        {
            var result = new List<string>();
            for (int i = 0; i < NumberOfElements; i++)
            {
                result.Add(Any.Url(protocol));
            }
            return result;
        }

        /// <summary>
        /// Generates a list of new Guid
        /// </summary>
        /// <returns></returns>
        public List<Guid> Guid()
        {
            var result = new List<Guid>();
            for (int i = 0; i < NumberOfElements; i++)
            {
                result.Add(Any.Guid());
            }
            return result;
        }

        /// <summary>
        /// Generates a list of Base64 string from a random string, with an optional prefix and a random suffix of the desired length
        /// <example>
        /// Get a random Base64 string
        /// </example>
        /// <code>
        /// var expectedResult = Any.Base64String();
        /// </code>
        /// <example>
        /// Get a random Base64 string with the original string of lenght 10
        /// </example>
        /// <code>
        /// var expectedResult = Any.Base64String(10);
        /// </code>
        /// <example>
        /// Get a random Base64 string, using a string of lenght 10 with prefix Name_
        /// </example>
        /// <code>
        /// var expectedResult = Any.Base64String(10, "Name_");
        /// </code>
        /// <example>
        /// Get a random Base64 string using a UTF-16 string long 10 characters
        /// </example>
        /// <code>
        /// var expectedResult = Any.Base64String(length: 10, utf: true);
        /// </code>
        /// </summary>
        /// <param name="length">Length of the random part of the string</param>
        /// <param name="prefix">Prefix of the generated string</param>
        /// <param name="charSet">Character set to be used for the string generation</param>
        /// <returns>Base 64 string</returns>
        public List<string> Base64String(string prefix = "", int length = 15, CharSet charSet = CharSet.Alphanumeric)
        {
            var result = new List<string>();
            for (int i = 0; i < NumberOfElements; i++)
            {
                result.Add(Any.Base64String(prefix, length, charSet));
            }
            return result;
        }
    }
}
