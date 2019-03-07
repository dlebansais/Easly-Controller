namespace EaslyDraw
{
    using System.Diagnostics;

    /// <summary>
    /// The format for a number parsed as real.
    /// </summary>
    public class RealNumber : FormattedNumber
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="RealNumber"/> class.
        /// </summary>
        /// <param name="integerText">The integer part of the the mantissa (before the dot).</param>
        /// <param name="fractionalText">The fractional part of the the mantissa (after the dot).</param>
        /// <param name="explicitExponent">The exponent, if any.</param>
        /// <param name="exponentText">The exponent text.</param>
        /// <param name="invalidText">The trailing invalid text, if any.</param>
        /// <param name="canonical">The canonical form of the number.</param>
        public RealNumber(string integerText, string fractionalText, ExplicitExponents explicitExponent, string exponentText, string invalidText, ICanonicalNumber canonical)
            : base(invalidText, canonical)
        {
            IntegerText = integerText;
            FractionalText = fractionalText;
            ExplicitExponent = explicitExponent;
            ExponentText = exponentText;
        }
        #endregion

        #region Properties
        /// <summary>
        /// The integer part of the the mantissa (before the dot).
        /// </summary>
        public string IntegerText { get; private set; }

        /// <summary>
        /// The factional part of the the mantissa (after the dot).
        /// </summary>
        public string FractionalText { get; private set; }

        /// <summary>
        /// The exponent, if any.
        /// </summary>
        public ExplicitExponents ExplicitExponent { get; private set; }

        /// <summary>
        /// The exponent text, if <see cref="ExplicitExponent"/> is not <see cref="ExplicitExponents.None"/>.
        /// </summary>
        public string ExponentText { get; private set; }

        /// <summary>
        /// Gets the significand part of the formatted number.
        /// </summary>
        public override string SignificandString { get { return $"{IntegerText}.{FractionalText}"; } }

        /// <summary>
        /// Gets the exponent part of the formatted number.
        /// </summary>
        public override string ExponentString
        {
            get
            {
                string Result = string.Empty;

                if (ExponentText.Length > 0)
                {
                    bool IsHandled = false;
                    switch (ExplicitExponent)
                    {
                        case ExplicitExponents.None:
                            Result += "e";
                            IsHandled = true;
                            break;
                        case ExplicitExponents.Negative:
                            Result += "e-";
                            IsHandled = true;
                            break;
                        case ExplicitExponents.Positive:
                            Result += "e+";
                            IsHandled = true;
                            break;
                    }
                    Debug.Assert(IsHandled);

                    Result += ExponentText;
                }

                return Result;
            }
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Returns the invalid number as a string.
        /// </summary>
        public override string ToString()
        {
            string Exp = ExplicitExponent == ExplicitExponents.None ? "E" : (ExplicitExponent == ExplicitExponents.Negative ? "E-" : "E+");
            return $"{IntegerText}.{FractionalText}{Exp}{ExponentText}{base.ToString()}";
        }
        #endregion
    }
}
