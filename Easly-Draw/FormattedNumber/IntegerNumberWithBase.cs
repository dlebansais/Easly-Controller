namespace EaslyDraw
{
    /// <summary>
    /// The format for a number parsed as integer, with a base.
    /// </summary>
    public class IntegerNumberWithBase : IntegerNumber
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="IntegerNumberWithBase"/> class.
        /// </summary>
        /// <param name="integerText">The integer text.</param>
        /// <param name="invalidText">The trailing invalid text, if any.</param>
        /// <param name="integerBase">The base.</param>
        /// <param name="canonical">The canonical form of the number.</param>
        public IntegerNumberWithBase(string integerText, string invalidText, IIntegerBase integerBase, ICanonicalNumber canonical)
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
    }
}
