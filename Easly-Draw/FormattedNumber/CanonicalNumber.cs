namespace EaslyDraw
{
    using System.Diagnostics;

    /// <summary>
    /// Interface to manipulate integer or real numbers of any size.
    /// </summary>
    public interface ICanonicalNumber
    {
        /// <summary>
        /// True if the number is negative.
        /// </summary>
        bool IsNegative { get; }

        /// <summary>
        /// The significand.
        /// </summary>
        string SignificandText { get; }

        /// <summary>
        /// True if the exponent is negative.
        /// </summary>
        bool IsExponentNegative { get; }

        /// <summary>
        /// The exponent.
        /// </summary>
        string ExponentText { get; }

        /// <summary>
        /// The canonic representation.
        /// </summary>
        string CanonicRepresentation { get; }
    }

    /// <summary>
    /// Interface to manipulate integer or real numbers of any size.
    /// </summary>
    public class CanonicalNumber : ICanonicalNumber
    {
        #region Constants
        /// <summary>
        /// The canonical number for zero.
        /// </summary>
        public static readonly CanonicalNumber Zero = new CanonicalNumber(false, IntegerBase.Zero, false, IntegerBase.Zero);
        #endregion

        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="CanonicalNumber"/> class.
        /// </summary>
        /// <param name="isNegative">True if the number is negative.</param>
        /// <param name="significandText">The significand.</param>
        /// <param name="isExponentNegative">True if the exponent is negative.</param>
        /// <param name="exponentText">The exponent.</param>
        public CanonicalNumber(bool isNegative, string significandText, bool isExponentNegative, string exponentText)
        {
            Debug.Assert(IntegerBase.Decimal.IsValidSignificand(significandText));
            Debug.Assert(IntegerBase.Decimal.IsValidNumber(exponentText));
            Debug.Assert(significandText != IntegerBase.Zero || (!isNegative && !isExponentNegative && exponentText == IntegerBase.Zero));

            IsNegative = isNegative;
            SignificandText = significandText;
            IsExponentNegative = isExponentNegative;
            ExponentText = exponentText;

            FormatCanonicString();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CanonicalNumber"/> class.
        /// </summary>
        /// <param name="n">An integer.</param>
        public CanonicalNumber(int n)
        {
            if (n < 0)
            {
                n = -n;
                IsNegative = true;
            }
            else
                IsNegative = false;

            string s = n.ToString();

            IsExponentNegative = false;
            ExponentText = (s.Length - 1).ToString();

            while (s.Length > 1 && s[s.Length - 1] == '0')
                s = s.Substring(0, s.Length - 1);

            SignificandText = s;

            FormatCanonicString();
        }
        #endregion

        #region Properties
        /// <summary>
        /// True if the number is negative.
        /// </summary>
        public bool IsNegative { get; private set; }

        /// <summary>
        /// The significand.
        /// </summary>
        public string SignificandText { get; private set; }

        /// <summary>
        /// True if the exponent is negative.
        /// </summary>
        public bool IsExponentNegative { get; private set; }

        /// <summary>
        /// The exponent.
        /// </summary>
        public string ExponentText { get; private set; }

        /// <summary>
        /// The canonic representation.
        /// </summary>
        public string CanonicRepresentation { get; private set; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Checks if two numbers are equal.
        /// </summary>
        /// <param name="other">The other instance.</param>
        public virtual bool IsEqual(ICanonicalNumber other)
        {
            return IsNegative == other.IsNegative && SignificandText == other.SignificandText && IsExponentNegative == other.IsExponentNegative && ExponentText == other.ExponentText;
        }

        /// <summary>
        /// Checks if <paramref name="number1"/> is lesser than <paramref name="number2"/>.
        /// </summary>
        /// <param name="number1">The first number.</param>
        /// <param name="number2">The second number.</param>
        public static bool operator <(CanonicalNumber number1, CanonicalNumber number2)
        {
            // Compare positive and negative numbers.
            if (number1.IsNegative != number2.IsNegative)
                return number1.IsNegative;

            // If both positive or negative, compare positive and negative exponents.
            if (number1.IsExponentNegative && !number2.IsExponentNegative)
                return !number1.IsNegative;

            else if (!number1.IsExponentNegative && number2.IsExponentNegative)
                return number1.IsNegative;

            // If signs of significands and signs of exponents are identical.
            else
            {
                int ComparedExponent = string.Compare(number1.ExponentText, number2.ExponentText);

                if (ComparedExponent < 0)
                    return number1.IsNegative == number1.IsExponentNegative;
                else if (ComparedExponent > 0)
                    return number1.IsNegative != number1.IsExponentNegative;

                // If exponents are identical, compare significands.
                else
                {
                    int ComparedSignificand = string.Compare(number1.SignificandText, number2.SignificandText);
                    return (ComparedSignificand < 0) == number1.IsNegative;
                }
            }
        }

        /// <summary>
        /// Checks if <paramref name="number1"/> is greater than <paramref name="number2"/>.
        /// </summary>
        /// <param name="number1">The first number.</param>
        /// <param name="number2">The second number.</param>
        public static bool operator >(CanonicalNumber number1, CanonicalNumber number2)
        {
            return number2 < number1;
        }

        /// <summary>
        /// Returns the opposite number.
        /// </summary>
        public virtual ICanonicalNumber OppositeOf()
        {
            return new CanonicalNumber(!IsNegative, SignificandText, IsExponentNegative, ExponentText);
        }

        /// <summary>
        /// Gets the value if it can be represented with a <see cref="int"/>.
        /// </summary>
        /// <param name="value">The value upon return.</param>
        public bool TryParseInt(out int value)
        {
            value = 0;

            if (IsExponentNegative)
                return false;

            if (SignificandText.Length > 10 || ExponentText.Length > 1)
                return false;

            int Significand;
            int Exponent;
            if (!int.TryParse(SignificandText, out Significand) || !int.TryParse(ExponentText, out Exponent))
                return false;

            if (Exponent + 1 < SignificandText.Length)
                return false;

            value = Significand;
            int RemainingDigits = Exponent + 1 - SignificandText.Length;

            while (RemainingDigits-- > 0)
                value *= 10;

            return true;
        }
        #endregion

        #region Implementation
        private void FormatCanonicString()
        {
            if (SignificandText == IntegerBase.Zero)
                CanonicRepresentation = IntegerBase.Zero;

            else
            {
                if (SignificandText.Length == 1)
                    CanonicRepresentation = SignificandText[0] + ".0e" + (IsExponentNegative ? "-" : "+") + ExponentText;

                else
                    CanonicRepresentation = SignificandText[0] + "." + SignificandText.Substring(1) + "e" + (IsExponentNegative ? "-" : "+") + ExponentText;

                if (IsNegative)
                    CanonicRepresentation = '-' + CanonicRepresentation;
            }
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Returns a string representation of this instance.
        /// </summary>
        public override string ToString()
        {
            return CanonicRepresentation;
        }
        #endregion
    }
}
