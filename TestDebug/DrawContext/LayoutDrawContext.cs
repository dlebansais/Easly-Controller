﻿using System.Diagnostics;
using EaslyController.Constants;
using EaslyController.Controller;
using EaslyController.Layout;

namespace TestDebug
{
    public class LayoutDrawContext : ILayoutDrawContext
    {
        public static LayoutDrawContext Default = new LayoutDrawContext();

        public double LineHeight { get { return 12; } }
        public double TabulationWidth { get { return 12; } }

        public double GetHorizontalSeparatorWidth(HorizontalSeparators separator)
        {
            return 0;
        }

        public double GetVerticalSeparatorHeight(VerticalSeparators separator)
        {
            return 0;
        }

        public Size MeasureSymbol(Symbols symbol)
        {
            switch (symbol)
            {
                default:
                case Symbols.LeftArrow:
                case Symbols.Dot:
                case Symbols.InsertSign:
                    return new Size(20, LineHeight);
                case Symbols.LeftBracket:
                case Symbols.RightBracket:
                case Symbols.LeftCurlyBracket:
                case Symbols.RightCurlyBracket:
                case Symbols.LeftParenthesis:
                case Symbols.RightParenthesis:
                    return new Size(20, double.NaN);
                case Symbols.HorizontalLine:
                    return new Size(double.NaN, 20);
            }
        }

        public Size MeasureText(string text, TextStyles textStyle, double maxTextWidth)
        {
            return new Size(text.Length * 20, LineHeight);
        }

        public void UpdatePadding(Margins leftMargin, Margins rightMargin, ref Size size, out Padding padding)
        {
            padding = Padding.Empty;
        }

        public void DrawText(string text, Point origin, TextStyles textStyle, bool isFocused)
        {
        }

        public void DrawSymbol(Symbols symbol, Point origin, Size size, Padding padding, bool isFocused)
        {
        }

        public void DrawHorizontalSeparator(HorizontalSeparators separator, Point origin, double height)
        {
        }

        public void DrawVerticalSeparator(VerticalSeparators separator, Point origin, double width)
        {
        }

        public void ShowCaret(Point origin, string text, TextStyles textStyle, CaretModes mode, int position)
        {
            Debug.Assert(position >= 0 && ((mode == CaretModes.Insertion && position <= text.Length) || (mode == CaretModes.Override && position < text.Length)));
        }

        public void HideCaret()
        {
        }

        public void DrawCommentIcon(Rect region)
        {
        }
    }
}
