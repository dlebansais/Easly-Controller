namespace EaslyDraw
{
    /// <summary>
    /// Class describing an octal (base 8) integer.
    /// </summary>
    public class OctalIntegerBase : IntegerBase
    {
        /// <summary>
        /// The suffix for octal integers.
        /// </summary>
        public override string Suffix { get { return ":O"; } }

        /// <summary>
        /// The number of digits for octal integers.
        /// </summary>
        public override int Radix { get { return 8; } }

        /// <summary>
        /// Checks if a character is an octal digit, and return the corresponding digit value.
        /// </summary>
        /// <param name="c">The character to check.</param>
        /// <param name="value">The digit value.</param>
        /// <returns>True if <paramref name="c"/> is a valid digit; Otherwise, false.</returns>
        public override bool IsValidDigit(char c, out int value)
        {
            value = 0;
            bool IsParsed = false;

            if (c >= '0' && c <= '7')
            {
                value = c - '0';
                IsParsed = true;
            }

            return IsParsed;
        }
    }
}
