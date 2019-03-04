namespace EaslyDraw
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Windows.Media;
    using System.Windows.Media.Animation;
    using EaslyController.Constants;
    using EaslyController.Controller;
    using EaslyController.Layout;

    /// <summary>
    /// An implementation of IxxxPrintContext for WPF.
    /// </summary>
    public class PrintContext : MeasureContext, ILayoutPrintContext
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
            PrintArea = new PrintableCharacter[0, 0];
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
        /// Prints a string, at the location specified in <paramref name="corner"/>.
        /// </summary>
        /// <param name="text">The text to print.</param>
        /// <param name="corner">The location where to start printing.</param>
        /// <param name="textStyle">Style to use for the text.</param>
        public virtual void PrintText(string text, Corner corner, TextStyles textStyle)
        {
            BrushSettings Brush = StyleToForegroundBrush(textStyle);
            PrintTextOnArea(text, corner.X, corner.Y, Brush);
        }

        /// <summary></summary>
        protected virtual void PrintTextOnArea(string text, int x, int y, BrushSettings brush)
        {
            int CX = PrintArea.GetLength(0);
            int CY = PrintArea.GetLength(1);

            int MaxCX = x + text.Length > CX ? x + text.Length : CX;
            int MaxCY = y + 1 > CY ? y + 1 : CY;

            if (MaxCX > CX || MaxCY > CY)
            {
                PrintableCharacter[,] NewPrintArea = new PrintableCharacter[MaxCX, MaxCY];
                for (int i = 0; i < CX; i++)
                    for (int j = 0; j < CY; j++)
                        NewPrintArea[i, j] = PrintArea[i, j];

                PrintArea = NewPrintArea;
            }

            for (int n = 0; n < text.Length; n++)
                PrintArea[x + n, y] = new PrintableCharacter(text[n], brush);
        }

        /// <summary>
        /// Prints a symbol, at the location specified in <paramref name="corner"/>.
        /// </summary>
        /// <param name="symbol">The symbol to print.</param>
        /// <param name="corner">The location where to start printing.</param>
        /// <param name="plane">The printing size, padding included.</param>
        /// <param name="padding">The padding to use when printing.</param>
        public virtual void PrintSymbol(Symbols symbol, Corner corner, Plane plane, SpacePadding padding)
        {
            switch (symbol)
            {
                default:
                case Symbols.LeftArrow:
                case Symbols.Dot:
                    PrintTextSymbol(SymbolToText(symbol), corner, plane, padding);
                    break;
                case Symbols.InsertSign:
                    break;
                case Symbols.LeftBracket:
                    PrintGeometrySymbol(LeftBracketGeometry, corner, plane, padding);
                    break;
                case Symbols.RightBracket:
                    PrintGeometrySymbol(RightBracketGeometry, corner, plane, padding);
                    break;
                case Symbols.LeftCurlyBracket:
                    PrintGeometrySymbol(LeftCurlyBracketGeometry, corner, plane, padding);
                    break;
                case Symbols.RightCurlyBracket:
                    PrintGeometrySymbol(RightCurlyBracketGeometry, corner, plane, padding);
                    break;
                case Symbols.LeftParenthesis:
                    PrintGeometrySymbol(LeftParenthesisGeometry, corner, plane, padding);
                    break;
                case Symbols.RightParenthesis:
                    PrintGeometrySymbol(RightParenthesisGeometry, corner, plane, padding);
                    break;
                case Symbols.HorizontalLine:
                    PrintGeometrySymbol(HorizontalLineGeometry, corner, plane, padding);
                    break;
            }
        }

        /// <summary></summary>
        protected virtual void PrintTextSymbol(string text, Corner corner, Plane plane, SpacePadding padding)
        {
            BrushSettings Brush = BrushSettings.Symbol;
            PrintTextOnArea(text, corner.X + padding.Left, corner.Y, Brush);
        }

        /// <summary></summary>
        protected virtual void PrintGeometrySymbol(PrintableGeometry geometry, Corner corner, Plane plane, SpacePadding padding)
        {
            int X = corner.X + padding.Left;
            int Y = corner.Y;
            int Width = plane.Width - padding.Left - padding.Right;
            int Height = plane.Height;

            char[] GeometryAtOrigin = geometry.Scale(Width, Height);
            BrushSettings Brush = BrushSettings.Symbol;

            if (geometry.Orientation == PrintOrientations.Horizontal)
            {
                for (int n = 0; n < GeometryAtOrigin.Length; n++)
                    PrintArea[X + n, Y] = new PrintableCharacter(GeometryAtOrigin[n], Brush);
            }
            else
            {
                for (int n = 0; n < GeometryAtOrigin.Length; n++)
                    PrintArea[X, Y + n] = new PrintableCharacter(GeometryAtOrigin[n], Brush);
            }
        }

        /// <summary>
        /// Prints the horizontal separator left of the specified origin and with the specified height.
        /// </summary>
        /// <param name="separator">The separator to print.</param>
        /// <param name="corner">The location where to print.</param>
        /// <param name="height">The separator height.</param>
        public virtual void PrintHorizontalSeparator(HorizontalSeparators separator, Corner corner, int height)
        {
            BrushSettings Brush = BrushSettings.Symbol;

            switch (separator)
            {
                case HorizontalSeparators.Comma:
                    PrintTextOnArea(CommaSeparatorString, corner.X - CommaSeparatorString.Length, corner.Y, Brush);
                    break;
                case HorizontalSeparators.Dot:
                    PrintTextOnArea(DotSeparatorString, corner.X - DotSeparatorString.Length, corner.Y, Brush);
                    break;
            }
        }

        /// <summary>
        /// Prints the vertical separator above the specified origin and with the specified width.
        /// </summary>
        /// <param name="separator">The separator to print.</param>
        /// <param name="corner">The location where to print.</param>
        /// <param name="width">The separator width.</param>
        public virtual void PrintVerticalSeparator(VerticalSeparators separator, Corner corner, int width)
        {
            // TODO
        }
        #endregion

        #region Implementation
        private PrintableCharacter[,] PrintArea;
        private PrintableGeometry LeftBracketGeometry;
        private PrintableGeometry RightBracketGeometry;
        private PrintableGeometry LeftCurlyBracketGeometry;
        private PrintableGeometry RightCurlyBracketGeometry;
        private PrintableGeometry LeftParenthesisGeometry;
        private PrintableGeometry RightParenthesisGeometry;
        private PrintableGeometry HorizontalLineGeometry;
        #endregion
    }
}
