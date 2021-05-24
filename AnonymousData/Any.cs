using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading;

namespace AnonymousData
{
    /// <summary>
    /// Any (Anonymous Data) is a random data generator to use for unit testing
    /// when the actual value is not relevant for the test itself.
    /// <remarks>
    /// It is a bad practice to initialize variables used for assertion with static values.
    /// This in fact can be confused with the expectation to get exactly that value.
    /// </remarks>
    /// </summary>
    public class Any
    {
        private static readonly char[] AphanumericChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
        private static int _seed = Environment.TickCount;
        private static readonly ThreadLocal<Random> RandomWrapper = new ThreadLocal<Random>(() => new Random(Interlocked.Increment(ref _seed)));
        private static bool _doNotAcceptDefaultValues = false;
        private static List<Type> baseTypes = new List<Type> {
                typeof(DateTime),
                typeof(TimeSpan),
                typeof(Guid),
                typeof(Uri),
                typeof(MailAddress),
                typeof(string),
                typeof(object),
                typeof(StringBuilder)
            };

        #region Unique values feature
        private static AnyUnique anyUnique;
        /// <summary>
        /// Guarantee that the value returned in the following method is unique.
        /// To avoid memory leaks consider calling Any.DisposeDefaultValues() when the unicity scope terminates, for example on a test teardown.
        /// </summary>
        /// <returns>Any instance which handles unique values</returns>
        public static AnyUnique Unique { get
            {
                if (anyUnique == null)
                {
                    anyUnique = new AnyUnique();
                }

                return anyUnique;
            }
        }

        public static void ResetUniqueValues()
        {
            anyUnique = null;
        }
        #endregion

        private static Func<int, int, int> GenerateRandomIntValueHandlingUniqueness = (int mi, int ma) =>
        {
            const int MaximumRetryIterations = 10000;
            int result;
            bool notUnique = false;

            int iterations = 0;
            do
            {
                if (anyUnique != null)
                {
                    result = anyUnique.TryGetUniqueValue(Random().Next(mi, ma), out notUnique);
                }
                else
                {
                    result = Random().Next(mi, ma);
                }

                if (++iterations >= MaximumRetryIterations)
                    throw new Exception("Exceeded the number of retry available to find a unique value, use the Unique feature wisely and consider lenght, "+
                        "ranges and other factors which can quickly lead to exaustion of available values to randomly find.");
            }
            while (notUnique);

            return result;
        };

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

            if (maxValue.HasValue && minValue.HasValue)
            {
                return GenerateRandomIntValueHandlingUniqueness(minValue.Value, maxValue.Value);
            }

            var maxValueForLenght = maxLength == 10 ? max : (int)(Math.Pow(10, maxLength)) - 1;
            if (minValue.HasValue)
            {
                return GenerateRandomIntValueHandlingUniqueness(minValue.Value, maxValueForLenght);
            }

            if (maxValue.HasValue)
            {
                var minimum = int.MinValue;
                if (onlyPositive) minimum = 0;
                if (!allowZero) minimum = 1;
                return GenerateRandomIntValueHandlingUniqueness(minimum, maxValue.Value);
            }

            if (onlyPositive)
                return GenerateRandomIntValueHandlingUniqueness(allowZero ? 0 : 1, maxValueForLenght);

            return GenerateRandomIntValueHandlingUniqueness(min, max);
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

            return Random().Next(0, 100) % 2 == 0;
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
            var result = (sbyte)Random().Next(sbyte.MinValue, sbyte.MaxValue);
            
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
            var result = (byte)Random().Next(0, byte.MaxValue);

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
            var result = (short)Random().Next(short.MinValue, short.MaxValue);

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
            long result = Random().Next(onlyPositive ? 0 : int.MinValue, int.MaxValue);

