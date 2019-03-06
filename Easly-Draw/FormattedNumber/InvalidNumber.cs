namespace EaslyDraw
{
    /// <summary>
    /// The format for a number parsed as totally invalid.
    /// </summary>
    public class InvalidNumber : FormattedNumber
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidNumber"/> class.
        /// </summary>
        /// <param name="invalidText">The invalid text.</param>
        public InvalidNumber(string invalidText)
            : base(invalidText, CanonicalNumber.Zero)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the significand part of the formatted number.
        /// </summary>
        public override string SignificandString { get { return string.Empty; } }

        /// <summary>
        /// Gets the exponent part of the formatted number.
        /// </summary>
        public override string ExponentString { get { return string.Empty; } }
        #endregion
    }
}
