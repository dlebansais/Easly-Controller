namespace EaslyDraw
{
    /// <summary>
    /// Class describing an hexadecimal (base 16) integer.
    /// </summary>
    public class HexIntegerBase : IntegerBase
    {
        /// <summary>
        /// The suffix for hexadecimal integers.
        /// </summary>
        public override string Suffix { get { return ":H"; } }

        /// <summary>
        /// The number of digits for hexadecimal integers.
        /// </summary>
        public override int Radix { get { return 16; } }

        /// <summary>
        /// Checks if a character is an hex digit, and return the corresponding digit value.
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

            else if (c >= 'a' && c <= 'f')
            {
                value = c - 'a' + 10;
                IsParsed = true;
            }

            else if (c >= 'A' && c <= 'F')
            {
                value = c - 'A' + 10;
                IsParsed = true;
            }

            return IsParsed;
        }
    }
}
