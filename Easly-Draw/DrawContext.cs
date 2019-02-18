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
    /// An implementation of IxxxDrawContext for WPF.
    /// </summary>
    public class DrawContext : ILayoutDrawContext
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="DrawContext"/> class.
        /// </summary>
        public DrawContext()
        {
            Typeface = new Typeface("Consolas");
            FontSize = 14;
            Culture = CultureInfo.CurrentCulture;
            FlowDirection = System.Windows.FlowDirection.LeftToRight;
            DotCharacter = '·';
            IsLastFocusedFullCell = false;

            BrushTable = new Dictionary<BrushSettings, Brush>
            {
                { BrushSettings.Default, Brushes.Black },
                { BrushSettings.Keyword, Brushes.Blue },
                { BrushSettings.Symbol, Brushes.Blue },
                { BrushSettings.Character, Brushes.Orange },
                { BrushSettings.Discrete, Brushes.DarkRed },
                { BrushSettings.Number, Brushes.Green },
                { BrushSettings.TypeIdentifier, new SolidColorBrush(Color.FromArgb(0xFF, 0x2B, 0x91, 0xAF)) },
                { BrushSettings.CaretInsertion, Brushes.Black },
                { BrushSettings.CaretOverride, Brushes.DarkGray }
            };

            FlashAnimation = new DoubleAnimation(0, new System.Windows.Duration(TimeSpan.FromSeconds(1)));
            FlashAnimation.RepeatBehavior = RepeatBehavior.Forever;
            FlashAnimation.EasingFunction = new FlashEasingFunction();
            FlashClock = FlashAnimation.CreateClock();

            Update();
        }
        #endregion

        #region Properties
        /// <summary>
        /// The font family, weight, style and stretch the text should be formatted with.
        /// </summary>
        public Typeface Typeface { get; set; }

        /// <summary>
        /// The font size.
        /// </summary>
        public double FontSize { get; set; }

        /// <summary>
        /// The specific culture of the text.
        /// </summary>
        public CultureInfo Culture { get; set; }

        /// <summary>
        /// The direction the text is read.
        /// </summary>
        public System.Windows.FlowDirection FlowDirection { get; set; }

        /// <summary>
        /// The character used as dot symbol.
        /// </summary>
        public char DotCharacter { get; set; }

        /// <summary>
        /// Table of brushes to use when drawing.
        /// </summary>
        public IDictionary<BrushSettings, Brush> BrushTable { get; }

        /// <summary>
        /// The WPF context used to draw.
        /// </summary>
        public DrawingContext WpfDrawingContext { get; private set; }
        #endregion

        #region Implementation of IxxxDrawContext
        /// <summary>
        /// Width of a tabulation margin.
        /// </summary>
        public double TabulationWidth { get; private set; }

        /// <summary>
        /// Height of a line of text.
        /// </summary>
        public double LineHeight { get; private set; }

        /// <summary>
        /// Gets the width corresponding to a separator between cells in a line.
        /// </summary>
        /// <param name="separator">The separator.</param>
        public virtual double GetHorizontalSeparatorWidth(HorizontalSeparators separator)
        {
            return HorizontalSeparatorWidthTable[separator];
        }

        /// <summary>
        /// Gets the height corresponding to a separator between cells in a column.
        /// </summary>
        /// <param name="separator">The separator.</param>
        public virtual double GetVerticalSeparatorHeight(VerticalSeparators separator)
        {
            return VerticalSeparatorWidthTable[separator];
        }

        /// <summary>
        /// Measures a string.
        /// </summary>
        /// <param name="text">The string to measure.</param>
        /// <param name="textStyle">Style to use for the text.</param>
        /// <returns>The size of the string.</returns>
        public virtual Size MeasureText(string text, TextStyles textStyle)
        {
            Brush Brush = StyleToBrush(textStyle);
            FormattedText ft = new FormattedText(text, Culture, FlowDirection, Typeface, FontSize, Brush);

            return new Size(ft.Width, LineHeight);
        }

        /// <summary></summary>
        protected virtual Brush StyleToBrush(TextStyles textStyle)
        {
            switch (textStyle)
            {
                default:
                case TextStyles.Default:
                    return BrushTable[BrushSettings.Default];
                case TextStyles.Character:
                    return BrushTable[BrushSettings.Character];
                case TextStyles.Discrete:
                    return BrushTable[BrushSettings.Discrete];
                case TextStyles.Keyword:
                    return BrushTable[BrushSettings.Keyword];
                case TextStyles.Number:
                    return BrushTable[BrushSettings.Number];
                case TextStyles.Type:
                    return BrushTable[BrushSettings.TypeIdentifier];
            }
        }

        /// <summary>
        /// Measures a symbol.
        /// </summary>
        /// <param name="symbol">The symbol to measure.</param>
        /// <returns>The size of the symbol.</returns>
        public virtual Size MeasureSymbol(Symbols symbol)
        {
            string Text = SymbolToText(symbol);
            FormattedText ft = new FormattedText(Text, Culture, FlowDirection, Typeface, FontSize, BrushTable[BrushSettings.Symbol]);

            switch (symbol)
            {
                default:
                case Symbols.LeftArrow:
                case Symbols.Dot:
                case Symbols.InsertSign:
                    return new Size(ft.Width, LineHeight);
                case Symbols.LeftBracket:
                case Symbols.RightBracket:
                case Symbols.LeftCurlyBracket:
                case Symbols.RightCurlyBracket:
                case Symbols.LeftParenthesis:
                case Symbols.RightParenthesis:
                    return new Size(ft.Width, double.NaN);
                case Symbols.HorizontalLine:
                    return new Size(double.NaN, ft.Height);
            }
        }

        /// <summary></summary>
        protected virtual string SymbolToText(Symbols symbol)
        {
            string Text = null;

            switch (symbol)
            {
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

            Debug.Assert(Text != null);
            return Text;
        }

        /// <summary>
        /// Extends a size according to the left and right margin settings.
        /// </summary>
        /// <param name="leftMargin">The left margin setting.</param>
        /// <param name="rightMargin">The right margin setting.</param>
        /// <param name="size">The size to extend with the calculated padding.</param>
        /// <param name="padding">The padding calculated from <paramref name="leftMargin"/> and <paramref name="rightMargin"/>.</param>
        public virtual void UpdatePadding(Margins leftMargin, Margins rightMargin, ref Size size, out Padding padding)
        {
            double LeftPadding = 0;
            double RightPadding = 0;

            switch (leftMargin)
            {
                case Margins.ThinSpace:
                    LeftPadding = WhitespaceWidth / 2;
                    break;
                case Margins.Whitespace:
                    LeftPadding = WhitespaceWidth;
                    break;
            }

            switch (rightMargin)
            {
                case Margins.ThinSpace:
                    RightPadding = WhitespaceWidth / 2;
                    break;
                case Margins.Whitespace:
                    RightPadding = WhitespaceWidth;
                    break;
            }

            size = new Size(size.Width + LeftPadding + RightPadding, size.Height);
            padding = new Padding(LeftPadding, 0, RightPadding, 0);
        }

        /// <summary>
        /// Draws a string, at the location specified in <paramref name="origin"/>.
        /// </summary>
        /// <param name="text">The text to draw.</param>
        /// <param name="origin">The location where to start drawing.</param>
        /// <param name="textStyle">Style to use for the text.</param>
        /// <param name="isFocused">true if the whole text has the focus.</param>
        public virtual void DrawText(string text, Point origin, TextStyles textStyle, bool isFocused)
        {
            Debug.Assert(WpfDrawingContext != null);

            if (isFocused)
            {
                ChangeFlashClockOpacity(isVisible: true);
                WpfDrawingContext.PushOpacity(1, FlashClock);
            }

            Brush Brush = StyleToBrush(textStyle);
            FormattedText ft = new FormattedText(text, Culture, FlowDirection, Typeface, FontSize, Brush);
            WpfDrawingContext.DrawText(ft, new System.Windows.Point(origin.X, origin.Y));

            if (isFocused)
            {
                WpfDrawingContext.Pop();
                IsLastFocusedFullCell = true;
            }
        }

        /// <summary>
        /// Draws a symbol, at the location specified in <paramref name="origin"/>.
        /// </summary>
        /// <param name="symbol">The symbol to draw.</param>
        /// <param name="origin">The location where to start drawing.</param>
        /// <param name="size">The drawing size, padding included.</param>
        /// <param name="padding">The padding to use when drawing.</param>
        /// <param name="isFocused">true if the symbol text has the focus.</param>
        public virtual void DrawSymbol(Symbols symbol, Point origin, Size size, Padding padding, bool isFocused)
        {
            Debug.Assert(WpfDrawingContext != null);

            if (isFocused)
            {
                ChangeFlashClockOpacity(isVisible: true);
                WpfDrawingContext.PushOpacity(1, FlashClock);
            }

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
                    DrawGeometrySymbol(LeftBracketGeometry, origin, size, padding);
                    break;
                case Symbols.RightBracket:
                    DrawGeometrySymbol(RightBracketGeometry, origin, size, padding);
                    break;
                case Symbols.LeftCurlyBracket:
                    DrawGeometrySymbol(LeftCurlyBracketGeometry, origin, size, padding);
                    break;
                case Symbols.RightCurlyBracket:
                    DrawGeometrySymbol(RightCurlyBracketGeometry, origin, size, padding);
                    break;
                case Symbols.LeftParenthesis:
                    DrawGeometrySymbol(LeftParenthesisGeometry, origin, size, padding);
                    break;
                case Symbols.RightParenthesis:
                    DrawGeometrySymbol(RightParenthesisGeometry, origin, size, padding);
                    break;
                case Symbols.HorizontalLine:
                    DrawGeometrySymbol(HorizontalLineGeometry, origin, size, padding);
                    break;
            }

            if (isFocused)
            {
                WpfDrawingContext.Pop();
                IsLastFocusedFullCell = true;
            }
        }

        /// <summary></summary>
        protected virtual void DrawTextSymbol(string text, Point origin, Size size, Padding padding)
        {
            Debug.Assert(WpfDrawingContext != null);

            FormattedText ft = new FormattedText(text, Culture, FlowDirection, Typeface, FontSize, BrushTable[BrushSettings.Symbol]);
            WpfDrawingContext.DrawText(ft, new System.Windows.Point(origin.X + padding.Left, origin.Y + padding.Top));
        }

        /// <summary></summary>
        protected virtual void DrawGeometrySymbol(ScalableGeometry geometry, Point origin, Size size, Padding padding)
        {
            Debug.Assert(WpfDrawingContext != null);

            Point PaddedOrigin = origin.Moved(padding.Left, padding.Top);
            Size PaddedSize = new Size(size.Width - padding.Left - padding.Right, size.Height - padding.Top - padding.Bottom);
            Geometry GeometryAtOrigin = MoveAndScaleGeometry(geometry, PaddedOrigin, GeometryScalings.None, GeometryScalings.Stretch, PaddedSize);

            WpfDrawingContext.DrawGeometry(Brushes.Blue, null, GeometryAtOrigin);
        }

        /// <summary></summary>
        protected virtual Geometry MoveAndScaleGeometry(ScalableGeometry geometry, Point origin, GeometryScalings widthScaling, GeometryScalings heightScaling, Size measuredSize)
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
        public virtual void DrawHorizontalSeparator(HorizontalSeparators separator, Point origin, double height)
        {
            Debug.Assert(WpfDrawingContext != null);

            FormattedText ft;

            switch (separator)
            {
                case HorizontalSeparators.Dot:
                    ft = new FormattedText(DotText, Culture, FlowDirection, Typeface, FontSize, BrushTable[BrushSettings.Symbol]);
                    WpfDrawingContext.DrawText(ft, new System.Windows.Point(origin.X - ft.WidthIncludingTrailingWhitespace, origin.Y));
                    break;
                case HorizontalSeparators.Comma:
                    ft = new FormattedText(", ", Culture, FlowDirection, Typeface, FontSize, BrushTable[BrushSettings.Symbol]);
                    WpfDrawingContext.DrawText(ft, new System.Windows.Point(origin.X - ft.WidthIncludingTrailingWhitespace, origin.Y));
                    break;
            }
        }

        /// <summary>
        /// Draws the vertical separator above the specified origin and with the specified width.
        /// </summary>
        /// <param name="separator">The separator to draw.</param>
        /// <param name="origin">The location where to draw.</param>
        /// <param name="width">The separator width.</param>
        public virtual void DrawVerticalSeparator(VerticalSeparators separator, Point origin, double width)
        {
            // TODO
        }

        /// <summary>
        /// Shows the caret.
        /// </summary>
        /// <param name="origin">Location of the cell with the caret.</param>
        /// <param name="text">The full cell text.</param>
        /// <param name="textStyle">The text style.</param>
        /// <param name="mode">The caret mode.</param>
        /// <param name="position">The position of the caret in <paramref name="text"/>.</param>
        public virtual void ShowCaret(Point origin, string text, TextStyles textStyle, CaretModes mode, int position)
        {
            Debug.Assert(position >= 0 && ((mode == CaretModes.Insertion && position <= text.Length) || (mode == CaretModes.Override && position < text.Length)));
            Debug.Assert(WpfDrawingContext != null);

            string LeftText = text.Substring(0, position);

            Brush Brush = StyleToBrush(textStyle);
            FormattedText ft = new FormattedText(LeftText, Culture, FlowDirection, Typeface, FontSize, Brush);
            double X = origin.X + ft.WidthIncludingTrailingWhitespace;
            double Y = origin.Y;

            ChangeFlashClockOpacity(isVisible: true);

            if (mode == CaretModes.Insertion)
            {
                System.Windows.Rect CaretRect = new System.Windows.Rect(X, Y, WhitespaceWidth / 4, LineHeight);

                WpfDrawingContext.PushOpacity(1, FlashClock);
                WpfDrawingContext.DrawRectangle(BrushTable[BrushSettings.CaretInsertion], null, CaretRect);
                WpfDrawingContext.Pop();
            }
            else
            {
                string CharText = text.Substring(position, 1);
                ft = new FormattedText(CharText, Culture, FlowDirection, Typeface, FontSize, BrushTable[BrushSettings.CaretOverride]);

                System.Windows.Rect CaretRect = new System.Windows.Rect(X, Y, ft.WidthIncludingTrailingWhitespace, LineHeight);

                WpfDrawingContext.PushOpacity(1, FlashClock);
                WpfDrawingContext.DrawRectangle(BrushTable[BrushSettings.CaretOverride], null, CaretRect);
                WpfDrawingContext.Pop();
            }

            IsLastFocusedFullCell = false;
        }

        /// <summary>
        /// Hides the caret.
        /// </summary>
        public virtual void HideCaret()
        {
            ChangeFlashClockOpacity(isVisible: false);
        }

        /// <summary></summary>
        protected virtual void ChangeFlashClockOpacity(bool isVisible)
        {
            FlashEasingFunction EasingFunction = FlashAnimation.EasingFunction as FlashEasingFunction;
            EasingFunction.SetIsVisible(isVisible, IsLastFocusedFullCell);
        }
        #endregion

        #region Client Interface
        /// <summary>
        /// Updates the drawing context.
        /// </summary>
        /// <param name="wpfDrawingContext">The new instance of <see cref="DrawingContext"/>.</param>
        public virtual void SetWpfDrawingContext(DrawingContext wpfDrawingContext)
        {
            WpfDrawingContext = wpfDrawingContext;
        }

        /// <summary>
        /// Recalculate internal constants.
        /// To call after a property was changed.
        /// </summary>
        public virtual void Update()
        {
            FormattedText ft;

            ft = new FormattedText(" ", Culture, FlowDirection, Typeface, FontSize, BrushTable[BrushSettings.Default]);
            WhitespaceWidth = ft.WidthIncludingTrailingWhitespace;
            LineHeight = ft.Height;
            TabulationWidth = WhitespaceWidth * 3;
            VerticalSeparatorWidthTable[VerticalSeparators.Line] = LineHeight;

            ft = new FormattedText(", ", Culture, FlowDirection, Typeface, FontSize, BrushTable[BrushSettings.Symbol]);
            HorizontalSeparatorWidthTable[HorizontalSeparators.Comma] = ft.WidthIncludingTrailingWhitespace;

            DotText = DotCharacter.ToString();
            ft = new FormattedText(DotText, Culture, FlowDirection, Typeface, FontSize, BrushTable[BrushSettings.Symbol]);
            HorizontalSeparatorWidthTable[HorizontalSeparators.Dot] = ft.WidthIncludingTrailingWhitespace;

            LeftBracketGeometry = ScaleGlyphGeometryHeight("[", true, 0.3, 0.3);
            RightBracketGeometry = ScaleGlyphGeometryHeight("]", true, 0.3, 0.3);
            LeftCurlyBracketGeometry = ScaleGlyphGeometryHeight("{", true, 0.25, 0.3);
            RightCurlyBracketGeometry = ScaleGlyphGeometryHeight("}", true, 0.25, 0.3);
            LeftParenthesisGeometry = ScaleGlyphGeometryHeight("(", true, 0, 0);
            RightParenthesisGeometry = ScaleGlyphGeometryHeight(")", true, 0, 0);
            HorizontalLineGeometry = ScaleGlyphGeometryWidth("-", true, 0, 0);
        }
        #endregion

        #region Implementation
        /// <summary></summary>
        protected virtual ScalableGeometry ScaleGlyphGeometryWidth(string text, bool isWidthScaled, double leftPercent, double rightPercent)
        {
            FormattedText GlyphText = new FormattedText(text, Culture, FlowDirection, Typeface, FontSize, BrushTable[BrushSettings.Symbol]);
            GlyphText.Trimming = System.Windows.TextTrimming.None;

            System.Windows.Rect Bounds = new System.Windows.Rect(new System.Windows.Point(0, 0), new System.Windows.Size(GlyphText.Width, GlyphText.Width));
            Geometry GlyphGeometry = GlyphText.BuildGeometry(Bounds.Location);

            return new ScalableGeometry(GlyphGeometry, Bounds, isWidthScaled, leftPercent, rightPercent, false, 0, 0);
        }

        /// <summary></summary>
        protected virtual ScalableGeometry ScaleGlyphGeometryHeight(string text, bool isHeightScaled, double topPercent, double bottomPercent)
        {
            FormattedText GlyphText = new FormattedText(text, Culture, FlowDirection, Typeface, FontSize, BrushTable[BrushSettings.Symbol]);
            GlyphText.Trimming = System.Windows.TextTrimming.None;

            System.Windows.Rect Bounds = new System.Windows.Rect(new System.Windows.Point(0, 0), new System.Windows.Size(GlyphText.Width, GlyphText.Height));
            Geometry GlyphGeometry = GlyphText.BuildGeometry(Bounds.Location);

            return new ScalableGeometry(GlyphGeometry, Bounds, false, 0, 0, isHeightScaled, topPercent, bottomPercent);
        }

        private double WhitespaceWidth;
        private Dictionary<HorizontalSeparators, double> HorizontalSeparatorWidthTable = new Dictionary<HorizontalSeparators, double>()
        {
            { HorizontalSeparators.None, 0 },
            { HorizontalSeparators.Comma, 0 },
            { HorizontalSeparators.Dot, 0 },
        };
        private Dictionary<VerticalSeparators, double> VerticalSeparatorWidthTable = new Dictionary<VerticalSeparators, double>()
        {
            { VerticalSeparators.None, 0 },
            { VerticalSeparators.Line, 30 },
        };
        private string DotText;
        private ScalableGeometry LeftBracketGeometry;
        private ScalableGeometry RightBracketGeometry;
        private ScalableGeometry LeftCurlyBracketGeometry;
        private ScalableGeometry RightCurlyBracketGeometry;
        private ScalableGeometry LeftParenthesisGeometry;
        private ScalableGeometry RightParenthesisGeometry;
        private ScalableGeometry HorizontalLineGeometry;
        private DoubleAnimation FlashAnimation;
        private AnimationClock FlashClock;
        private bool IsLastFocusedFullCell;
        #endregion
    }
}
