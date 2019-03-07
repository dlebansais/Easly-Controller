namespace EaslyDraw
{
    using System.Diagnostics;
    using System.Globalization;

    /// <summary>
    /// Base interface for a number format.
    /// </summary>
    public interface IFormattedNumber
    {
        /// <summary>
        /// The trailing invalid text, if any.
        /// </summary>
        string InvalidText { get; }

        /// <summary>
        /// The canonical form of the parsed number.
        /// </summary>
        ICanonicalNumber Canonical { get; }

        /// <summary>
        /// Gets the significand part of the formatted number.
        /// </summary>
        string SignificandString { get; }

        /// <summary>
        /// Gets the exponent part of the formatted number.
        /// </summary>
        string ExponentString { get; }
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

            // Debug.WriteLine(Result);
            return Result;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FormattedNumber"/> class.
        /// </summary>
        /// <param name="invalidText">The trailing invalid text, if any.</param>
        /// <param name="canonical">The canonical form of the number.</param>
        public FormattedNumber(string invalidText, ICanonicalNumber canonical)
        {
            InvalidText = invalidText;
            Canonical = canonical;
        }
        #endregion

        #region Properties
        /// <summary>
        /// The trailing invalid text, if any.
        /// </summary>
        public string InvalidText { get; }

        /// <summary>
        /// The canonical form of the parsed number.
        /// </summary>
        public ICanonicalNumber Canonical { get; }

        /// <summary>
        /// Gets the exponent part of the formatted number.
        /// </summary>
        public static string DecimalSeparator { get { return CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator; } }

        /// <summary>
        /// Gets the significand part of the formatted number.
        /// </summary>
        public abstract string SignificandString { get; }

        /// <summary>
        /// Gets the exponent part of the formatted number.
        /// </summary>
        public abstract string ExponentString { get; }
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

            number = new IntegerNumberWithBase(ValidText, InvalidText, Canonical, integerBase);
        }

        /// <summary></summary>
        protected static IFormattedNumber ParseAsReal(string text)
        {
            // If empty, assume invalid.
            if (text.Length == 0)
                return new InvalidNumber(text);

            // Assume '.' as the decimal separator in what follows. This means '.' is ALWAYS a valid separator.
            text = text.Replace(DecimalSeparator, ".");

            // Reject numbers that don't start with a digit.
            int DigitValue;
            if (!IntegerBase.Decimal.IsValidDigit(text[0], out DigitValue))
                return new InvalidNumber(text);

            // If the first digit is 0, only accept the number zero.
            if (DigitValue == 0)
                return new IntegerNumber(IntegerBase.Zero, text.Substring(1), CanonicalNumber.Zero);

            string ValidText;
            string InvalidText;
            string DecimalExponent;
            string SignificandText;
            ICanonicalNumber Canonical;

            // Collect all digits before the decimal separator.
            int n = 1;
            while (n < text.Length && IntegerBase.Decimal.IsValidDigit(text[n], out DigitValue))
                n++;

            string IntegerText = text.Substring(0, n);
            Debug.Assert(IntegerText.Length > 0 && IntegerText[0] != '0');

            // If the number is a simple decimal number or continues with an invalid character, return now.
            if (n >= text.Length || text[n] != '.')
            {
                InvalidText = text.Substring(n);
                DecimalExponent = (n - 1).ToString();
                Canonical = new CanonicalNumber(false, TrailingZeroesRemoved(IntegerText), false, DecimalExponent);
                return new IntegerNumber(IntegerText, InvalidText, Canonical);
            }

            n++;

            // Get the fractional part.
            int FractionalBegin = n;
            int ZeroIndex = n;
            while (n < text.Length && IntegerBase.Decimal.IsValidDigit(text[n], out DigitValue))
            {
                n++;
                if (DigitValue > 0)
                    ZeroIndex = n;
            }

            string FractionalText = text.Substring(IntegerText.Length + 1, ZeroIndex - IntegerText.Length - 1);

            // If the fractional part is empty, or ends with a zero but is not zero, just return the integer.
            if (FractionalText.Length == 0 || FractionalText[FractionalText.Length - 1] == '0')
            {
                ValidText = IntegerText;
                InvalidText = text.Substring(ValidText.Length);
                DecimalExponent = (IntegerText.Length - 1).ToString();
                Canonical = new CanonicalNumber(false, TrailingZeroesRemoved(ValidText), false, DecimalExponent);
                return new IntegerNumber(ValidText, InvalidText, Canonical);
            }

            // If the fractional part is not followed by an exponent, return the corresponding real number that has no exponent.
            if (n == text.Length || (text[n] != 'e' && text[n] != 'E'))
            {
                InvalidText = text.Substring(n);
                SignificandText = IntegerText + FractionalText;
                DecimalExponent = (IntegerText.Length - 1).ToString();
                return new RealNumber(IntegerText, FractionalText, ExplicitExponents.None, string.Empty, InvalidText, new CanonicalNumber(false, SignificandText, false, DecimalExponent));
            }

            Debug.Assert(FractionalText.Length > 0);
            Debug.Assert(FractionalText == IntegerBase.Zero || FractionalText[FractionalText.Length - 1] != '0');
            Debug.Assert(n < text.Length && (text[n] == 'e' || text[n] == 'E'));

            // When an expontent is present, the mantissa part must have exactly one digit on the left side. We already know it's not zero.
            if (IntegerText.Length != 1)
            {
                ValidText = text.Substring(0, 1);
                InvalidText = text.Substring(ValidText.Length);
                return new RealNumber(ValidText, string.Empty, ExplicitExponents.None, string.Empty, InvalidText, new CanonicalNumber(false, ValidText, false, IntegerBase.Zero));
            }

            SignificandText = TrailingZeroesRemoved(IntegerText + FractionalText);

            int MantissaEnd = n;
            n++;

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

            string ExponentText = text.Substring(ExponentBegin, ExponentEnd - ExponentBegin);

            // The exponent cannot be empty, and must not start with zero. In that case, the valid part ends at the exponent character.
            if (ExponentText.Length == 0 || ExponentText[0] == '0')
            {
                ValidText = text.Substring(0, MantissaEnd);
                InvalidText = text.Substring(ValidText.Length);
                DecimalExponent = (IntegerText.Length - 1).ToString();
                return new RealNumber(IntegerText, FractionalText, ExplicitExponents.None, string.Empty, InvalidText, new CanonicalNumber(false, SignificandText, false, DecimalExponent));
            }

            // A number with a valid mantissa and explicit exponent.
            ValidText = text.Substring(0, ExponentEnd);
            InvalidText = text.Substring(ValidText.Length);
            return new RealNumber(IntegerText, FractionalText, ExplicitExponent, ExponentText, InvalidText, new CanonicalNumber(false, SignificandText, ExplicitExponent == ExplicitExponents.Negative, ExponentText));
        }

        /// <summary></summary>
        protected static string TrailingZeroesRemoved(string text)
        {
            while (text.Length > 1 && text[text.Length - 1] == '0')
                text = text.Substring(0, text.Length - 1);

            return text;
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Returns the formatted number as a string.
        /// </summary>
        public override string ToString()
        {
            return $"~{InvalidText} ({Canonical})";
        }
        #endregion
    }
}
