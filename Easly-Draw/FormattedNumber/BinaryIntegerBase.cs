namespace EaslyDraw
{
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
        /// Checks if a character is a binary digit, and return the corresponding digit value.
        /// </summary>
        /// <param name="c">The character to check.</param>
        /// <param name="value">The digit value.</param>
        /// <returns>True if <paramref name="c"/> is a valid digit; Otherwise, false.</returns>
        public override bool IsValidDigit(char c, out int value)
        {
            value = 0;
            bool IsParsed = false;

            if (c >= '0' && c <= '1')
            {
                value = c - '0';
                IsParsed = true;
            }

            return IsParsed;
        }
    }
}
