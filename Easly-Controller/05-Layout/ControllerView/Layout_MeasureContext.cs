namespace EaslyController.Layout
{
    using EaslyController.Constants;
    using EaslyController.Controller;

    /// <summary>
    /// Context for measuring and arranging cells in a view.
    /// </summary>
    public interface ILayoutMeasureContext
    {
        /// <summary>
        /// Width of a tabulation margin, for drawing.
        /// </summary>
        double TabulationWidth { get; }

        /// <summary>
        /// Width of a tabulation margin, in number of whitespace.
        /// </summary>
        int TabulationLength { get; }

        /// <summary>
        /// Height of a line of text.
        /// </summary>
        double LineHeight { get; }

        /// <summary>
        /// Gets the width corresponding to a separator between cells in a line.
        /// </summary>
        /// <param name="separator">The separator.</param>
        double GetHorizontalSeparatorWidth(HorizontalSeparators separator);

        /// <summary>
        /// Gets the width corresponding to a separator between cells in a line, in number of whitespace.
        /// </summary>
        /// <param name="separator">The separator.</param>
        int GetHorizontalSeparatorLength(HorizontalSeparators separator);

        /// <summary>
        /// Gets the height corresponding to a separator between cells in a column.
        /// </summary>
        /// <param name="separator">The separator.</param>
        SeparatorLength GetVerticalSeparatorHeight(VerticalSeparators separator);

        /// <summary>
        /// Measures a string.
        /// </summary>
        /// <param name="text">The string to measure.</param>
        /// <param name="textStyle">Style to use for the text.</param>
        /// <param name="maxTextWidth">The maximum width for a line of text. NaN means no limit.</param>
        /// <returns>The size of the string.</returns>
        Size MeasureTextSize(string text, TextStyles textStyle, double maxTextWidth);

        /// <summary>
        /// Measures a string.
        /// </summary>
        /// <param name="text">The string to measure.</param>
        /// <param name="textStyle">Style to use for the text.</param>
        /// <param name="maxTextLength">The maximum width for a line of text. 0 means no limit.</param>
        /// <returns>The size of the string.</returns>
        Plane MeasureTextPlane(string text, TextStyles textStyle, int maxTextLength);

        /// <summary>
        /// Measures a symbol.
        /// </summary>
        /// <param name="symbol">The symbol to measure.</param>
        /// <returns>The size of the symbol.</returns>
        Size MeasureSymbolSize(Symbols symbol);

        /// <summary>
        /// Measures a symbol.
        /// </summary>
        /// <param name="symbol">The symbol to measure.</param>
        /// <returns>The size of the symbol.</returns>
        Plane MeasureSymbolPlane(Symbols symbol);

        /// <summary>
        /// Extends a size according to the left and right margin settings.
        /// </summary>
        /// <param name="leftMargin">The left margin setting.</param>
        /// <param name="rightMargin">The right margin setting.</param>
        /// <param name="size">The size to extend with the calculated padding.</param>
        /// <param name="padding">The padding calculated from <paramref name="leftMargin"/> and <paramref name="rightMargin"/>.</param>
        void UpdatePadding(Margins leftMargin, Margins rightMargin, ref Size size, out Padding padding);

        /// <summary>
        /// Extends a size according to the left and right margin settings.
        /// </summary>
        /// <param name="leftMargin">The left margin setting.</param>
        /// <param name="rightMargin">The right margin setting.</param>
        /// <param name="plane">The size to extend with the calculated padding.</param>
        /// <param name="padding">The padding calculated from <paramref name="leftMargin"/> and <paramref name="rightMargin"/>.</param>
        void UpdatePadding(Margins leftMargin, Margins rightMargin, ref Plane plane, out SpacePadding padding);
    }
}
