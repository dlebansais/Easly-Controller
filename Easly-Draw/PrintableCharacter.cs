namespace EaslyDraw
{
    /// <summary>
    /// A character and the brush to display it.
    /// </summary>
    public class PrintableCharacter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PrintableCharacter"/> class.
        /// </summary>
        /// <param name="c">The character.</param>
        /// <param name="brush">The brush.</param>
        public PrintableCharacter(char c, BrushSettings brush)
        {
            C = c;
            Brush = brush;
        }

        /// <summary>
        /// The character.
        /// </summary>
        public char C { get; }

        /// <summary>
        /// The brush.
        /// </summary>
        public BrushSettings Brush { get; }
    }
}