            if(result > 0)
                while(result < int.MaxValue)
                {
                    result += Random().Next(0, int.MaxValue);
                }
            else if (result < 0)
                while (result > int.MinValue)
                {
                    result += Random().Next(int.MinValue, 0);
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
            var randomDouble = Random().NextDouble();
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
            var randomDecimal = (decimal)Random().NextDouble();
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
            var randomFloat = (float)Random().NextDouble();
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

            int days = Random().Next(0, maximumDays ?? (customLimit ? 0 : limitDays));
            int hours = Random().Next(0, maximumHours ?? (customLimit ? 0 : limitHours));
            int minutes = Random().Next(0, maximumMinutes ?? (customLimit ? 0 : limitMinutesSeconds));
            int seconds = Random().Next(0, maximumSeconds ?? (customLimit ? 0 : limitMinutesSeconds));
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
            
            var year = Random().Next(future ? now.Year : minYear, future ? maxYear : now.Year);

            var month = 1;
            
            if (now.Year == year)
            {
                month = future ? Random().Next(now.Month, 12) : Random().Next(1, now.Month);
            } else
            {
                month = Random().Next(1, 12);
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
                    day = Random().Next(minimumDay, maximumDay ?? 30);
                    break;
                case 2:                    
                    day = Random().Next(minimumDay, maximumDay ?? (System.DateTime.IsLeapYear(year) ? 29 : 28));
                    break;
                default:
                    day = Random().Next(minimumDay, maximumDay ?? 31);
                    break;
            }

            var minimumHour = 0;
            var maximumHour = 23;
            if (now.Year == year && now.Month == month && now.Day == day)
            {
                if (future) minimumHour = now.Hour;
                if (!future) maximumHour = now.Hour;
            }
            var hour = Random().Next(minimumHour, maximumHour);

            var minimumMinutes = 0;
            var maximumMinutes = 59;
            if (now.Year == year && now.Month == month && now.Day == day && now.Hour == hour)
            {
                if (future) minimumMinutes = now.Minute;
                if (!future) maximumMinutes = now.Minute;
            }
            var minute = Random().Next(minimumMinutes, maximumMinutes);
            
            var minimumSeconds = 0;
            var maximumSeconds = 59;
            if (now.Year == year && now.Month == month && now.Day == day && now.Hour == hour && now.Minute == minute)
            {
                if (future) minimumSeconds = now.Second;
                if (!future) maximumSeconds = now.Second;
            }
            var second = Random().Next(minimumSeconds, maximumSeconds);

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
                result = AphanumericChars[Random().Next(AphanumericChars.Length)];
            }
            else if (CharSet.ASCII == charSet)
            {
                result = (char)Random().Next(33, 126);
            }
            else
            {
                // Generate random character until it is valid UTF16
                string resultAsString;
                string transcodedResultString;
                do
                {
                    result = (char)Random().Next(char.MinValue, char.MaxValue);
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
            
            var randomIndex = Random().Next(0, options.Length - 1);

            return (T)values.GetValue(randomIndex);
        }

        /// <summary>
        /// Gets an enumeration of objects from which one will be randomly selected
        /// </summary>
        /// <typeparam name="T">Type of objects to be seleted</typeparam>
        /// <param name="options">List of options to select from</param>
        /// <returns>A randomly selected option</returns>
        public static T In<T>(IEnumerable<T> options)
        {
            if (options == null || !options.Any()) return default;
            var index = Int(allowZero: true, maxValue: options.Count()-1);
            return options.ElementAt(index);
        }

        /// <summary>
        /// Gets a series of objects from which one will be randomly selected
        /// </summary>
        /// <typeparam name="T">Type of objects to be seleted</typeparam>
        /// <param name="options">List of options to select from</param>
        /// <returns>A randomly selected option</returns>
        public static T In<T>(params T[] options)
        {
            return In(options.AsEnumerable());
        }

        /// <summary>
        /// Generate an object of type T with random properties and fields
        /// </summary>
        /// <typeparam name="T">Type of the object to generate</typeparam>
        /// <returns>Instance of T with random properties and fields</returns>
        public static T Of<T>(CharSet charSet = CharSet.Alphanumeric)
        {
            if(IsBaseType(typeof(T)))
                return (T)GetBesicTypeValueFor(typeof(T), charSet, false);

            return (T) GenerateObjectMembers(typeof(T), charSet, true);
        }
                
        /// <summary>
        /// Generate an object of Type with random properties and fields
        /// </summary>
        /// <param name="t"></param>
        /// <param name="charSet"></param>
        /// <returns>Instance of Type with random properties and fields</returns>
        public static object Of(Type t, CharSet charSet = CharSet.Alphanumeric)
        {
            if (IsBaseType(t))
                return GetBesicTypeValueFor(t, charSet, false);

            return GenerateObjectMembers(t, charSet, true);
        }

        /// <summary>
        /// Generates a random Email
        /// </summary>
        /// <returns>Well formatted email</returns>
        public static string Email()
        {
            return $"{String(length: 15)}@{String(length: 8)}.any";
        }

        /// <summary>
        /// Generates a random Uri
        /// </summary>
        /// <param name="protocol">Desired protocol to be used, by default is http</param>
        /// <returns>Rando Uri</returns>
        public static Uri Uri(string protocol = "http")
        {
            return new System.Uri($"{protocol}://{String(length:8)}.any");
        }

        /// <summary>
        /// Generates a random Url
        /// </summary>
        /// <param name="protocol">Desired protocol to be used, by default is http</param>
        /// <returns>Rando Uri</returns>
        public static string Url(string protocol = "http")
        {
            return Uri(protocol).ToString();
        }

        /// <summary>
        /// Generates a new Guid
        /// </summary>
        /// <returns></returns>
        public static Guid Guid()
        {
            return System.Guid.NewGuid();
        }

        /// <summary>
        /// Set the random gerator mode to avoid default results with same values as the default
        /// </summary>
        /// <param name="excludeDefaultValues"></param>
        public static void ExcludeDefaultValues(bool excludeDefaultValues)
        {
            _doNotAcceptDefaultValues = excludeDefaultValues;
        }
                
        /// <summary>
        /// Generates the Base64 string from a random string, with an optional prefix and a random suffix of the desired length
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
        public static string Base64String(string prefix = "", int length = 15, CharSet charSet = CharSet.Alphanumeric)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(String(prefix, length, charSet)));
        }
        #region Private methods
        /// <summary>
        /// Generates all properties for an object
        /// </summary>
        /// <param name="t"></param>
        /// <param name="charSet"></param>
        /// <returns></returns>
        private static object GenerateObjectMembers(Type t, CharSet charSet, bool propagate)
        {
            // Handle IEnumerable members if the hierarchy depth has been set
            if (typeof(IEnumerable).IsAssignableFrom(t))
            {
                return GenerateEnumerations(t, charSet, propagate);
            }

