using AnonymousData;
using System;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Microsoft.VisualStudio.TestTools.UnitTesting
{
    /// <summary>
    /// Any (Anonymous Data) is a random data generator to use for unit testing
    /// when the actual value is not relevant for the test itself.
    /// <remarks>
    /// It is a bad practice to initialize variables used for assertion with static values.
    /// This in fact can be confused with the expectation to get exactly that value.
    /// </remarks>
    /// </summary>
    public static class Any
    {
        private static readonly char[] AphanumericChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
        private static int _seed = Environment.TickCount;
        private static readonly ThreadLocal<Random> RandomWrapper =
            new ThreadLocal<Random>(() => new Random(Interlocked.Increment(ref _seed)));
        private static bool _doNotAcceptDefaultValues = false;

        private static Random GetThreadRandom()
        {
            return RandomWrapper.Value;
        }

        /// <summary>
        /// Random number is generated with the specified maximum length of digits
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
        public static int Int(int maxLength = 5, bool allowZero = true, bool onlyPositive = true, int? minValue = null, int? maxValue = null)
        {            
            if (_doNotAcceptDefaultValues) allowZero = false;

            var max = maxValue ?? int.MaxValue;
            var min = minValue ?? int.MinValue;

            if (maxLength < 1)
                throw new ArgumentOutOfRangeException($"{nameof(maxLength)} must be greater than zero");

            if (maxLength > 10)
                throw new ArgumentOutOfRangeException($"{nameof(maxLength)} cannot be greater than 10");

            if(maxValue.HasValue && minValue.HasValue)
                return GetThreadRandom().Next(minValue.Value, maxValue.Value);

            var maxValueForLenght = maxLength == 10 ? max : (int)(Math.Pow(10, maxLength)) - 1;
            
            if (onlyPositive)
                return GetThreadRandom().Next(allowZero ? 0 : 1, maxValueForLenght);

            return GetThreadRandom().Next(min, max);
        }

        /// <summary>
        /// Generate a string with an optional prefix and a random suffix of the desired length
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
        public static string String(string prefix = "", int length = 15, CharSet charSet = CharSet.Alphanumeric)
        {
            if (length < 1)
                throw new ArgumentOutOfRangeException($"{nameof(length)} must be greater than zero");

            if (length < prefix.Length)
                throw new ArgumentOutOfRangeException($"{nameof(length)} should be greater then the lenght of the given {nameof(prefix)}");

            var characters = new char[length];
            for (int idx = 0; idx < length; idx++)
            {                
                characters[idx] = prefix.Length > idx ? prefix[idx] : Char(charSet);
            }
            
            return new string(characters);
        }

        /// <summary>
        /// Generate a random boolean value
        /// </summary>
        /// <example>
        /// Get a boolean value
        /// </example>
        /// <code>
        /// var expectedResult = Any.Bool();
        /// </code>
        /// <returns>Random boolean value</returns>
        public static bool Bool()
        {
            if (_doNotAcceptDefaultValues) return true;

            return GetThreadRandom().Next(0, 100) % 2 == 0;
        }

        /// <summary>
        /// Generate a random signed byte value
        /// </summary>
        /// <example>
        /// Get a random signed byte
        /// </example>
        /// <code>
        /// var expectedResult = Any.Byte();
        /// </code>
        /// <returns>Random signed byte value</returns>
        public static sbyte SByte()
        {
            var result = (sbyte)GetThreadRandom().Next(sbyte.MinValue, sbyte.MaxValue);
            
            if (_doNotAcceptDefaultValues && default(sbyte) == result)
                return SByte();

            return result;
        }

        /// <summary>
        /// Generate a random unsigned byte value
        /// </summary>
        /// <example>
        /// Get a random signed byte
        /// </example>
        /// <code>
        /// var expectedResult = Any.Byte();
        /// </code>
        /// <returns>Random signed byte value</returns>
        public static byte Byte()
        {
            var result = (byte)GetThreadRandom().Next(0, byte.MaxValue);

            if (_doNotAcceptDefaultValues && default(byte) == result)
                return Byte();

            return result;
        }

        /// <summary>
        /// Generate a random short value
        /// </summary>
        /// <example>
        /// Get a random Short
        /// </example>
        /// <code>
        /// var expectedResult = Any.Short();
        /// </code>
        /// <returns>Random short value</returns>
        public static short Short()
        {
            var result = (short)GetThreadRandom().Next(short.MinValue, short.MaxValue);

            if (_doNotAcceptDefaultValues && default(short) == result)
                return Short();

            return result;   
        }

        /// <summary>
        /// Generate a random long value
        /// </summary>
        /// <param name="onlyPositive">True by default allows only postive long including zero</param>
        /// <returns>Random long value</returns>
        public static long Long(bool onlyPositive = true)
        {
            long result = GetThreadRandom().Next(onlyPositive ? 0 : int.MinValue, int.MaxValue);

            if(result > 0)
                while(result < int.MaxValue)
                {
                    result += GetThreadRandom().Next(0, int.MaxValue);
                }
            else if (result < 0)
                while (result > int.MinValue)
                {
                    result += GetThreadRandom().Next(int.MinValue, 0);
                }

            if (_doNotAcceptDefaultValues && default(long) == result)
                return Long(onlyPositive);

            return result;
        }

        /// <summary>
        /// Generate random double with integer part made of 4 digits and decimal part made of 2 digits.
        /// The number of digits for integer and decimal part can be customized.
        /// </summary>
        /// <param name="integerLenght">Number of digits of the integer part of the result</param>
        /// <param name="decimalLenght">Number of digits of the decimal part of the result</param>
        /// <returns>Double value</returns>
        public static double Double(int integerLenght = 4, int decimalLenght = 2)
        {
            var randomDouble = GetThreadRandom().NextDouble();
            var result = ((double)Any.Int(integerLenght)) + Math.Round(randomDouble, decimalLenght);
            
            if (_doNotAcceptDefaultValues && default(double) == result)
                return Double(integerLenght, decimalLenght);

            return result;
        }

        /// <summary>
        /// Generate random decimal with integer part made of up to 4 digits and decimal part made of 2 digits.
        /// The number of digits for integer and decimal part can be customized.
        /// </summary>
        /// <param name="integerLenght">Number of digits of the integer part of the result</param>
        /// <param name="decimalLenght">Number of digits of the decimal part of the result</param>
        /// </summary>
        /// <returns>Decimal value</returns>
        public static decimal Decimal(int integerLenght = 4, int decimalLenght = 2)
        {
            var randomDecimal = (decimal)GetThreadRandom().NextDouble();
            var result = ((decimal)Any.Int(integerLenght)) + Math.Round(randomDecimal, decimalLenght);

            if (_doNotAcceptDefaultValues && default(decimal) == result)
                return Decimal(integerLenght, decimalLenght);

            return result;
        }

        /// <summary>
        /// Generate a random float with integer part made of 4 digits and decimal part made of 2 digits.
        /// The number of digits for integer and decimal part can be customized.
        /// </summary>
        /// <param name="integerLenght">Number of digits of the integer part of the result</param>
        /// <param name="decimalLenght">Number of digits of the decimal part of the result</param>
        /// </summary>
        /// <returns>Float value</returns>
        public static float Float(int integerLenght = 4, int decimalLenght = 2)
        {
            var randomFloat = (float)GetThreadRandom().NextDouble();
            var result = ((float)Any.Int(integerLenght)) + (float)Math.Round(randomFloat, decimalLenght);

            if (_doNotAcceptDefaultValues && default(float) == result)
                return Float(integerLenght, decimalLenght);

            return result;
        }

        /// <summary>
        /// Generate a random TimeSpan of maximum 1000 days.
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
        public static TimeSpan TimeSpan(int? maximumDays = null,
            int? maximumHours = null,
            int? maximumMinutes = null,
            int? maximumSeconds = null)
        {
            if (maximumDays > 1000) throw new ArgumentOutOfRangeException("The supported maximum days is 1000");
            if (maximumMinutes > 59) throw new ArgumentOutOfRangeException("The supported maximum minutes is 59");
            if (maximumSeconds > 59) throw new ArgumentOutOfRangeException("The supported maximum seconds is 59");
            if (maximumHours > 23) throw new ArgumentOutOfRangeException("The supported maximum hours is 23");
            
            if ((maximumDays.HasValue && maximumDays == 0 || !maximumDays.HasValue) &&
                (maximumMinutes.HasValue && maximumMinutes == 0 || !maximumMinutes.HasValue) &&
                (maximumSeconds.HasValue && maximumSeconds == 0 || !maximumSeconds.HasValue) &&
                (maximumHours.HasValue && maximumHours == 0 || !maximumHours.HasValue) &&
                (maximumDays.HasValue || maximumHours.HasValue || maximumMinutes.HasValue || maximumSeconds.HasValue))
                throw new ArgumentException("Cannot set all limits to zero");

            var customLimit = false;

            // If no limits are set, the maximum of 10 days is used
            if (maximumDays == maximumHours &&
                maximumMinutes == maximumSeconds &&
                maximumHours == maximumSeconds &&
                maximumSeconds == null)
            {
                customLimit = false;
                maximumDays = 10;
            } else customLimit = true;

            var limitDays = 1000;
            var limitHours = 23;
            var limitMinutesSeconds = 59;

            int days = GetThreadRandom().Next(0, maximumDays ?? (customLimit ? 0 : limitDays));
            int hours = GetThreadRandom().Next(0, maximumHours ?? (customLimit ? 0 : limitHours));
            int minutes = GetThreadRandom().Next(0, maximumMinutes ?? (customLimit ? 0 : limitMinutesSeconds));
            int seconds = GetThreadRandom().Next(0, maximumSeconds ?? (customLimit ? 0 : limitMinutesSeconds));
            var result = new TimeSpan(days, hours, minutes, seconds);
                
            if (_doNotAcceptDefaultValues && default(TimeSpan) == result)
                return TimeSpan(maximumDays, maximumHours, maximumMinutes, maximumSeconds);

            return result;
        }

        /// <summary>
        /// Generate a random date time in the future, it is possible to request a date time in the past.
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
        public static DateTime DateTime(bool future = true, DateTime? limit = null)
        {
            var now = System.DateTime.Now;
            if (limit.HasValue && limit < now && future)
                throw new ArgumentOutOfRangeException($"The {nameof(limit)} parameter is in the past, so a random date in the future cannot be generated.");

            if (limit.HasValue && limit > now && !future)
                throw new ArgumentOutOfRangeException($"The {nameof(limit)} parameter is in the future, so a random date in the past cannot be generated.");

            int minYear = 1900;
            if (!future && limit.HasValue) minYear = limit.Value.Year;

            int maxYear = 2100;
            if (future && limit.HasValue) maxYear = limit.Value.Year;
            
            var year = GetThreadRandom().Next(future ? now.Year : minYear, future ? maxYear : now.Year);

            var month = 1;
            
            if (now.Year == year)
            {
                month = future ? GetThreadRandom().Next(now.Month, 12) : GetThreadRandom().Next(1, now.Month);
            } else
            {
                month = GetThreadRandom().Next(1, 12);
            }
            
            var day = 1;                        
            var minimumDay = 1;
            int? maximumDay = null;

            if (now.Year == year && now.Month == month)
            {
                if (future) minimumDay = now.Day;
                if (!future) maximumDay = now.Day;
            }

            switch (month)
            {
                case 4:
                case 6:
                case 9:
                case 11:
                    day = GetThreadRandom().Next(minimumDay, maximumDay ?? 30);
                    break;
                case 2:                    
                    day = GetThreadRandom().Next(minimumDay, maximumDay ?? (System.DateTime.IsLeapYear(year) ? 29 : 28));
                    break;
                default:
                    day = GetThreadRandom().Next(minimumDay, maximumDay ?? 31);
                    break;
            }

            var minimumHour = 0;
            var maximumHour = 23;
            if (now.Year == year && now.Month == month && now.Day == day)
            {
                if (future) minimumHour = now.Hour;
                if (!future) maximumHour = now.Hour;
            }
            var hour = GetThreadRandom().Next(minimumHour, maximumHour);

            var minimumMinutes = 0;
            var maximumMinutes = 59;
            if (now.Year == year && now.Month == month && now.Day == day && now.Hour == hour)
            {
                if (future) minimumMinutes = now.Minute;
                if (!future) maximumMinutes = now.Minute;
            }
            var minute = GetThreadRandom().Next(minimumMinutes, maximumMinutes);
            
            var minimumSeconds = 0;
            var maximumSeconds = 59;
            if (now.Year == year && now.Month == month && now.Day == day && now.Hour == hour && now.Minute == minute)
            {
                if (future) minimumSeconds = now.Second;
                if (!future) maximumSeconds = now.Second;
            }
            var second = GetThreadRandom().Next(minimumSeconds, maximumSeconds);

            if(now.Year == year && now.Month == month && 
                now.Day == day && now.Hour == hour && 
                now.Minute == minute && now.Second == second)
            {
                return limit.Value.AddSeconds(future ? 1 : -1);
            }

            var result = new DateTime(
                year,
                month,
                day,
                hour,
                minute,
                second);

            if (_doNotAcceptDefaultValues && default(DateTime) == result)
                return DateTime(future, limit);

            return result;
        }

        /// <summary>
        /// Generate a random UTF-16 Character
        /// </summary>
        /// <example>
        /// Get a random char
        /// </example>
        /// <code>
        /// var expectedResult = Any.Char();
        /// </code>
        /// <param name="charSet">Character set used for the character generation</param>
        /// <returns>Random character</returns>
        public static char Char(CharSet charSet = CharSet.Alphanumeric)
        {
            char result = (char)65533;

            if (CharSet.Alphanumeric == charSet)
            {
                result = AphanumericChars[GetThreadRandom().Next(AphanumericChars.Length)];
            }
            else if (CharSet.ASCII == charSet)
            {
                result = (char)GetThreadRandom().Next(33, 126);
            }
            else
            {
                // Generate random character until it is valid UTF16
                string resultAsString;
                string transcodedResultString;
                do
                {
                    result = (char)GetThreadRandom().Next(char.MinValue, char.MaxValue);
                    resultAsString = result.ToString();
                    transcodedResultString = Encoding.Unicode.GetString(Encoding.Unicode.GetBytes(resultAsString));
                } while (resultAsString != transcodedResultString);
            }
            
            if (_doNotAcceptDefaultValues && default(char) == result)
                return Char(charSet);

            return result;
        }

        /// <summary>
        /// Get randomly an Enumeratio value
        /// </summary>
        /// <typeparam name="T">Enumeration type</typeparam>
        /// <returns>A value of the enumeration</returns>
        public static T In<T>() where T : Enum
        {
            var options = Enum.GetNames(typeof(T));
            var values = Enum.GetValues(typeof(T));
            
            var randomIndex = GetThreadRandom().Next(0, options.Length - 1);

            return (T)values.GetValue(randomIndex);
        }

        /// <summary>
        /// Generate an object of type T with random properties and fields
        /// </summary>
        /// <typeparam name="T">Type of the object to generate</typeparam>
        /// <returns>Instance of T with random properties and fields</returns>
        public static T Of<T>() where T : new()
        {
            var e = (T)Activator.CreateInstance(typeof(T));
            var properties = e.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);

            foreach (var propertyInfo in properties)
            {
                propertyInfo.SetValue(e, GenerateAnonymousData(e, propertyInfo.PropertyType, propertyInfo.Name));
            }

            var fields = e.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);

            foreach (var fieldInfo in fields)
            {
                fieldInfo.SetValue(e, GenerateAnonymousData(e, fieldInfo.FieldType, fieldInfo.Name));
            }
            return e;
        }

        internal static object GenerateAnonymousData(object entity, Type propertyType, string propertyName)
        {
            if (propertyType == typeof(string))
                return Any.String(propertyName);

            if (propertyType == typeof(sbyte) || propertyType == typeof(byte) || propertyType == typeof(Byte) || propertyType == typeof(SByte))
                return Any.Byte();

            if (propertyType == typeof(short) || propertyType == typeof(ushort) || propertyType == typeof(Int16) || propertyType == typeof(UInt16))
                return Any.Short();

            if (propertyType == typeof(int) || propertyType == typeof(uint) || propertyType == typeof(Int32) || propertyType == typeof(UInt32))
                return Any.Int(3, false);

            if (propertyType == typeof(long) || propertyType == typeof(ulong) || propertyType == typeof(Int64) || propertyType == typeof(UInt64))
                return Any.Long();

            if (propertyType == typeof(double) || propertyType == typeof(Double))
                return Any.Double();

            if (propertyType == typeof(decimal) || propertyType == typeof(Decimal))
                return Any.Decimal();

            if (propertyType == typeof(float) || propertyType == typeof(Single))
                return Any.Float();

            if (propertyType == typeof(char) || propertyType == typeof(Char))
                return Any.Char();

            if (propertyType == typeof(DateTime))
                return Any.DateTime();

            if (propertyType == typeof(TimeSpan))
                return Any.TimeSpan();
            
            if (propertyType?.BaseType == typeof(Enum))
            {
                var randomIndex = GetThreadRandom().Next(0, Enum.GetNames(propertyType).Length - 1);
                return Enum.GetValues(propertyType).GetValue(randomIndex);
            }

            if (propertyType.IsValueType)
                return Activator.CreateInstance(propertyType);

            return null;
        }

        /// <summary>
        /// Set the random gerator mode to avoid default results with same values as the default
        /// </summary>
        /// <param name="excludeDefaultValues"></param>
        public static void ExcludeDefaultValues(bool excludeDefaultValues)
        {
            _doNotAcceptDefaultValues = excludeDefaultValues;
        }
    }
}