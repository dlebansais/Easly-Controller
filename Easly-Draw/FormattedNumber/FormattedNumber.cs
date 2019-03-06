namespace EaslyDraw
{
    using System.Diagnostics;

    /// <summary>
    /// Base interface for a number format.
    /// </summary>
    public interface IFormattedNumber
    {
        /// <summary>
        /// The canonical form of the parsed number.
        /// </summary>
        ICanonicalNumber Canonical { get; }
    }

    /// <summary>
    /// Base class for a number format.
    /// All strings can be parsed using this class, it includes trailing invalid characters.
    /// </summary>
    public abstract class FormattedNumber : IFormattedNumber
    {
        #region Init
        /// <summary>
        /// Parses a string to a formatted number.
        /// </summary>
        /// <param name="text">The string to parse.</param>
        public static IFormattedNumber Parse(string text)
        {
            IFormattedNumber Result = null;

            if (!TryParseAsIntegerWithBase(text, IntegerBase.Hexadecimal, out Result) &&
                !TryParseAsIntegerWithBase(text, IntegerBase.Octal, out Result) &&
                !TryParseAsIntegerWithBase(text, IntegerBase.Binary, out Result))
                Result = ParseAsReal(text);

            return Result;
        }
        #endregion

        #region Properties
        /// <summary>
        /// The canonical form of the parsed number.
        /// </summary>
        public abstract ICanonicalNumber Canonical { get; }
        #endregion

        #region Implementation
        /// <summary></summary>
        protected static bool TryParseAsIntegerWithBase(string text, IIntegerBase integerBase, out IFormattedNumber number)
        {
            number = null;
            bool IsParsed = false;

            if (integerBase.Suffix != null && text.EndsWith(integerBase.Suffix))
            {
                ParseAsIntegerWithBase(text, integerBase, out number);
                IsParsed = true;
            }

            return IsParsed;
        }

        /// <summary></summary>
        protected static void ParseAsIntegerWithBase(string text, IIntegerBase integerBase, out IFormattedNumber number)
        {
            string Suffix = integerBase.Suffix;

            Debug.Assert(Suffix == null || Suffix.Length <= text.Length);
            string DigitText = Suffix != null ? text.Substring(0, text.Length - integerBase.Suffix.Length) : text;

            int i;
            for (i = 0; i < DigitText.Length; i++)
            {
                int DigitValue;
                if (!integerBase.IsValidDigit(DigitText[i], out DigitValue))
                    break;
            }

            Debug.Assert(i <= text.Length);

            string ValidText = DigitText.Substring(0, i);
            string InvalidText = DigitText.Substring(i);
            string DecimalNumber = IntegerBase.Convert(ValidText, integerBase, IntegerBase.Decimal);
            string DecimalExponent = (DecimalNumber.Length - 1).ToString();

            ICanonicalNumber Canonical = new CanonicalNumber(false, DecimalNumber, false, DecimalExponent);

            number = new IntegerNumberWithBase(ValidText, InvalidText, integerBase, Canonical);
        }

        /// <summary></summary>
        protected static IFormattedNumber ParseAsReal(string text)
        {
            // If empty, assume invalid.
            if (text.Length == 0)
                return new InvalidNumber(text);

            int n = 0;
            int DigitValue;
            string ValidText;
            string InvalidText;

            // Get the integer part.
            int IntegerBegin = n;
            while (n < text.Length && IntegerBase.Decimal.IsValidDigit(text[n], out DigitValue))
                n++;
            int IntegerEnd = n;

            // If the number is a simple decimal number, return now.
            if (IntegerEnd > 0 && IntegerEnd >= text.Length)
            {
                // The number may not start with a zero unless it's zero itself.
                if ((IntegerEnd > IntegerBegin + 1) && text[IntegerBegin] == '0')
                    return new InvalidNumber(text);
                else
                    return new IntegerNumber(text, string.Empty, new CanonicalNumber(false, text, false, IntegerBase.Zero));
            }

            // If the number contains an invalid character, return now.
            if (text[IntegerEnd] != '.')
            {
                ValidText = text.Substring(0, IntegerEnd);
                InvalidText = text.Substring(ValidText.Length);
                return new IntegerNumber(ValidText, InvalidText, new CanonicalNumber(false, ValidText, false, IntegerBase.Zero));
            }

            string IntegerText = text.Substring(IntegerBegin, IntegerEnd - IntegerBegin);

            n++;

            // Get the fractional part.
            int FractionalBegin = n;
            while (n < text.Length && IntegerBase.Decimal.IsValidDigit(text[n], out DigitValue))
                n++;
            int FractionalEnd = n;

            string FractionalText = text.Substring(FractionalBegin, FractionalEnd - FractionalBegin);

            // Both the integer part and the fractional part must exist, or the whole number is invalid.
            if (IntegerText.Length == 0 || FractionalText.Length == 0)
            {
                ValidText = IntegerText;
                InvalidText = text.Substring(ValidText.Length);
                return new IntegerNumber(ValidText, InvalidText, new CanonicalNumber(false, ValidText, false, IntegerBase.Zero));
            }

            int ZeroIndex = FractionalEnd;
            while (ZeroIndex > FractionalBegin && text[ZeroIndex - 1] == '0')
                ZeroIndex--;

            // The fraction must not end with zero, unless it's zero itself.
            if (ZeroIndex > FractionalBegin && ZeroIndex < FractionalEnd)
            {
                ValidText = text.Substring(0, ZeroIndex);
                InvalidText = text.Substring(ValidText.Length);
                return new RealNumber(IntegerText, FractionalText, ExplicitExponents.None, string.Empty, InvalidText, new CanonicalNumber(false, ValidText, false, IntegerBase.Zero));
            }

            // If the number has no exponent, return now.
            if (FractionalEnd >= text.Length)
            {
                ValidText = text;
                return new RealNumber(IntegerText, FractionalText, ExplicitExponents.None, string.Empty, string.Empty, new CanonicalNumber(false, ValidText, false, IntegerBase.Zero));
            }

            // The number must be followed by the exponent tag.
            if (text[FractionalEnd] != 'e' && text[FractionalEnd] != 'E')
            {
                ValidText = text.Substring(0, FractionalEnd);
                InvalidText = text.Substring(ValidText.Length);
                return new RealNumber(IntegerText, FractionalText, ExplicitExponents.None, string.Empty, InvalidText, new CanonicalNumber(false, ValidText, false, IntegerBase.Zero));
            }

            // The mantissa part must have exactly one digit on the left side, and it cannot be zero.
            if (IntegerEnd != IntegerBegin + 1 || text[IntegerBegin] == '0')
            {
                ValidText = text.Substring(0, IntegerEnd);
                InvalidText = text.Substring(ValidText.Length);
                return new RealNumber(IntegerText, string.Empty, ExplicitExponents.None, string.Empty, InvalidText, new CanonicalNumber(false, ValidText, false, IntegerBase.Zero));
            }

            n++;

            int SuperscriptBegin = n;
            ExplicitExponents ExplicitExponent;

            // Allow exponent sign.
            if (n + 1 < text.Length)
            {
                if (text[n] == '-')
                {
                    ExplicitExponent = ExplicitExponents.Negative;
                    n++;
                }

                else if (text[n] == '+')
                {
                    ExplicitExponent = ExplicitExponents.Positive;
                    n++;
                }

                else
                    ExplicitExponent = ExplicitExponents.None;
            }
            else
                ExplicitExponent = ExplicitExponents.None;

            // Get the exponent.
            int ExponentBegin = n;
            while (n < text.Length && IntegerBase.Decimal.IsValidDigit(text[n], out DigitValue))
                n++;
            int ExponentEnd = n;

            // The exponent cannot be empty.
            if (ExponentBegin >= ExponentEnd)
            {
                ValidText = text.Substring(0, SuperscriptBegin - 1);
                InvalidText = text.Substring(ValidText.Length);
                return new RealNumber(IntegerText, FractionalText, ExplicitExponent, string.Empty, InvalidText, new CanonicalNumber(false, ValidText, false, IntegerBase.Zero));
            }

            string ExponentText = text.Substring(ExponentBegin, ExponentEnd - ExponentBegin);

            ValidText = text.Substring(0, ExponentEnd);
            InvalidText = text.Substring(ValidText.Length);

            return new RealNumber(IntegerText, FractionalText, ExplicitExponent, ExponentText, InvalidText, new CanonicalNumber(false, ValidText, ExplicitExponent == ExplicitExponents.Negative, ExponentText));
        }
        #endregion
    }
}
