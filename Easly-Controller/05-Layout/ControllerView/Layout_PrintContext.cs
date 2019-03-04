namespace EaslyController.Layout
{
    using EaslyController.Constants;
    using EaslyController.Controller;

    /// <summary>
    /// Context for measuring, arranging and printing cells as string.
    /// </summary>
    public interface ILayoutPrintContext : ILayoutMeasureContext
    {
        /// <summary>
        /// Prints a string, at the location specified in <paramref name="origin"/>.
        /// </summary>
        /// <param name="text">The text to print.</param>
        /// <param name="origin">The location where to start printing.</param>
        /// <param name="textStyle">Style to use for the text.</param>
        void PrintText(string text, Point origin, TextStyles textStyle);

        /// <summary>
        /// Prints a symbol, at the location specified in <paramref name="origin"/>.
        /// </summary>
        /// <param name="symbol">The symbol to print.</param>
        /// <param name="origin">The location where to start printing.</param>
        /// <param name="size">The printing size, padding included.</param>
        /// <param name="padding">The padding to use when printing.</param>
        void PrintSymbol(Symbols symbol, Point origin, Size size, Padding padding);

        /// <summary>
        /// Prints the horizontal separator left of the specified origin and with the specified height.
        /// </summary>
        /// <param name="separator">The separator to print.</param>
        /// <param name="origin">The location where to print.</param>
        /// <param name="height">The separator height.</param>
        void PrintHorizontalSeparator(HorizontalSeparators separator, Point origin, Measure height);

        /// <summary>
        /// Prints the vertical separator above the specified origin and with the specified width.
        /// </summary>
        /// <param name="separator">The separator to print.</param>
        /// <param name="origin">The location where to print.</param>
        /// <param name="width">The separator width.</param>
        void PrintVerticalSeparator(VerticalSeparators separator, Point origin, Measure width);
    }
}