            // Reference object requires generation for each member
            var properties = t.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            var e = Activator.CreateInstance(t);

            foreach (var propertyInfo in properties)
            {
                propertyInfo.SetValue(e, GetBesicTypeValueFor(propertyInfo.PropertyType, charSet, propagate));
            }

            var fields = t.GetFields(BindingFlags.Instance | BindingFlags.Public);

            foreach (var fieldInfo in fields)
            {
                fieldInfo.SetValue(e, GetBesicTypeValueFor(fieldInfo.FieldType, charSet, propagate));
            }

            return e;
        }

        private static bool IsBaseType(Type t)
        {
            return t.IsValueType || baseTypes.Contains(t) || t?.BaseType == typeof(Enum);
        }

        private static object GetBesicTypeValueFor(Type type, CharSet charSet, bool propagate)
        {
            if (type == typeof(int) || type == typeof(uint) || type == typeof(Nullable<int>) || type == typeof(Nullable<uint>))
                return (object)Any.Int(3, false);

            if (type == typeof(string))
                return Any.String(length: 15, charSet: charSet);

            if (type == typeof(sbyte) || type == typeof(byte) || type == typeof(Nullable<sbyte>) || type == typeof(Nullable<byte>))
                return (object)Any.Byte();

            if (type == typeof(short) || type == typeof(ushort) || type == typeof(Nullable<short>) || type == typeof(Nullable<ushort>))
                return (object)Any.Short();

            if (type == typeof(long) || type == typeof(ulong) || type == typeof(Nullable<long>) || type == typeof(Nullable<ulong>))
                return (object)Any.Long();

            if (type == typeof(double) || type == typeof(Nullable<double>))
                return (object)Any.Double();

            if (type == typeof(decimal) || type == typeof(Nullable<decimal>))
                return (object)Any.Decimal();

            if (type == typeof(float) || type == typeof(Nullable<float>))
                return (object)Any.Float();

            if (type == typeof(char) || type == typeof(Nullable<char>))
                return (object)Any.Char(charSet);

            if (type == typeof(DateTime) || type == typeof(Nullable<DateTime>))
                return (object)Any.DateTime();

