namespace EditorDebug
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Animation;
    using BaseNode;
    using EaslyController.Constants;
    using EaslyController.Layout;
    using static EaslyController.Constants.Margins;

    public class LayoutDrawContext : ILayoutDrawContext
    {
        public LayoutDrawContext(DrawingContext dc)
        {
            this.dc = dc;
            Typeface = new Typeface("Verdana");
            FontSize = 20;
            Foreground = Brushes.Black;

            FormattedText ft;

            ft = new FormattedText(" ", CultureInfo.CurrentCulture, FlowDirection.LeftToRight, Typeface, FontSize, Foreground);
            WhitespaceWidth = ft.WidthIncludingTrailingWhitespace;
            LineHeight = ft.Height;
            TabulationWidth = WhitespaceWidth * 3;
            VerticalSeparatorWidthTable[VerticalSeparators.Line] = LineHeight;

            ft = new FormattedText(", ", CultureInfo.CurrentCulture, FlowDirection.LeftToRight, Typeface, FontSize, Foreground);
            HorizontalSeparatorWidthTable[HorizontalSeparators.Comma] = ft.WidthIncludingTrailingWhitespace;

            ft = new FormattedText(DotText, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, Typeface, FontSize, Foreground);
            HorizontalSeparatorWidthTable[HorizontalSeparators.Dot] = ft.WidthIncludingTrailingWhitespace;

            LeftBracket = ScaleGlyphGeometry("[", true, 0.3, 0.3);
            RightBracket = ScaleGlyphGeometry("]", true, 0.3, 0.3);
            LeftCurlyBracket = ScaleGlyphGeometry("{", true, 0.25, 0.3);
            RightCurlyBracket = ScaleGlyphGeometry("}", true, 0.25, 0.3);
            LeftParenthesis = ScaleGlyphGeometry("(", true, 0, 0);
            RightParenthesis = ScaleGlyphGeometry(")", true, 0, 0);
            HorizontalLine = ScaleGlyphGeometryWidth("-", true);

            FlashAnimation = new DoubleAnimation(0, new Duration(TimeSpan.FromSeconds(1)));
            FlashAnimation.RepeatBehavior = RepeatBehavior.Forever;
            FlashAnimation.EasingFunction = new FlashEasingFunction();
            FlashClock = FlashAnimation.CreateClock();
        }

        public void UpdateDC(DrawingContext dc)
        {
            this.dc = dc;
        }

        protected ScalableGeometry ScaleGlyphGeometryWidth(string Text, bool IsWidthScaled)
        {
            FormattedText GlyphText = new FormattedText(Text, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, Typeface, FontSize, Foreground);
            GlyphText.Trimming = TextTrimming.None;

            Rect Bounds = new Rect(new Point(0, 0), new Size(GlyphText.Width, GlyphText.Height));
            Geometry GlyphGeometry = GlyphText.BuildGeometry(Bounds.Location);

            return new ScalableGeometry(GlyphGeometry, Bounds, IsWidthScaled, 0, 0, false, 0, 0);
        }

        protected ScalableGeometry ScaleGlyphGeometry(string Text, bool IsHeightScaled, double TopPercent, double BottomPercent)
        {
            FormattedText GlyphText = new FormattedText(Text, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, Typeface, FontSize, Foreground);
            GlyphText.Trimming = TextTrimming.None;

            Rect Bounds = new Rect(new Point(0, 0), new Size(GlyphText.Width, GlyphText.Height));
            Geometry GlyphGeometry = GlyphText.BuildGeometry(Bounds.Location);

            return new ScalableGeometry(GlyphGeometry, Bounds, false, 0, 0, IsHeightScaled, TopPercent, BottomPercent);
        }

        //IClassReplicate Node = null;
        private DrawingContext dc;
        public Typeface Typeface { get; }
        public double FontSize { get; }
        public Brush Foreground { get; }
        public double WhitespaceWidth { get; }
        public double LineHeight { get; }
        public double TabulationWidth { get; }
        private Dictionary<HorizontalSeparators, double> HorizontalSeparatorWidthTable = new Dictionary<HorizontalSeparators, double>()
        {
            {  HorizontalSeparators.None, 0},
            {  HorizontalSeparators.Comma, 0},
            {  HorizontalSeparators.Dot, 0},
        };
        private Dictionary<VerticalSeparators, double> VerticalSeparatorWidthTable = new Dictionary<VerticalSeparators, double>()
        {
            {  VerticalSeparators.None, 0},
            {  VerticalSeparators.Line, 30},
        };
        public static readonly string DotText = "·";
        ScalableGeometry LeftBracket;
        ScalableGeometry RightBracket;
        ScalableGeometry LeftCurlyBracket;
        ScalableGeometry RightCurlyBracket;
        ScalableGeometry LeftParenthesis;
        ScalableGeometry RightParenthesis;
        ScalableGeometry HorizontalLine;
        DoubleAnimation FlashAnimation;
        AnimationClock FlashClock;

        public double GetHorizontalSeparatorWidth(HorizontalSeparators separator)
        {
            return HorizontalSeparatorWidthTable[separator];
        }

        public double GetVerticalSeparatorHeight(VerticalSeparators separator)
        {
            return VerticalSeparatorWidthTable[separator];
        }

        public EaslyController.Controller.Size MeasureText(string text, TextStyles textStyle)
        {
            Brush Brush = StyleToBrush(textStyle);
            FormattedText ft = new FormattedText(text, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, Typeface, FontSize, Brush);

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
                    Text = DotText;
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
                case Symbols.HorizontalLine:
                    Text = "-";
                    break;
            }

            return Text;
        }

        public EaslyController.Controller.Size MeasureSymbol(Symbols symbol)
        {
            string Text = SymbolToText(symbol);
            FormattedText ft = new FormattedText(Text, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, Typeface, FontSize, Brushes.Blue);

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
                case Symbols.HorizontalLine:
                    return new EaslyController.Controller.Size(double.NaN, ft.Height);
            }
        }

        public void UpdatePadding(Margins leftMargin, Margins rightMargin, ref EaslyController.Controller.Size size, out EaslyController.Controller.Padding padding)
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

        private Brush StyleToBrush(TextStyles textStyle)
        {
            Color LightBlueColor = Color.FromArgb(0xFF, 0x2B, 0x91, 0xAF);

            switch (textStyle)
            {
                default:
                case TextStyles.Default:
                    return Foreground;
                case TextStyles.Character:
                    return Brushes.Orange;
                case TextStyles.Discrete:
                    return Brushes.DarkRed;
                case TextStyles.Keyword:
                    return Brushes.Blue;
                case TextStyles.Number:
                    return Brushes.Green;
                case TextStyles.Type:
                    return new SolidColorBrush(LightBlueColor);
            }
        }

        public void DrawText(string text, EaslyController.Controller.Point origin, TextStyles textStyle, bool isFocused)
        {
            if (isFocused)
                dc.PushOpacity(1, FlashClock);

            Brush Brush = StyleToBrush(textStyle);
            FormattedText ft = new FormattedText(text, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, Typeface, FontSize, Brush);
            dc.DrawText(ft, new Point(origin.X, origin.Y));

            if (isFocused)
                dc.Pop();
        }

        public void DrawSymbol(Symbols symbol, EaslyController.Controller.Point origin, EaslyController.Controller.Size size, EaslyController.Controller.Padding padding, bool isFocused)
        {
            if (isFocused)
                dc.PushOpacity(1, FlashClock);

            switch (symbol)
            {
                default:
                case Symbols.LeftArrow:
                case Symbols.Dot:
                    DrawTextSymbol(SymbolToText(symbol), origin, size, padding);
                    break;
                case Symbols.InsertSign:
                    if (isFocused)
                        DrawTextSymbol(SymbolToText(symbol), origin, size, padding);
                    break;
                case Symbols.LeftBracket:
                    DrawGeometrySymbol(LeftBracket, origin, size, padding);
                    break;
                case Symbols.RightBracket:
                    DrawGeometrySymbol(RightBracket, origin, size, padding);
                    break;
                case Symbols.LeftCurlyBracket:
                    DrawGeometrySymbol(LeftCurlyBracket, origin, size, padding);
                    break;
                case Symbols.RightCurlyBracket:
                    DrawGeometrySymbol(RightCurlyBracket, origin, size, padding);
                    break;
                case Symbols.LeftParenthesis:
                    DrawGeometrySymbol(LeftParenthesis, origin, size, padding);
                    break;
                case Symbols.RightParenthesis:
                    DrawGeometrySymbol(RightParenthesis, origin, size, padding);
                    break;
                case Symbols.HorizontalLine:
                    DrawGeometrySymbol(HorizontalLine, origin, size, padding);
                    break;
            }

            if (isFocused)
                dc.Pop();
        }

        public void DrawTextSymbol(string text, EaslyController.Controller.Point origin, EaslyController.Controller.Size size, EaslyController.Controller.Padding padding)
        {
            FormattedText ft = new FormattedText(text, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, Typeface, FontSize, Brushes.Blue);
            dc.DrawText(ft, new Point(origin.X + padding.Left, origin.Y + padding.Top));
        }

        public void DrawGeometrySymbol(ScalableGeometry geometry, EaslyController.Controller.Point origin, EaslyController.Controller.Size size, EaslyController.Controller.Padding padding)
        {
            Point PaddedOrigin = new Point(origin.X + padding.Left, origin.Y + padding.Top);
            Size PaddedSize = new Size(size.Width - padding.Left - padding.Right, size.Height - padding.Top - padding.Bottom);
            Geometry GeometryAtOrigin = MoveAndScaleGeometry(geometry, PaddedOrigin, GeometryScalings.None, GeometryScalings.Stretch, PaddedSize);

            dc.DrawGeometry(Brushes.Blue, null, GeometryAtOrigin);
        }

        private Geometry MoveAndScaleGeometry(ScalableGeometry geometry, Point origin, GeometryScalings widthScaling, GeometryScalings heightScaling, Size measuredSize)
        {
            double Width, Height;

            switch (widthScaling)
            {
                default:
                case GeometryScalings.None:
                    Width = double.NaN;
                    break;

                case GeometryScalings.Font:
                    Width = WhitespaceWidth;
                    break;

                case GeometryScalings.Stretch:
                    Width = measuredSize.Width;
                    break;
            }

            switch (heightScaling)
            {
                default:
                case GeometryScalings.None:
                    Height = double.NaN;
                    break;

                case GeometryScalings.Font:
                    Height = LineHeight;
                    break;

                case GeometryScalings.Stretch:
                    Height = measuredSize.Height;
                    break;
            }

            Geometry Result = geometry.Scaled(Width, Height);
            Result.Transform = new TranslateTransform(origin.X, origin.Y);

            Result.Freeze();

            return Result;
        }

        /// <summary>
        /// Draws the horizontal separator left of the specified origin and with the specified height.
        /// </summary>
        /// <param name="separator">The separator to draw.</param>
        /// <param name="origin">The location where to draw.</param>
        /// <param name="height">The separator height.</param>
        public void DrawHorizontalSeparator(HorizontalSeparators separator, EaslyController.Controller.Point origin, double height)
        {
            FormattedText ft;

            switch (separator)
            {
                case HorizontalSeparators.None:
                    break;
                case HorizontalSeparators.Dot:
                    ft = new FormattedText(DotText, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, Typeface, FontSize, Brushes.Blue);
                    dc.DrawText(ft, new Point(origin.X - ft.WidthIncludingTrailingWhitespace, origin.Y));
                    break;
                case HorizontalSeparators.Comma:
                    ft = new FormattedText(", ", CultureInfo.CurrentCulture, FlowDirection.LeftToRight, Typeface, FontSize, Brushes.Blue);
                    dc.DrawText(ft, new Point(origin.X - ft.WidthIncludingTrailingWhitespace, origin.Y));
                    break;
            }
        }

        /// <summary>
        /// Draws the vertical separator above the specified origin and with the specified width.
        /// </summary>
        /// <param name="separator">The separator to draw.</param>
        /// <param name="origin">The location where to draw.</param>
        /// <param name="width">The separator width.</param>
        public void DrawVerticalSeparator(VerticalSeparators separator, EaslyController.Controller.Point origin, double width)
        {
        }

        public void ShowCaret(EaslyController.Controller.Point origin, string text, TextStyles textStyle, CaretModes mode, int position)
        {
            Debug.Assert(position >= 0 && ((mode == CaretModes.Insertion && position <= text.Length) || (mode == CaretModes.Override && position < text.Length)));

            string LeftText = text.Substring(0, position);

            Brush Brush = StyleToBrush(textStyle);
            FormattedText ft = new FormattedText(LeftText, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, Typeface, FontSize, Brush);
            double X = origin.X + ft.WidthIncludingTrailingWhitespace;
            double Y = origin.Y;

            if (mode == CaretModes.Insertion)
            {
                Rect CaretRect = new Rect(X, Y, WhitespaceWidth / 4, LineHeight);

                dc.PushOpacity(1, FlashClock);
                dc.DrawRectangle(Brushes.Black, null, CaretRect);
                dc.Pop();
            }
            else
            {
                string CharText = text.Substring(position, 1);
                ft = new FormattedText(CharText, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, Typeface, FontSize, Brushes.DarkGray);

                Rect CaretRect = new Rect(X, Y, ft.WidthIncludingTrailingWhitespace, LineHeight);

                dc.PushOpacity(1, FlashClock);
                dc.DrawRectangle(Brushes.DarkGray, null, CaretRect);
                dc.Pop();
            }
        }

        public void HideCaret()
        {
        }
    }
}
