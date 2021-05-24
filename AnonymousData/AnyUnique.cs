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
    public class AnyUnique
    {
        internal Dictionary<Type, List<object>> UniqueValue;

        internal AnyUnique()
        {
            UniqueValue = new Dictionary<Type, List<object>>();
        }

        internal T TryGetUniqueValue<T>(T input, out bool notUnique)
        {
            if (!UniqueValue.ContainsKey(typeof(T)))
            {
                UniqueValue.Add(typeof(T), new List<object> { input });
                notUnique = false;
                return input;
            }

            // Handle custom checks
            if (typeof(T) == typeof(Uri)) notUnique = UniqueValue[typeof(T)].Any(o => ((Uri)o).ToString() == input.ToString());
            else if (typeof(T) == typeof(StringBuilder)) notUnique = UniqueValue[typeof(T)].Any(o => ((StringBuilder)o).ToString() == input.ToString());
            else if (typeof(T) == typeof(MailAddress)) notUnique = UniqueValue[typeof(T)].Any(o => ((MailAddress)o).ToString() == input.ToString());
            else notUnique = UniqueValue[typeof(T)].Contains(input);

            UniqueValue[typeof(T)].Add(input);
            return input;
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
        public int Int(int maxLength = 5, bool allowZero = true, bool onlyPositive = true, int? minValue = null, int? maxValue = null)
        {
            if (maxLength < 5)
                throw new ArgumentOutOfRangeException($"{nameof(maxLength)} must be greater than 5 when requesting Unique values");

            if ((maxValue.HasValue && minValue.HasValue && maxValue.Value - minValue.Value < 10000) &&
                (maxValue.HasValue && int.MaxValue - maxValue.Value < 10000) &&
                (minValue.HasValue && int.MaxValue - minValue.Value < 10000))
                    throw new ArgumentOutOfRangeException($"It is not possible to specify a range smaller than 10000 when requesting Unique values");
                        
            return Any.Int(maxLength, allowZero, onlyPositive, minValue, maxValue);
        }       
    }
}
