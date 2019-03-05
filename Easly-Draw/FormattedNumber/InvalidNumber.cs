﻿namespace EaslyDraw
{
    /// <summary>
    /// The format for a number parsed as totally invalid.
    /// </summary>
    public class InvalidNumber : FormattedNumber
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidNumber"/> class.
        /// </summary>
        /// <param name="invalidText">The invalid text.</param>
        public InvalidNumber(string invalidText)
        {
            InvalidText = invalidText;
            Canonical = new CanonicalNumber(string.Empty, false, string.Empty);
        }

        #region Properties
        /// <summary>
        /// The invalid text.
        /// </summary>
        public string InvalidText { get; private set; }

        /// <summary>
        /// The canonical form of the parsed number.
        /// </summary>
        public override ICanonicalNumber Canonical { get; }
        #endregion
    }
}