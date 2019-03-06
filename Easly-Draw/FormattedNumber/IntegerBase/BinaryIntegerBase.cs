namespace EaslyDraw
{
    using System.Diagnostics;

    /// <summary>
    /// Class describing a binary (base 2) integer.
    /// </summary>
    public class BinaryIntegerBase : IntegerBase
    {
        /// <summary>
        /// The suffix for binary integers.
        /// </summary>
        public override string Suffix { get { return ":B"; } }

        /// <summary>
        /// The number of digits for binary integers.
        /// </summary>
        public override int Radix { get { return 2; } }

        /// <summary>
        /// Checks if a character is a binary digit, and return the corresponding value.
        /// </summary>
        /// <param name="digit">The character to check.</param>
        /// <param name="value">The digit's value.</param>
        /// <returns>True if <paramref name="digit"/> is a valid digit; Otherwise, false.</returns>
        public override bool IsValidDigit(char digit, out int value)
        {
            value = 0;
            bool IsParsed = false;

            if (digit >= '0' && digit <= '1')
            {
                value = digit - '0';
                IsParsed = true;
            }

            return IsParsed;
        }

        /// <summary>
        /// Returns the digit corresponding to a value.
        /// </summary>
        /// <param name="value">The value.</param>
        public override char ToDigit(int value)
        {
            Debug.Assert(value >= 0 && value < Radix);

            return (char)('0' + value);
        }

        /// <summary>
        /// Returns the value corresponding to a digit.
        /// </summary>
        /// <param name="digit">The digit.</param>
        public override int ToValue(char digit)
        {
            Debug.Assert(digit >= '0' && digit <= '1');

            return digit - '0';
        }
    }
}
