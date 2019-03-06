namespace EaslyDraw
{
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
        {
            IntegerText = integerText;
            FractionalText = fractionalText;
            ExplicitExponent = explicitExponent;
            ExponentText = exponentText;
            InvalidText = invalidText;
            Canonical = canonical;
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
        /// The trailing invalid text, if any.
        /// </summary>
        public string InvalidText { get; private set; }

        /// <summary>
        /// The canonical form of the parsed number.
        /// </summary>
        public override ICanonicalNumber Canonical { get; }
        #endregion
    }
}