            if (type == typeof(TimeSpan) || type == typeof(Nullable<TimeSpan>))
                return (object)Any.TimeSpan();

            if (type == typeof(Guid) || type == typeof(Nullable<Guid>))
                return (object)Any.Guid();

            if (type == typeof(Uri))
                return (object)Any.Uri();

            if (type == typeof(MailAddress))
                return (object)new MailAddress(Any.Email());

            if (type == typeof(object))
                return new object();

            if (type == typeof(StringBuilder))
                return new StringBuilder(Any.String());

            if (type?.BaseType == typeof(Enum))
            {
                var randomIndex = Any.Int(minValue: 0, maxValue: Enum.GetNames(type).Length - 1);
                return (object)Enum.GetValues(type).GetValue(randomIndex);
            }

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>) && type.GetGenericArguments()[0].IsEnum)
            {
                var enumType = type.GenericTypeArguments.First();
                var randomIndex = Any.Int(minValue: 0, maxValue: Enum.GetNames(enumType).Length - 1);
                return (object)Enum.GetValues(enumType).GetValue(randomIndex);
            }

            if (typeof(IEnumerable).IsAssignableFrom(type))
            {
                return GenerateEnumerations(type, charSet, propagate);
            }

            try
            {
                return Activator.CreateInstance(type);
            }
            catch
            {
                throw new NotSupportedException($"Type {type.Name} is not supported");
            }
        }

        private static object GenerateEnumerations(Type type, CharSet charSet, bool propagate)
        {
            if (typeof(IList).IsAssignableFrom(type) && type.IsGenericType)
            {
                return GenerateList(type, charSet, propagate);
            }

            if (type.IsArray)
            {
                return GenerateArray(type, charSet, propagate);
            }

            if (typeof(IDictionary).IsAssignableFrom(type) && type.IsGenericType)
            {
                return GenerateDictionary(type, charSet, propagate);
            }

            // Non generic List, SortedList, ArrayList, Hashtable ..
            return Activator.CreateInstance(type);
        }

        private static object GenerateDictionary(Type type, CharSet charSet, bool propagate)
        {
            var keyType = type.GenericTypeArguments.FirstOrDefault();
            var valueType = type.GenericTypeArguments.LastOrDefault();
            var dictionary = (IDictionary)Activator.CreateInstance(typeof(Dictionary<,>).MakeGenericType(keyType, valueType));
            
            if (!propagate)
                return dictionary;

            object key, value;

            if (IsBaseType(keyType))
            {
                key = GetBesicTypeValueFor(keyType, charSet, false);
            }
            else
            {
                key = GenerateObjectMembers(keyType, charSet, false);
            }

            if (IsBaseType(valueType))
            {
                value = GetBesicTypeValueFor(valueType, charSet, false);
            }
            else
            {
                value = GenerateObjectMembers(valueType, charSet, false);
            }

            dictionary.Add(key, value);
            return dictionary;
        }

        private static object GenerateList(Type type, CharSet charSet, bool propagate)
        {
            var genericTypeArgument = type.GenericTypeArguments.FirstOrDefault();

            IList list;
            if (genericTypeArgument.IsGenericTypeDefinition)
            {
                list = (IList)Activator.CreateInstance(type.MakeGenericType(genericTypeArgument));
            }
            else
            {
                list = (IList)Activator.CreateInstance(type);
            }

            if (!propagate)
                return list;

            if (IsBaseType(genericTypeArgument))
            {
                list.Add(GetBesicTypeValueFor(genericTypeArgument, charSet, false));
            }
            else
            {
                list.Add(GenerateObjectMembers(genericTypeArgument, charSet, false));
            }

            return list;
        }

        private static object GenerateArray(Type type, CharSet charSet, bool propagate)
        {
            var elementType = type.GetElementType();
            var array = (object[])Array.CreateInstance(elementType, 1);

            if (!propagate)
                return array;

            if (IsBaseType(elementType))
            {
                array[0] = GetBesicTypeValueFor(elementType, charSet, false);
            }
            else
            {
                array[0] = GenerateObjectMembers(elementType, charSet, false);
            }

            return array;
        }

        private static Random Random()
        {
            return RandomWrapper.Value;
        }
        #endregion
    }
}
