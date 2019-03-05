namespace EaslyDraw
{
    /// <summary>
    /// The format for a number parsed as real.
    /// </summary>
    public class IntegerNumber : FormattedNumber
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="IntegerNumber"/> class.
        /// </summary>
        /// <param name="integerText">The integer text.</param>
        /// <param name="invalidText">The trailing invalid text, if any.</param>
        /// <param name="canonical">The canonical form of the number.</param>
        public IntegerNumber(string integerText, string invalidText, ICanonicalNumber canonical)
        {
            IntegerText = integerText;
            InvalidText = invalidText;
            Canonical = canonical;
        }
        #endregion

        #region Properties
        /// <summary>
        /// The integer text.
        /// </summary>
        public string IntegerText { get; }

        /// <summary>
        /// The trailing invalid text, if any.
        /// </summary>
        public string InvalidText { get; }

        /// <summary>
        /// The canonical form of the parsed number.
        /// </summary>
        public override ICanonicalNumber Canonical { get; }
        #endregion
    }
}
