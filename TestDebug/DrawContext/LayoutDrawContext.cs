using System.Diagnostics;
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
        public int TabulationLength { get { return 1; } }
        
        public double GetHorizontalSeparatorWidth(HorizontalSeparators separator)
        {
            return 0;
        }

        public int GetHorizontalSeparatorLength(HorizontalSeparators separator)
        {
            return 0;
        }

        public SeparatorLength GetVerticalSeparatorHeight(VerticalSeparators separator)
        {
            return default(SeparatorLength);
        }

        public int GetVerticalSeparatorLineCount(VerticalSeparators separator)
        {
            return 0;
        }

        public Size MeasureSymbolSize(Symbols symbol)
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

        public Plane MeasureSymbolPlane(Symbols symbol)
        {
            switch (symbol)
            {
                default:
                case Symbols.LeftArrow:
                case Symbols.Dot:
                case Symbols.InsertSign:
                    return new Plane(1, 1);
                case Symbols.LeftBracket:
                case Symbols.RightBracket:
                case Symbols.LeftCurlyBracket:
                case Symbols.RightCurlyBracket:
                case Symbols.LeftParenthesis:
                case Symbols.RightParenthesis:
                    return new Plane(1, -1);
                case Symbols.HorizontalLine:
                    return new Plane(-1, 1);
            }
        }

        public Size MeasureTextSize(string text, TextStyles textStyle, double maxTextWidth)
        {
            return new Size(text.Length * 20, LineHeight);
        }

        public Plane MeasureTextPlane(string text, TextStyles textStyle, int maxTextLength)
        {
            return new Plane(text.Length, 1);
        }

        public void UpdatePadding(Margins leftMargin, Margins rightMargin, ref Size size, out Padding padding)
        {
            padding = Padding.Empty;
        }

        public virtual void UpdatePadding(Margins leftMargin, Margins rightMargin, ref Plane plane, out SpacePadding padding)
        {
            padding = SpacePadding.Empty;
        }

        public virtual void DrawSelectionText(string text, Point origin, TextStyles textStyle, int start, int end)
        {
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

        public virtual int GetCaretPositionInText(Point origin, string text, TextStyles textStyle, CaretModes mode, double maxTextWidth)
        {
            return 0;
        }

        public virtual Point ToRelativeLocation(Point origin)
        {
            return origin;
        }

        public virtual void DrawSelectionRectangle(Rect rect)
        {
        }
    }
}
