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
        /// The brush for numbers.
        /// </summary>
        Number,

        /// <summary>
        /// The brush for identifiers that represent a type.
        /// </summary>
        TypeIdentifier,

        /// <summary>
        /// The caret brush when in insertion mode.
        /// </summary>
        CaretInsertion,

        /// <summary>
        /// The caret brush when in override mode.
        /// </summary>
        CaretOverride,
    }
}
