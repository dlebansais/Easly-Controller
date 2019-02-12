using EaslyController.Constants;
using EaslyController.Controller;
using EaslyController.Layout;
using NodeController;

namespace TestDebug
{
    public class LayoutDrawContext : ILayoutDrawContext
    {
        public static LayoutDrawContext Default = new LayoutDrawContext();

        public double LineHeight { get { return 12; } }

        public Size MeasureSymbol(Symbols symbol)
        {
            switch (symbol)
            {
                default:
                case Symbols.LeftArrow:
                case Symbols.Dot:
                    return new Size(20, LineHeight);
                case Symbols.LeftBracket:
                case Symbols.RightBracket:
                case Symbols.LeftCurlyBracket:
                case Symbols.RightCurlyBracket:
                case Symbols.LeftParenthesis:
                case Symbols.RightParenthesis:
                    return new Size(20, LineHeight);
                    //return new Size(20, double.NaN);
            }
        }

        public Size MeasureText(string text)
        {
            return new Size(text.Length * 20, LineHeight);
        }

        public Size MarginExtended(Size size, Margins leftMargin, Margins rightMargin)
        {
            return size;
        }
    }
}
