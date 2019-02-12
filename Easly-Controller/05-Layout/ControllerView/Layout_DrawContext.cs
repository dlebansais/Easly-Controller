namespace EaslyController.Layout
{
    using EaslyController.Constants;
    using EaslyController.Controller;
    using NodeController;

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
        /// <param name="size">The size to extend.</param>
        /// <param name="leftMargin">The left margin setting.</param>
        /// <param name="rightMargin">The right margin setting.</param>
        /// <returns>The new size.</returns>
        Size MarginExtended(Size size, Margins leftMargin, Margins rightMargin);
    }
}
