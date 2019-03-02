namespace EaslyDraw
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Windows.Interop;
    using System.Windows.Media;
    using System.Windows.Media.Animation;
    using System.Windows.Media.Imaging;
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
            FontSize = 10;
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
                { BrushSettings.CommentBackground, Brushes.LightGreen },
                { BrushSettings.CommentForeground, Brushes.Black },
                { BrushSettings.CaretInsertion, Brushes.Black },
                { BrushSettings.CaretOverride, Brushes.DarkGray },
                { BrushSettings.Selection, new SolidColorBrush(Color.FromArgb(0xFF, 0x99, 0xC9, 0xEF)) },
            };

            FlashAnimation = new DoubleAnimation(0, new System.Windows.Duration(TimeSpan.FromSeconds(1)));
            FlashAnimation.RepeatBehavior = RepeatBehavior.Forever;
            FlashAnimation.EasingFunction = new FlashEasingFunction();
            FlashClock = FlashAnimation.CreateClock();
            CommentIcon = LoadPngResource("Comment");

            Update();
        }

        private BitmapSource LoadPngResource(string resourceName)
        {
            Assembly CurrentAssembly = Assembly.GetExecutingAssembly();
            string ResourcePath = $"EaslyDraw.Resources.{resourceName}.png";

            using (Stream ResourceStream = CurrentAssembly.GetManifestResourceStream(ResourcePath))
            {
                System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(ResourceStream);
                return BitmapToBitmapSource(bmp);
            }
        }

        [DllImport("gdi32")]
        private static extern int DeleteObject(IntPtr o);

        private BitmapSource BitmapToBitmapSource(System.Drawing.Bitmap bmp)
        {
            IntPtr ip = bmp.GetHbitmap();
            try
            {
                return Imaging.CreateBitmapSourceFromHBitmap(ip, IntPtr.Zero, System.Windows.Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                DeleteObject(ip);
            }
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
        /// The em size.
        /// </summary>
        public double EmSize { get { return FontSize * 128.0 / 96.0; } }

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

        /// <summary>
        /// The icon to use to signal a comment.
        /// </summary>
        public BitmapSource CommentIcon { get; set; }

        /// <summary>
        /// The padding margin applied to comment text.
        /// </summary>
        public Padding CommentPadding { get; private set; }

        /// <summary>
        /// The padding applied to the entire page.
        /// </summary>
        public Padding PagePadding { get; private set; }

        /// <summary>
        /// The insertion caret width.
        /// </summary>
        public double InsertionCaretWidth { get; private set; }
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
        /// <param name="maxTextWidth">The maximum width for a line of text. NaN means no limit.</param>
        /// <returns>The size of the string.</returns>
        public virtual Size MeasureText(string text, TextStyles textStyle, double maxTextWidth)
        {
            Brush Brush = StyleToForegroundBrush(textStyle);
            FormattedText ft = new FormattedText(text, Culture, FlowDirection, Typeface, EmSize, Brush);
            if (!double.IsNaN(maxTextWidth))
                ft.MaxTextWidth = maxTextWidth;

            double Width = ft.Width;
            double Height = LineHeight;

            if (textStyle == TextStyles.Comment)
                return new Size(CommentPadding.Left + Width + CommentPadding.Right, CommentPadding.Top + Height + CommentPadding.Bottom);
            else
                return new Size(Width, Height);
        }

        /// <summary></summary>
        protected virtual Brush StyleToForegroundBrush(TextStyles textStyle)
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
                case TextStyles.Comment:
                    return BrushTable[BrushSettings.CommentForeground];
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
            FormattedText ft = new FormattedText(Text, Culture, FlowDirection, Typeface, EmSize, BrushTable[BrushSettings.Symbol]);

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
        /// Draws the background of a selected text.
        /// </summary>
        /// <param name="text">The text</param>
        /// <param name="origin">The location where to start drawing.</param>
        /// <param name="textStyle">The style used to measure selected text.</param>
        /// <param name="start">The starting point of the selection.</param>
        /// <param name="end">The ending point of the selection.</param>
        public virtual void DrawSelectionText(string text, Point origin, TextStyles textStyle, int start, int end)
        {
            Debug.Assert(WpfDrawingContext != null);
            Debug.Assert(start >= 0 && start <= text.Length);
            Debug.Assert(end >= 0 && end <= text.Length);

            if (start > end)
            {
                int n = end;
                end = start;
                start = n;
            }

            Brush Brush = StyleToForegroundBrush(textStyle);
            FormattedText ft;
            double X = PagePadding.Left + origin.X;
            double Y = PagePadding.Top + origin.Y;

            if (textStyle == TextStyles.Comment)
            {
                X += CommentPadding.Left;
                Y += CommentPadding.Top;
            }

            ft = new FormattedText(text.Substring(0, start), Culture, FlowDirection, Typeface, EmSize, Brush);
            X += ft.WidthIncludingTrailingWhitespace;

            ft = new FormattedText(text.Substring(start, end - start), Culture, FlowDirection, Typeface, EmSize, Brush);
            System.Windows.Rect SelectionRect = new System.Windows.Rect(X, Y, ft.WidthIncludingTrailingWhitespace, LineHeight);

            WpfDrawingContext.DrawRectangle(BrushTable[BrushSettings.Selection], null, SelectionRect);
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

            Brush Brush = StyleToForegroundBrush(textStyle);
            FormattedText ft = new FormattedText(text, Culture, FlowDirection, Typeface, EmSize, Brush);

            double X = PagePadding.Left + origin.X;
            double Y = PagePadding.Top + origin.Y;
            double Width = ft.Width;
            double Height = LineHeight;

            if (textStyle == TextStyles.Comment)
            {
                WpfDrawingContext.DrawRectangle(BrushTable[BrushSettings.CommentBackground], null, new System.Windows.Rect(X, Y, CommentPadding.Left + Width + CommentPadding.Right, CommentPadding.Top + Height + CommentPadding.Bottom));

                X += CommentPadding.Left;
                Y += CommentPadding.Top;
            }

            WpfDrawingContext.DrawText(ft, new System.Windows.Point(X, Y));

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

            FormattedText ft = new FormattedText(text, Culture, FlowDirection, Typeface, EmSize, BrushTable[BrushSettings.Symbol]);
            WpfDrawingContext.DrawText(ft, new System.Windows.Point(PagePadding.Left + origin.X + padding.Left, PagePadding.Top + origin.Y + padding.Top));
        }

        /// <summary></summary>
        protected virtual void DrawGeometrySymbol(ScalableGeometry geometry, Point origin, Size size, Padding padding)
        {
            Debug.Assert(WpfDrawingContext != null);

            Point PaddedOrigin = origin.Moved(PagePadding.Left + padding.Left, PagePadding.Top + padding.Top);
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
                    ft = new FormattedText(DotText, Culture, FlowDirection, Typeface, EmSize, BrushTable[BrushSettings.Symbol]);
                    WpfDrawingContext.DrawText(ft, new System.Windows.Point(PagePadding.Left + origin.X - ft.WidthIncludingTrailingWhitespace, PagePadding.Top + origin.Y));
                    break;
                case HorizontalSeparators.Comma:
                    ft = new FormattedText(", ", Culture, FlowDirection, Typeface, EmSize, BrushTable[BrushSettings.Symbol]);
                    WpfDrawingContext.DrawText(ft, new System.Windows.Point(PagePadding.Left + origin.X - ft.WidthIncludingTrailingWhitespace, PagePadding.Top + origin.Y));
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

            Brush Brush = StyleToForegroundBrush(textStyle);
            FormattedText ft = new FormattedText(LeftText, Culture, FlowDirection, Typeface, EmSize, Brush);
            double X = origin.X + ft.WidthIncludingTrailingWhitespace;
            double Y = origin.Y;

            if (textStyle == TextStyles.Comment)
            {
                X += CommentPadding.Left;
                Y += CommentPadding.Top;
            }

            ChangeFlashClockOpacity(isVisible: true);

            if (mode == CaretModes.Insertion)
            {
                System.Windows.Rect CaretRect = new System.Windows.Rect(PagePadding.Left + X, PagePadding.Top + Y, InsertionCaretWidth, LineHeight);

                WpfDrawingContext.PushOpacity(1, FlashClock);
                WpfDrawingContext.DrawRectangle(BrushTable[BrushSettings.CaretInsertion], null, CaretRect);
                WpfDrawingContext.Pop();
            }
            else
            {
                string CharText = text.Substring(position, 1);
                ft = new FormattedText(CharText, Culture, FlowDirection, Typeface, EmSize, BrushTable[BrushSettings.CaretOverride]);

                System.Windows.Rect CaretRect = new System.Windows.Rect(PagePadding.Left + X, PagePadding.Top + Y, ft.WidthIncludingTrailingWhitespace, LineHeight);

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

        /// <summary>
        /// Draws the vertical separator above the specified origin and with the specified width.
        /// </summary>
        /// <param name="region">The region corresponding to the node that has a comment.</param>
        public virtual void DrawCommentIcon(Rect region)
        {
            if (CommentIcon != null)
            {
                WpfDrawingContext.PushOpacity(0.5);
                WpfDrawingContext.DrawImage(CommentIcon, new System.Windows.Rect(PagePadding.Left + region.X - (CommentIcon.Width / 2), PagePadding.Top + region.Y - (CommentIcon.Height / 2), CommentIcon.Width, CommentIcon.Height));
                WpfDrawingContext.Pop();
            }
        }

        /// <summary>
        /// Get the location where draw occurs corresponding to the specified absolute location.
        /// </summary>
        /// <param name="origin">The absolute location.</param>
        /// <returns>The relative location where things would be drawn.</returns>
        public virtual Point ToRelativeLocation(Point origin)
        {
            return origin.Moved(-PagePadding.Left, -PagePadding.Top);
        }

        /// <summary>
        /// Get the caret position corresponding to <paramref name="origin"/> in <paramref name="text"/>.
        /// </summary>
        /// <param name="origin">The location.</param>
        /// <param name="text">The text</param>
        /// <param name="textStyle">The style used to measure <paramref name="text"/>.</param>
        /// <param name="mode">The caret mode.</param>
        /// <param name="maxTextWidth">The maximum width for a line of text. NaN means no limit.</param>
        /// <returns>The position of the caret.</returns>
        public virtual int GetCaretPositionInText(Point origin, string text, TextStyles textStyle, CaretModes mode, double maxTextWidth)
        {
            Brush Brush = StyleToForegroundBrush(textStyle);

            if (textStyle == TextStyles.Comment)
                origin = origin.Moved(CommentPadding.Left, CommentPadding.Top);

            int Result = 0;

            for (int i = 0; i < text.Length; i++)
            {
                FormattedText ft = new FormattedText(text.Substring(0, i + 1), Culture, FlowDirection, Typeface, EmSize, Brush);
                if (!double.IsNaN(maxTextWidth))
                    ft.MaxTextWidth = maxTextWidth;

                if (ft.WidthIncludingTrailingWhitespace >= origin.X)
                    break;

                Result++;
            }

            return Result;
        }

        /// <summary>
        /// Draws the background of a selected rectangle.
        /// </summary>
        /// <param name="rect">The rectangle to draw.</param>
        public virtual void DrawSelectionRectangle(Rect rect)
        {
            Debug.Assert(WpfDrawingContext != null);
            Debug.Assert(RegionHelper.IsFixed(rect));

            WpfDrawingContext.DrawRectangle(BrushTable[BrushSettings.Selection], null, new System.Windows.Rect(PagePadding.Left + rect.X, PagePadding.Top + rect.Y, rect.Width, rect.Height));
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

            ft = new FormattedText(" ", Culture, FlowDirection, Typeface, EmSize, BrushTable[BrushSettings.Default]);
            WhitespaceWidth = ft.WidthIncludingTrailingWhitespace;
            LineHeight = ft.Height;
            TabulationWidth = WhitespaceWidth * 3;
            InsertionCaretWidth = WhitespaceWidth / 4;
            VerticalSeparatorWidthTable[VerticalSeparators.Line] = LineHeight;

            ft = new FormattedText(", ", Culture, FlowDirection, Typeface, EmSize, BrushTable[BrushSettings.Symbol]);
            HorizontalSeparatorWidthTable[HorizontalSeparators.Comma] = ft.WidthIncludingTrailingWhitespace;

            DotText = DotCharacter.ToString();
            ft = new FormattedText(DotText, Culture, FlowDirection, Typeface, EmSize, BrushTable[BrushSettings.Symbol]);
            HorizontalSeparatorWidthTable[HorizontalSeparators.Dot] = ft.WidthIncludingTrailingWhitespace;

            LeftBracketGeometry = ScaleGlyphGeometryHeight("[", true, 0.3, 0.3);
            RightBracketGeometry = ScaleGlyphGeometryHeight("]", true, 0.3, 0.3);
            LeftCurlyBracketGeometry = ScaleGlyphGeometryHeight("{", true, 0.25, 0.3);
            RightCurlyBracketGeometry = ScaleGlyphGeometryHeight("}", true, 0.25, 0.3);
            LeftParenthesisGeometry = ScaleGlyphGeometryHeight("(", true, 0, 0);
            RightParenthesisGeometry = ScaleGlyphGeometryHeight(")", true, 0, 0);
            HorizontalLineGeometry = ScaleGlyphGeometryWidth("-", true, 0, 0);
            CommentPadding = new Padding(WhitespaceWidth / 2, LineHeight / 4, WhitespaceWidth / 2, LineHeight / 4);

            double PagePaddingX, PagePaddingY;
            if (CommentIcon != null)
            {
                PagePaddingX = CommentIcon.Width / 2;
                PagePaddingY = CommentIcon.Height / 2;
            }
            else
            {
                PagePaddingX = 0;
                PagePaddingY = 0;
            }

            PagePadding = new Padding(PagePaddingX, PagePaddingY, InsertionCaretWidth, 0);
        }
        #endregion

        #region Implementation
        /// <summary></summary>
        protected virtual ScalableGeometry ScaleGlyphGeometryWidth(string text, bool isWidthScaled, double leftPercent, double rightPercent)
        {
            FormattedText GlyphText = new FormattedText(text, Culture, FlowDirection, Typeface, EmSize, BrushTable[BrushSettings.Symbol]);
            GlyphText.Trimming = System.Windows.TextTrimming.None;

            System.Windows.Rect Bounds = new System.Windows.Rect(new System.Windows.Point(0, 0), new System.Windows.Size(GlyphText.Width, GlyphText.Width));
            Geometry GlyphGeometry = GlyphText.BuildGeometry(Bounds.Location);

            return new ScalableGeometry(GlyphGeometry, Bounds, isWidthScaled, leftPercent, rightPercent, false, 0, 0);
        }

        /// <summary></summary>
        protected virtual ScalableGeometry ScaleGlyphGeometryHeight(string text, bool isHeightScaled, double topPercent, double bottomPercent)
        {
            FormattedText GlyphText = new FormattedText(text, Culture, FlowDirection, Typeface, EmSize, BrushTable[BrushSettings.Symbol]);
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
