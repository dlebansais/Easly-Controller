namespace EaslyDraw
{
    /// <summary>
    /// The format for a number parsed as an integer, with a base.
    /// </summary>
    public class IntegerNumberWithBase : IntegerNumber
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="IntegerNumberWithBase"/> class.
        /// </summary>
        /// <param name="integerText">The integer text.</param>
        /// <param name="invalidText">The trailing invalid text, if any.</param>
        /// <param name="canonical">The canonical form of the number.</param>
        /// <param name="integerBase">The base.</param>
        public IntegerNumberWithBase(string integerText, string invalidText, ICanonicalNumber canonical, IIntegerBase integerBase)
            : base(integerText, invalidText, canonical)
        {
            IntegerBase = integerBase;
        }
        #endregion

        #region Properties
        /// <summary>
        /// The base.
        /// </summary>
        public IIntegerBase IntegerBase { get; private set; }
        #endregion

        #region Debugging
        /// <summary>
        /// Returns the invalid number as a string.
        /// </summary>
        public override string ToString()
        {
            return $"{IntegerText}[{IntegerBase.Radix}]{base.ToString()}";
        }
        #endregion
    }
}
