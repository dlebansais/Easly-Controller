namespace EaslyDraw
{
    using System;
    using EaslyController.Constants;
    using EaslyController.Controller;
    using EaslyController.Layout;

    /// <summary>
    /// An implementation of IxxxPrintContext for WPF.
    /// </summary>
    public class PrintContext : MeasureContext, ILayoutPrintContext, IFormattable
    {
        #region Init
        /// <summary>
        /// Creates and initializes a new context.
        /// </summary>
        public static PrintContext CreatePrintContext()
        {
            PrintContext Result = new PrintContext();
            Result.Update();
            return Result;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrintContext"/> class.
        /// </summary>
        protected PrintContext()
        {
            PrintableArea = new PrintableArea();
            LeftBracketGeometry = new PrintableGeometry('[', ' ', ' ', ' ', ' ', PrintOrientations.Vertical);
            RightBracketGeometry = new PrintableGeometry(']', ' ', ' ', ' ', ' ', PrintOrientations.Vertical);
            LeftCurlyBracketGeometry = new PrintableGeometry('{', ' ', ' ', ' ', ' ', PrintOrientations.Vertical);
            RightCurlyBracketGeometry = new PrintableGeometry('}', ' ', ' ', ' ', ' ', PrintOrientations.Vertical);
            LeftParenthesisGeometry = new PrintableGeometry('(', ' ', ' ', ' ', ' ', PrintOrientations.Vertical);
            RightParenthesisGeometry = new PrintableGeometry(']', ' ', ' ', ' ', ' ', PrintOrientations.Vertical);
            HorizontalLineGeometry = new PrintableGeometry('-', ' ', ' ', ' ', ' ', PrintOrientations.Horizontal);
        }
        #endregion

        #region Implementation of IxxxPrintContext
        /// <summary>
        /// Prints a string, at the location specified in <paramref name="origin"/>.
        /// </summary>
        /// <param name="text">The text to print.</param>
        /// <param name="origin">The location where to start printing.</param>
        /// <param name="textStyle">Style to use for the text.</param>
        public virtual void PrintText(string text, Point origin, TextStyles textStyle)
        {
            BrushSettings Brush = StyleToForegroundBrush(textStyle);
            PrintableArea.Print(text, origin.X.Print, origin.Y.Print, Brush);
        }

        /// <summary>
        /// Prints a symbol, at the location specified in <paramref name="origin"/>.
        /// </summary>
        /// <param name="symbol">The symbol to print.</param>
        /// <param name="origin">The location where to start printing.</param>
        /// <param name="size">The printing size, padding included.</param>
        /// <param name="padding">The padding to use when printing.</param>
        public virtual void PrintSymbol(Symbols symbol, Point origin, Size size, Padding padding)
        {
            switch (symbol)
            {
                default:
                case Symbols.LeftArrow:
                case Symbols.Dot:
                    PrintTextSymbol(SymbolToText(symbol), origin, size, padding);
                    break;
                case Symbols.InsertSign:
                    break;
                case Symbols.LeftBracket:
                    PrintGeometrySymbol(LeftBracketGeometry, origin, size, padding);
                    break;
                case Symbols.RightBracket:
                    PrintGeometrySymbol(RightBracketGeometry, origin, size, padding);
                    break;
                case Symbols.LeftCurlyBracket:
                    PrintGeometrySymbol(LeftCurlyBracketGeometry, origin, size, padding);
                    break;
                case Symbols.RightCurlyBracket:
                    PrintGeometrySymbol(RightCurlyBracketGeometry, origin, size, padding);
                    break;
                case Symbols.LeftParenthesis:
                    PrintGeometrySymbol(LeftParenthesisGeometry, origin, size, padding);
                    break;
                case Symbols.RightParenthesis:
                    PrintGeometrySymbol(RightParenthesisGeometry, origin, size, padding);
                    break;
                case Symbols.HorizontalLine:
                    PrintGeometrySymbol(HorizontalLineGeometry, origin, size, padding);
                    break;
            }
        }

        /// <summary></summary>
        protected virtual void PrintTextSymbol(string text, Point origin, Size size, Padding padding)
        {
            BrushSettings Brush = BrushSettings.Symbol;
            PrintableArea.Print(text, origin.X.Print + padding.Left.Print, origin.Y.Print, Brush);
        }

        /// <summary></summary>
        protected virtual void PrintGeometrySymbol(PrintableGeometry geometry, Point origin, Size size, Padding padding)
        {
            int X = origin.X.Print + padding.Left.Print;
            int Y = origin.Y.Print;
            int Width = size.Width.Print - padding.Left.Print - padding.Right.Print;
            int Height = size.Height.Print;

            char[] GeometryAtOrigin = geometry.Scale(Width, Height);
            BrushSettings Brush = BrushSettings.Symbol;

            if (geometry.Orientation == PrintOrientations.Horizontal)
            {
                for (int n = 0; n < GeometryAtOrigin.Length; n++)
                    PrintableArea.Print(GeometryAtOrigin[n], X + n, Y, Brush);
            }
            else
            {
                for (int n = 0; n < GeometryAtOrigin.Length; n++)
                    PrintableArea.Print(GeometryAtOrigin[n], X, Y + n, Brush);
            }
        }

        /// <summary>
        /// Prints the horizontal separator left of the specified origin and with the specified height.
        /// </summary>
        /// <param name="separator">The separator to print.</param>
        /// <param name="origin">The location where to print.</param>
        /// <param name="height">The separator height.</param>
        public virtual void PrintHorizontalSeparator(HorizontalSeparators separator, Point origin, Measure height)
        {
            BrushSettings Brush = BrushSettings.Symbol;

            switch (separator)
            {
                case HorizontalSeparators.Comma:
                    PrintableArea.Print(CommaSeparatorString, origin.X.Print - CommaSeparatorString.Length, origin.Y.Print, Brush);
                    break;
                case HorizontalSeparators.Dot:
                    PrintableArea.Print(DotSeparatorString, origin.X.Print - DotSeparatorString.Length, origin.Y.Print, Brush);
                    break;
            }
        }

        /// <summary>
        /// Prints the vertical separator above the specified origin and with the specified width.
        /// </summary>
        /// <param name="separator">The separator to print.</param>
        /// <param name="origin">The location where to print.</param>
        /// <param name="width">The separator width.</param>
        public virtual void PrintVerticalSeparator(VerticalSeparators separator, Point origin, Measure width)
        {
            // TODO
        }
        #endregion

        #region Implementation
        private PrintableArea PrintableArea;
        private PrintableGeometry LeftBracketGeometry;
        private PrintableGeometry RightBracketGeometry;
        private PrintableGeometry LeftCurlyBracketGeometry;
        private PrintableGeometry RightCurlyBracketGeometry;
        private PrintableGeometry LeftParenthesisGeometry;
        private PrintableGeometry RightParenthesisGeometry;
        private PrintableGeometry HorizontalLineGeometry;

        /// <summary>
        /// Returns a string representation of this instance.
        /// </summary>
        public override string ToString()
        {
            return PrintableArea.ToString();
        }

        /// <summary>
        /// Returns a formatted string representation of this instance.
        /// </summary>
        /// <param name="provider">A format provider.</param>
        public string ToString(IFormatProvider provider)
        {
            return PrintableArea.ToString(provider);
        }

        /// <summary>
        /// Returns a formatted string representation of this instance.
        /// </summary>
        /// <param name="format">A format.</param>
        /// <param name="formatProvider">A format provider.</param>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return PrintableArea.ToString(format, formatProvider);
        }
        #endregion
    }
}
