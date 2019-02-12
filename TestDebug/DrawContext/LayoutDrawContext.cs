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
            }
        }

        public Size MeasureText(string text)
        {
            return new Size(text.Length * 20, LineHeight);
        }

        public void UpdatePadding(Margins leftMargin, Margins rightMargin, ref Size size, out Padding padding)
        {
            padding = Padding.Empty;
        }

        public void DrawText(string text, Point origin)
        {
        }

        public void DrawSymbol(Symbols symbol, Point origin)
        {
        }
    }
}
