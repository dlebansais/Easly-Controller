namespace EaslyDraw
{
    /// <summary>
    /// Class describing a decimal (base 10) integer.
    /// </summary>
    public class DecimalIntegerBase : IntegerBase
    {
        /// <summary>
        /// The suffix for decimal integers.
        /// </summary>
        public override string Suffix { get { return null; } }

        /// <summary>
        /// The number of digits for decimal integers.
        /// </summary>
        public override int Radix { get { return 10; } }

        /// <summary>
        /// Checks if a character is a decimal digit, and return the corresponding digit value.
        /// </summary>
        /// <param name="c">The character to check.</param>
        /// <param name="value">The digit value.</param>
        /// <returns>True if <paramref name="c"/> is a valid digit; Otherwise, false.</returns>
        public override bool IsValidDigit(char c, out int value)
        {
            value = 0;
            bool IsParsed = false;

            if (c >= '0' && c <= '9')
            {
                value = c - '0';
                IsParsed = true;
            }

            return IsParsed;
        }
    }
}
