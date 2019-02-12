namespace EditorDebug
{
    using System.Globalization;
    using System.Windows;
    using System.Windows.Media;
    using EaslyController.Constants;
    using EaslyController.Layout;
    using static EaslyController.Controller.Margins;

    public class LayoutDrawContext : ILayoutDrawContext
    {
        public LayoutDrawContext(DrawingContext dc)
        {
            this.dc = dc;
            Typeface = new Typeface("Verdana");
            FontSize = 20;
            Foreground = Brushes.Black;

            FormattedText ft = new FormattedText(" ", CultureInfo.CurrentCulture, FlowDirection.LeftToRight, Typeface, FontSize, Foreground);
            WhitespaceWidth = ft.WidthIncludingTrailingWhitespace;
            LineHeight = ft.Height;
            TabulationWidth = WhitespaceWidth * 2;
        }

        private DrawingContext dc;
        public Typeface Typeface { get; }
        public double FontSize { get; }
        public Brush Foreground { get; }
        public double WhitespaceWidth { get; }
        public double LineHeight { get; }
        public double TabulationWidth { get; }

        public EaslyController.Controller.Size MeasureText(string text)
        {
            FormattedText ft = new FormattedText(text, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, Typeface, FontSize, Foreground);

            return new EaslyController.Controller.Size(ft.Width, LineHeight);
        }

        private static string SymbolToText(Symbols symbol)
        {
            string Text;

            switch (symbol)
            {
                default:
                    Text = "";
                    break;
                case Symbols.LeftArrow:
                    Text = "←";
                    break;
                case Symbols.Dot:
                    Text = ".";
                    break;
                case Symbols.InsertSign:
                    Text = "◄";
                    break;
                case Symbols.LeftBracket:
                    Text = "[";
                    break;
                case Symbols.RightBracket:
                    Text = "]";
                    break;
                case Symbols.LeftCurlyBracket:
                    Text = "{";
                    break;
                case Symbols.RightCurlyBracket:
                    Text = "}";
                    break;
                case Symbols.LeftParenthesis:
                    Text = "(";
                    break;
                case Symbols.RightParenthesis:
                    Text = ")";
                    break;
            }

            return Text;
        }

        public EaslyController.Controller.Size MeasureSymbol(Symbols symbol)
        {
            string Text = SymbolToText(symbol);
            FormattedText ft = new FormattedText(Text, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, Typeface, FontSize, Foreground);

            switch (symbol)
            {
                default:
                case Symbols.LeftArrow:
                case Symbols.Dot:
                case Symbols.InsertSign:
                    return new EaslyController.Controller.Size(ft.Width, LineHeight);
                case Symbols.LeftBracket:
                case Symbols.RightBracket:
                case Symbols.LeftCurlyBracket:
                case Symbols.RightCurlyBracket:
                case Symbols.LeftParenthesis:
                case Symbols.RightParenthesis:
                    return new EaslyController.Controller.Size(ft.Width, double.NaN);
            }
        }

        public void UpdatePadding(EaslyController.Controller.Margins leftMargin, EaslyController.Controller.Margins rightMargin, ref EaslyController.Controller.Size size, out EaslyController.Controller.Padding padding)
        {
            double LeftPadding = 0;
            double RightPadding = 0;

            switch (leftMargin)
            {
                case None:
                    break;
                case ThinSpace:
                    LeftPadding = WhitespaceWidth / 2;
                    break;
                case Whitespace:
                    LeftPadding = WhitespaceWidth;
                    break;
            }

            switch (rightMargin)
            {
                case None:
                    break;
                case ThinSpace:
                    RightPadding = WhitespaceWidth / 2;
                    break;
                case Whitespace:
                    RightPadding = WhitespaceWidth;
                    break;
            }

            size = new EaslyController.Controller.Size(size.Width + LeftPadding + RightPadding, size.Height);
            padding = new EaslyController.Controller.Padding(LeftPadding, 0, RightPadding, 0);
        }

        public void DrawText(string text, EaslyController.Controller.Point origin)
        {
            FormattedText ft = new FormattedText(text, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, Typeface, FontSize, Foreground);
            dc.DrawText(ft, new Point(origin.X, origin.Y));
        }

        public void DrawSymbol(Symbols symbol, EaslyController.Controller.Point origin)
        {
            string Text = SymbolToText(symbol);
            FormattedText ft = new FormattedText(Text, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, Typeface, FontSize, Foreground);
            dc.DrawText(ft, new Point(origin.X, origin.Y));
        }

        public void UpdateDC(DrawingContext dc)
        {
            this.dc = dc;
        }
    }
}
