namespace EaslyController.Layout
{
    using EaslyController.Constants;
    using EaslyController.Controller;

    /// <summary>
    /// Context for measuring, arranging and drawing cells in a view.
    /// </summary>
    public interface ILayoutDrawContext
    {
        /// <summary>
        /// Height of a line
        /// </summary>
        double LineHeight { get; }

        /// <summary>
        /// Measures a string.
        /// </summary>
        /// <param name="text">The string to measure.</param>
        /// <returns>The size of the string.</returns>
        Size MeasureText(string text);

        /// <summary>
        /// Measures a symbol.
        /// </summary>
        /// <param name="symbol">The symbol to measure.</param>
        /// <returns>The size of the symbol.</returns>
        Size MeasureSymbol(Symbols symbol);

        /// <summary>
        /// Extends a size according to the left and right margin settings.
        /// </summary>
        /// <param name="leftMargin">The left margin setting.</param>
        /// <param name="rightMargin">The right margin setting.</param>
        /// <param name="size">The size to extend with the calculated padding.</param>
        /// <param name="padding">The padding calculated from <paramref name="leftMargin"/> and <paramref name="rightMargin"/>.</param>
        void UpdatePadding(Margins leftMargin, Margins rightMargin, ref Size size, out Padding padding);

        /// <summary>
        /// Draws a string, at the location specified in <paramref name="origin"/>.
        /// </summary>
        /// <param name="text">The text to draw.</param>
        /// <param name="origin">The location where to start drawing.</param>
        void DrawText(string text, Point origin);

        /// <summary>
        /// Draws a symbol, at the location specified in <paramref name="origin"/>.
        /// </summary>
        /// <param name="symbol">The symbol to draw.</param>
        /// <param name="origin">The location where to start drawing.</param>
        void DrawSymbol(Symbols symbol, Point origin);
    }
}
