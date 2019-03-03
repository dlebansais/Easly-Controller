namespace EaslyController.Layout
{
    using EaslyController.Constants;
    using EaslyController.Controller;

    /// <summary>
    /// Context for measuring, arranging and printing cells as string.
    /// </summary>
    public interface ILayoutPrintContext
    {
        /// <summary>
        /// Width of a tabulation margin, in number of whitespace.
        /// </summary>
        int TabulationWidth { get; }

        /// <summary>
        /// Gets the width corresponding to a separator between cells in a line, in number of whitespace.
        /// </summary>
        /// <param name="separator">The separator.</param>
        int GetHorizontalSeparatorWidth(HorizontalSeparators separator);

        /// <summary>
        /// Gets the height corresponding to a separator between cells in a column, in number of whitespace.
        /// </summary>
        /// <param name="separator">The separator.</param>
        int GetVerticalSeparatorHeight(VerticalSeparators separator);

        /// <summary>
        /// Measures a string.
        /// </summary>
        /// <param name="text">The string to measure.</param>
        /// <param name="textStyle">Style to use for the text.</param>
        /// <param name="maxTextWidth">The maximum width for a line of text. -1 means no limit.</param>
        /// <returns>The size of the string.</returns>
        Plane MeasureText(string text, TextStyles textStyle, int maxTextWidth);

        /// <summary>
        /// Measures a symbol.
        /// </summary>
        /// <param name="symbol">The symbol to measure.</param>
        /// <returns>The size of the symbol.</returns>
        Plane MeasureSymbol(Symbols symbol);

        /// <summary>
        /// Extends a size according to the left and right margin settings.
        /// </summary>
        /// <param name="leftMargin">The left margin setting.</param>
        /// <param name="rightMargin">The right margin setting.</param>
        /// <param name="size">The size to extend with the calculated padding.</param>
        void UpdatePadding(Margins leftMargin, Margins rightMargin, ref Plane size);

        /// <summary>
        /// Draws a string, at the location specified in <paramref name="origin"/>.
        /// </summary>
        /// <param name="text">The text to draw.</param>
        /// <param name="origin">The location where to start drawing.</param>
        /// <param name="textStyle">Style to use for the text.</param>
        void PrintText(string text, Corner origin, TextStyles textStyle);

        /// <summary>
        /// Draws a symbol, at the location specified in <paramref name="origin"/>.
        /// </summary>
        /// <param name="symbol">The symbol to draw.</param>
        /// <param name="origin">The location where to start drawing.</param>
        /// <param name="size">The drawing size, padding included.</param>
        /// <param name="padding">The padding to use when drawing.</param>
        void PrintSymbol(Symbols symbol, Corner origin, Plane size, Padding padding);

        /// <summary>
        /// Draws the horizontal separator left of the specified origin and with the specified height.
        /// </summary>
        /// <param name="separator">The separator to draw.</param>
        /// <param name="origin">The location where to draw.</param>
        /// <param name="height">The separator height.</param>
        void PrintHorizontalSeparator(HorizontalSeparators separator, Corner origin, int height);

        /// <summary>
        /// Draws the vertical separator above the specified origin and with the specified width.
        /// </summary>
        /// <param name="separator">The separator to draw.</param>
        /// <param name="origin">The location where to draw.</param>
        /// <param name="width">The separator width.</param>
        void PrintVerticalSeparator(VerticalSeparators separator, Corner origin, int width);

        /// <summary>
        /// Get the location where draw occurs corresponding to the specified absolute location.
        /// </summary>
        /// <param name="origin">The absolute location.</param>
        /// <returns>The relative location where things would be drawn.</returns>
        Corner ToRelativeLocation(Corner origin);
    }
}
