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
    public class DrawContext : MeasureContext, ILayoutDrawContext
    {
        #region Init
        /// <summary>
        /// Creates and initializes a new context.
        /// </summary>
        public static DrawContext CreateDrawContext()
        {
            DrawContext Result = new DrawContext();
            Result.Update();
            return Result;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DrawContext"/> class.
        /// </summary>
        protected DrawContext()
        {
            IsLastFocusedFullCell = false;

            FlashAnimation = new DoubleAnimation(0, new System.Windows.Duration(TimeSpan.FromSeconds(1)));
            FlashAnimation.RepeatBehavior = RepeatBehavior.Forever;
            FlashAnimation.EasingFunction = new FlashEasingFunction();
            FlashClock = FlashAnimation.CreateClock();
            CommentIcon = LoadPngResource("Comment");
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
        /// Measures a string.
        /// </summary>
        /// <param name="text">The string to measure.</param>
        /// <param name="textStyle">Style to use for the text.</param>
        /// <param name="maxTextWidth">The maximum width for a line of text. NaN means no limit.</param>
        /// <returns>The size of the string.</returns>
        public override Size MeasureTextSize(string text, TextStyles textStyle, Measure maxTextWidth)
        {
            Size Result = base.MeasureTextSize(text, textStyle, maxTextWidth);

            if (textStyle == TextStyles.Comment)
                Result = new Size(CommentPadding.Left + Result.Width + CommentPadding.Right, CommentPadding.Top + Result.Height + CommentPadding.Bottom);

            return Result;
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

            Brush Brush = BrushTable[StyleToForegroundBrush(textStyle)];
            FormattedText ft;
            double X = PagePadding.Left.Draw + origin.X.Draw;
            double Y = PagePadding.Top.Draw + origin.Y.Draw;

            if (textStyle == TextStyles.Comment)
            {
                X += CommentPadding.Left.Draw;
                Y += CommentPadding.Top.Draw;
            }

            ft = new FormattedText(text.Substring(0, start), Culture, FlowDirection, Typeface, EmSize, Brush);
            X += ft.WidthIncludingTrailingWhitespace;

            ft = new FormattedText(text.Substring(start, end - start), Culture, FlowDirection, Typeface, EmSize, Brush);
            System.Windows.Rect SelectionRect = new System.Windows.Rect(X, Y, ft.WidthIncludingTrailingWhitespace, LineHeight.Draw);

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

            Brush Brush = BrushTable[StyleToForegroundBrush(textStyle)];
            FormattedText ft = new FormattedText(text, Culture, FlowDirection, Typeface, EmSize, Brush);

            double X = PagePadding.Left.Draw + origin.X.Draw;
            double Y = PagePadding.Top.Draw + origin.Y.Draw;
            double Width = ft.Width;
            double Height = LineHeight.Draw;

            if (textStyle == TextStyles.Comment)
            {
                WpfDrawingContext.DrawRectangle(BrushTable[BrushSettings.CommentBackground], null, new System.Windows.Rect(X, Y, CommentPadding.Left.Draw + Width + CommentPadding.Right.Draw, CommentPadding.Top.Draw + Height + CommentPadding.Bottom.Draw));

                X += CommentPadding.Left.Draw;
                Y += CommentPadding.Top.Draw;
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

            Brush ForegroundBrush = BrushTable[BrushSettings.Symbol];
            FormattedText ft = new FormattedText(text, Culture, FlowDirection, Typeface, EmSize, ForegroundBrush);
            WpfDrawingContext.DrawText(ft, new System.Windows.Point(PagePadding.Left.Draw + origin.X.Draw + padding.Left.Draw, PagePadding.Top.Draw + origin.Y.Draw + padding.Top.Draw));
        }

        /// <summary></summary>
        protected virtual void DrawGeometrySymbol(ScalableGeometry geometry, Point origin, Size size, Padding padding)
        {
            Debug.Assert(WpfDrawingContext != null);

            Point PaddedOrigin = origin.Moved(PagePadding.Left + padding.Left, PagePadding.Top + padding.Top);
            Size PaddedSize = new Size(size.Width - padding.Left - padding.Right, size.Height - padding.Top - padding.Bottom);
            Geometry GeometryAtOrigin = MoveAndScaleGeometry(geometry, PaddedOrigin, GeometryScalings.None, GeometryScalings.Stretch, PaddedSize);

            Brush ForegroundBrush = BrushTable[BrushSettings.Symbol];
            WpfDrawingContext.DrawGeometry(ForegroundBrush, null, GeometryAtOrigin);
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
                    Width = measuredSize.Width.Draw;
                    break;
            }

            switch (heightScaling)
            {
                default:
                case GeometryScalings.None:
                    Height = double.NaN;
                    break;

                case GeometryScalings.Font:
                    Height = LineHeight.Draw;
                    break;

                case GeometryScalings.Stretch:
                    Height = measuredSize.Height.Draw;
                    break;
            }

            Geometry Result = geometry.Scaled(Width, Height);
            Result.Transform = new TranslateTransform(origin.X.Draw, origin.Y.Draw);

            Result.Freeze();

            return Result;
        }

        /// <summary>
        /// Draws the horizontal separator left of the specified origin and with the specified height.
        /// </summary>
        /// <param name="separator">The separator to draw.</param>
        /// <param name="origin">The location where to draw.</param>
        /// <param name="height">The separator height.</param>
        public virtual void DrawHorizontalSeparator(HorizontalSeparators separator, Point origin, Measure height)
        {
            Debug.Assert(WpfDrawingContext != null);

            Brush ForegroundBrush = BrushTable[BrushSettings.Symbol];
            FormattedText ft;

            switch (separator)
            {
                case HorizontalSeparators.Comma:
                    ft = new FormattedText(CommaSeparatorString, Culture, FlowDirection, Typeface, EmSize, ForegroundBrush);
                    WpfDrawingContext.DrawText(ft, new System.Windows.Point(PagePadding.Left.Draw + origin.X.Draw - ft.WidthIncludingTrailingWhitespace, PagePadding.Top.Draw + origin.Y.Draw));
                    break;
                case HorizontalSeparators.Dot:
                    ft = new FormattedText(DotSeparatorString, Culture, FlowDirection, Typeface, EmSize, ForegroundBrush);
                    WpfDrawingContext.DrawText(ft, new System.Windows.Point(PagePadding.Left.Draw + origin.X.Draw - ft.WidthIncludingTrailingWhitespace, PagePadding.Top.Draw + origin.Y.Draw));
                    break;
            }
        }

        /// <summary>
        /// Draws the vertical separator above the specified origin and with the specified width.
        /// </summary>
        /// <param name="separator">The separator to draw.</param>
        /// <param name="origin">The location where to draw.</param>
        /// <param name="width">The separator width.</param>
        public virtual void DrawVerticalSeparator(VerticalSeparators separator, Point origin, Measure width)
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

            Brush Brush = BrushTable[StyleToForegroundBrush(textStyle)];
            FormattedText ft = new FormattedText(LeftText, Culture, FlowDirection, Typeface, EmSize, Brush);
            double X = origin.X.Draw + ft.WidthIncludingTrailingWhitespace;
            double Y = origin.Y.Draw;

            if (textStyle == TextStyles.Comment)
            {
                X += CommentPadding.Left.Draw;
                Y += CommentPadding.Top.Draw;
            }

            ChangeFlashClockOpacity(isVisible: true);

            if (mode == CaretModes.Insertion)
            {
                System.Windows.Rect CaretRect = new System.Windows.Rect(PagePadding.Left.Draw + X, PagePadding.Top.Draw + Y, InsertionCaretWidth, LineHeight.Draw);

                WpfDrawingContext.PushOpacity(1, FlashClock);
                WpfDrawingContext.DrawRectangle(BrushTable[BrushSettings.CaretInsertion], null, CaretRect);
                WpfDrawingContext.Pop();
            }
            else
            {
                string CharText = text.Substring(position, 1);
                ft = new FormattedText(CharText, Culture, FlowDirection, Typeface, EmSize, BrushTable[BrushSettings.CaretOverride]);

                System.Windows.Rect CaretRect = new System.Windows.Rect(PagePadding.Left.Draw + X, PagePadding.Top.Draw + Y, ft.WidthIncludingTrailingWhitespace, LineHeight.Draw);

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
                WpfDrawingContext.DrawImage(CommentIcon, new System.Windows.Rect(PagePadding.Left.Draw + region.X - (CommentIcon.Width / 2), PagePadding.Top.Draw + region.Y - (CommentIcon.Height / 2), CommentIcon.Width, CommentIcon.Height));
                WpfDrawingContext.Pop();
            }
        }

        /// <summary>
        /// Get the location where draw occurs corresponding to the specified absolute location.
        /// </summary>
        /// <param name="x">X-coordinate of the location, absolute on entry, relative upon return.</param>
        /// <param name="y">Y-coordinate of the location, absolute on entry, relative upon return.</param>
        public virtual void ToRelativeLocation(ref double x, ref double y)
        {
            x -= PagePadding.Left.Draw;
            y -= PagePadding.Top.Draw;
        }

        /// <summary>
        /// Get the caret position corresponding to <paramref name="x"/> in <paramref name="text"/>.
        /// </summary>
        /// <param name="x">X-coordinate of the caret location.</param>
        /// <param name="text">The text</param>
        /// <param name="textStyle">The style used to measure <paramref name="text"/>.</param>
        /// <param name="mode">The caret mode.</param>
        /// <param name="maxTextWidth">The maximum width for a line of text. NaN means no limit.</param>
        /// <returns>The position of the caret.</returns>
        public virtual int GetCaretPositionInText(double x, string text, TextStyles textStyle, CaretModes mode, Measure maxTextWidth)
        {
            Brush Brush = BrushTable[StyleToForegroundBrush(textStyle)];

            if (textStyle == TextStyles.Comment)
                x += CommentPadding.Left.Draw;

            int Result = 0;

            for (int i = 0; i < text.Length; i++)
            {
                FormattedText ft = new FormattedText(text.Substring(0, i + 1), Culture, FlowDirection, Typeface, EmSize, Brush);
                if (!maxTextWidth.IsFloating)
                    ft.MaxTextWidth = maxTextWidth.Draw;

                if (ft.WidthIncludingTrailingWhitespace >= x)
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

            WpfDrawingContext.DrawRectangle(BrushTable[BrushSettings.Selection], null, new System.Windows.Rect(PagePadding.Left.Draw + rect.X, PagePadding.Top.Draw + rect.Y, rect.Width, rect.Height));
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
        public override void Update()
        {
            base.Update();

            FormattedText ft;

            ft = new FormattedText(" ", Culture, FlowDirection, Typeface, EmSize, BrushTable[BrushSettings.Default]);
            InsertionCaretWidth = WhitespaceWidth / 4;

            LeftBracketGeometry = ScaleGlyphGeometryHeight("[", true, 0.3, 0.3);
            RightBracketGeometry = ScaleGlyphGeometryHeight("]", true, 0.3, 0.3);
            LeftCurlyBracketGeometry = ScaleGlyphGeometryHeight("{", true, 0.25, 0.3);
            RightCurlyBracketGeometry = ScaleGlyphGeometryHeight("}", true, 0.25, 0.3);
            LeftParenthesisGeometry = ScaleGlyphGeometryHeight("(", true, 0, 0);
            RightParenthesisGeometry = ScaleGlyphGeometryHeight(")", true, 0, 0);
            HorizontalLineGeometry = ScaleGlyphGeometryWidth("-", true, 0, 0);
            CommentPadding = new Padding(new Measure() { Draw = WhitespaceWidth / 2 }, new Measure() { Draw = LineHeight.Draw / 4 }, new Measure() { Draw = WhitespaceWidth / 2 }, new Measure() { Draw = LineHeight.Draw / 4 });

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

            PagePadding = new Padding(new Measure() { Draw = PagePaddingX }, new Measure() { Draw = PagePaddingY }, new Measure() { Draw = InsertionCaretWidth }, Measure.Zero);
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

        /// <summary></summary>
        protected ScalableGeometry LeftBracketGeometry { get; private set; }
        /// <summary></summary>
        protected ScalableGeometry RightBracketGeometry { get; private set; }
        /// <summary></summary>
        protected ScalableGeometry LeftCurlyBracketGeometry { get; private set; }
        /// <summary></summary>
        protected ScalableGeometry RightCurlyBracketGeometry { get; private set; }
        /// <summary></summary>
        protected ScalableGeometry LeftParenthesisGeometry { get; private set; }
        /// <summary></summary>
        protected ScalableGeometry RightParenthesisGeometry { get; private set; }
        /// <summary></summary>
        protected ScalableGeometry HorizontalLineGeometry { get; private set; }
        /// <summary></summary>
        protected DoubleAnimation FlashAnimation { get; private set; }
        /// <summary></summary>
        protected AnimationClock FlashClock { get; private set; }
        /// <summary></summary>
        protected bool IsLastFocusedFullCell { get; private set; }
        #endregion
    }
}
