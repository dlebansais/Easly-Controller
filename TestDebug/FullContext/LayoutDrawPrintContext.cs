using System.Diagnostics;
using EaslyController.Constants;
using EaslyController.Controller;
using EaslyController.Layout;

namespace TestDebug
{
    public class LayoutDrawPrintContext : ILayoutDrawContext, ILayoutPrintContext
    {
        public static LayoutDrawPrintContext Default = new LayoutDrawPrintContext();

        public Measure LineHeight { get { return new Measure() { Draw = 12, Print = 1 }; } }
        public Measure TabulationWidth { get { return new Measure() { Draw = 24, Print = 2 }; } }
        public Measure BlockGeometryWidth { get { return new Measure() { Draw = 12, Print = 1 }; } }
        public Measure BlockGeometryHeight { get { return new Measure() { Draw = 12, Print = 1 }; } }
        public Padding PagePadding { get; private set; }

        public Measure GetHorizontalSeparatorWidth(HorizontalSeparators separator)
        {
            return Measure.Zero;
        }

        public Measure GetVerticalSeparatorHeight(VerticalSeparators separator)
        {
            return Measure.Zero;
        }

        public virtual void DrawHorizontalBlockGeometry(Point origin, Measure width)
        {
        }

        public virtual void DrawVerticalBlockGeometry(Point origin, Measure height)
        {
        }

        public Size MeasureSymbolSize(Symbols symbol)
        {
            switch (symbol)
            {
                default:
                case Symbols.LeftArrow:
                case Symbols.Dot:
                case Symbols.InsertSign:
                    return new Size(new Measure() { Draw = 20 }, LineHeight);
                case Symbols.LeftBracket:
                case Symbols.RightBracket:
                case Symbols.LeftCurlyBracket:
                case Symbols.RightCurlyBracket:
                case Symbols.LeftParenthesis:
                case Symbols.RightParenthesis:
                    return new Size(new Measure() { Draw = 20 }, Measure.Floating);
                case Symbols.HorizontalLine:
                    return new Size(Measure.Floating, new Measure() { Draw = 20 });
            }
        }

        public Size MeasureText(string text, TextStyles textStyle, Measure maxTextWidth)
        {
            return new Size(new Measure() { Draw = text.Length * 20, Print = text.Length }, LineHeight);
        }

        public Size MeasureNumber(string text)
        {
            return new Size(new Measure() { Draw = text.Length * 20, Print = text.Length }, LineHeight);
        }

        public void UpdatePadding(Margins leftMargin, Margins rightMargin, ref Size size, out Padding padding)
        {
            padding = Padding.Empty;
        }

        public void DrawSelectionText(string text, Point origin, TextStyles textStyle, int start, int end)
        {
        }

        public virtual void DrawTextBackground(string text, Point origin, TextStyles textStyle)
        {
        }

        public void DrawText(string text, Point origin, TextStyles textStyle, bool isFocused)
        {
        }

        public void DrawNumber(string text, Point origin)
        {
        }

        public void DrawSymbol(Symbols symbol, Point origin, Size size, Padding padding, bool isFocused)
        {
        }

        public void DrawHorizontalSeparator(HorizontalSeparators separator, Point origin, Measure height)
        {
        }

        public void DrawVerticalSeparator(VerticalSeparators separator, Point origin, Measure width)
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

        public int GetCaretPositionInText(double x, string text, TextStyles textStyle, CaretModes mode, Measure maxTextWidth)
        {
            return 0;
        }

        public virtual void FromRelativeLocation(ref double x, ref double y)
        {
        }

        public void ToRelativeLocation(ref double x, ref double y)
        {
        }

        public void DrawSelectionRectangle(Rect rect, SelectionStyles selectionStyle)
        {
        }

        public void PrintText(string text, Point origin, TextStyles textStyle)
        {
        }

        public void PrintNumber(string text, Point origin)
        {
        }

        public void PrintSymbol(Symbols symbol, Point origin, Size size, Padding padding)
        {
        }

        public void PrintHorizontalSeparator(HorizontalSeparators separator, Point origin, Measure height)
        {
        }

        public void PrintVerticalSeparator(VerticalSeparators separator, Point origin, Measure width)
        {
        }
    }
}
