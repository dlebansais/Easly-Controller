namespace EaslyDraw
{
    /// <summary>
    /// The format for a number parsed as an integer.
    /// The default base is decimal, unless subclassed.
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
            : base(invalidText, canonical)
        {
            IntegerText = integerText;
        }
        #endregion

        #region Properties
        /// <summary>
        /// The integer text.
        /// </summary>
        public string IntegerText { get; }

        /// <summary>
        /// Gets the significand part of the formatted number.
        /// </summary>
        public override string SignificandString { get { return IntegerText; } }

        /// <summary>
        /// Gets the exponent part of the formatted number.
        /// </summary>
        public override string ExponentString { get { return string.Empty; } }
        #endregion

        #region Debugging
        /// <summary>
        /// Returns the invalid number as a string.
        /// </summary>
        public override string ToString()
        {
            return $"{IntegerText}{base.ToString()}";
        }
        #endregion
    }
}
