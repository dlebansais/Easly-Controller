namespace EaslyDraw
{
    /// <summary>
    /// Choice of brush when drawing.
    /// </summary>
    public enum BrushSettings
    {
        /// <summary>
        /// The default brush.
        /// </summary>
        Default,

        /// <summary>
        /// The brush for keyword decoration.
        /// </summary>
        Keyword,

        /// <summary>
        /// The brush for symbols.
        /// </summary>
        Symbol,

        /// <summary>
        /// The brush for characters.
        /// </summary>
        Character,

        /// <summary>
        /// The brush for text representing discrete values (enum and booleans).
        /// </summary>
        Discrete,

        /// <summary>
        /// The brush for the significand of a number.
        /// </summary>
        NumberSignificand,

        /// <summary>
        /// The brush for the exponent of a number.
        /// </summary>
        NumberExponent,

        /// <summary>
        /// The brush for the invalid trailing part of a number.
        /// </summary>
        NumberInvalid,

        /// <summary>
        /// The brush for identifiers that represent a type.
        /// </summary>
        TypeIdentifier,

        /// <summary>
        /// The brush for comment background.
        /// </summary>
        CommentBackground,

        /// <summary>
        /// The brush for comment foreground.
        /// </summary>
        CommentForeground,

        /// <summary>
        /// The caret brush when in insertion mode.
        /// </summary>
        CaretInsertion,

        /// <summary>
        /// The caret brush when in override mode.
        /// </summary>
        CaretOverride,

        /// <summary>
        /// The selection background.
        /// </summary>
        Selection,

        /// <summary>
        /// The brush for line number foreground.
        /// </summary>
        LineNumberForeground,
    }
}
