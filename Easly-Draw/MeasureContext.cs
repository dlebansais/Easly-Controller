namespace EaslyDraw
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Windows.Media;
    using EaslyController.Constants;
    using EaslyController.Controller;
    using EaslyController.Layout;
    using EaslyNumber;

    /// <summary>
    /// An implementation of IxxxMeasureContext for WPF.
    /// </summary>
    public class MeasureContext : ILayoutMeasureContext
    {
        #region Constants
        /// <summary>
        /// The default brush table.
        /// </summary>
        public static IReadOnlyDictionary<BrushSettings, Brush> DefaultBrushTable { get; }

        /// <summary>
        /// The default pen table.
        /// </summary>
        public static IReadOnlyDictionary<PenSettings, Pen> DefaultPenTable { get; }
        #endregion

        #region Init
        static MeasureContext()
        {
            Color SelectionColor = Color.FromArgb(0xFF, 0x99, 0xC9, 0xEF);
            Color TypeIdentifierColor = Color.FromArgb(0xFF, 0x2B, 0x91, 0xAF);

            SolidColorBrush SelectionBrush = new SolidColorBrush(SelectionColor);
            SolidColorBrush TypeIdentifierBrush = new SolidColorBrush(TypeIdentifierColor);

            Dictionary<BrushSettings, Brush> BrushTable = new Dictionary<BrushSettings, Brush>
            {
                { BrushSettings.Default, Brushes.Black },
                { BrushSettings.Keyword, Brushes.Blue },
                { BrushSettings.Symbol, Brushes.Blue },
                { BrushSettings.Character, Brushes.Orange },
                { BrushSettings.Discrete, Brushes.DarkRed },
                { BrushSettings.NumberSignificand, Brushes.Green },
                { BrushSettings.NumberExponent, Brushes.Green },
                { BrushSettings.NumberInvalid, Brushes.Red },
                { BrushSettings.TypeIdentifier, TypeIdentifierBrush },
                { BrushSettings.CommentBackground, Brushes.LightGreen },
                { BrushSettings.CommentForeground, Brushes.Black },
                { BrushSettings.CaretInsertion, Brushes.Black },
                { BrushSettings.CaretOverride, Brushes.DarkGray },
                { BrushSettings.Selection, SelectionBrush },
                { BrushSettings.LineNumberForeground, TypeIdentifierBrush },
            };

            Dictionary<PenSettings, Pen> PenTable = new Dictionary<PenSettings, Pen>
            {
                { PenSettings.Default, null },
                { PenSettings.Comment, new Pen(Brushes.DarkGreen, 1) },
                { PenSettings.CaretInsertion, null },
                { PenSettings.CaretOverride, null },
                { PenSettings.SelectionText, null },
                { PenSettings.SelectionNode, new Pen(Brushes.Black, 1) { DashStyle = new DashStyle(new double[] { 1 }, 1) } },
                { PenSettings.SelectionNodeList, new Pen(Brushes.Black, 1) { DashStyle = new DashStyle(new double[] { 3 }, 3) } },
                { PenSettings.SelectionBlockList, new Pen(Brushes.Black, 1) { DashStyle = new DashStyle(new double[] { 3 }, 1) } },
                { PenSettings.BlockGeometry, new Pen(Brushes.Gray, 1) },
                { PenSettings.VerticalSeparator, new Pen(Brushes.Blue, 3) },
            };

            DefaultBrushTable = BrushTable;
            DefaultPenTable = PenTable;
        }

        /// <summary>
        /// Creates and initializes a new context.
        /// </summary>
        public static MeasureContext CreateMeasureContext(Typeface typeface, double fontSize, CultureInfo culture, System.Windows.FlowDirection flowDirection, IReadOnlyDictionary<BrushSettings, Brush> brushTable, IReadOnlyDictionary<PenSettings, Pen> penTable)
        {
            MeasureContext Result = new MeasureContext(typeface, fontSize, culture, flowDirection, brushTable, penTable);
            Result.Update();
            return Result;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MeasureContext"/> class.
        /// </summary>
        protected MeasureContext(Typeface typeface, double fontSize, CultureInfo culture, System.Windows.FlowDirection flowDirection, IReadOnlyDictionary<BrushSettings, Brush> brushTable, IReadOnlyDictionary<PenSettings, Pen> penTable)
        {
            Typeface = typeface;
            FontSize = fontSize;
            Culture = culture;
            FlowDirection = flowDirection;
            SubscriptRatio = 0.7;
            CommaSeparatorString = ", ";
            DotSeparatorString = "·";
            BrushTable = brushTable ?? DefaultBrushTable;
            PenTable = penTable ?? DefaultPenTable;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Width of a tabulation margin.
        /// </summary>
        public Measure TabulationWidth { get; protected set; }

        /// <summary>
        /// Width of a vertical block geometry.
        /// </summary>
        public Measure BlockGeometryWidth { get; protected set; }

        /// <summary>
        /// Height of an horizontal block geometry.
        /// </summary>
        public Measure BlockGeometryHeight { get; protected set; }

        /// <summary>
        /// Height of a line of text.
        /// </summary>
        public Measure LineHeight { get; protected set; }

        /// <summary>
        /// The padding applied to the entire page.
        /// </summary>
        public Padding PagePadding { get; protected set; }

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
        /// The ratio to normal size for subscript characters.
        /// </summary>
        public double SubscriptRatio { get; set; }

        /// <summary>
        /// The specific culture of the text.
        /// </summary>
        public CultureInfo Culture { get; set; }

        /// <summary>
        /// The direction the text is read.
        /// </summary>
        public System.Windows.FlowDirection FlowDirection { get; set; }

        /// <summary>
        /// The string used as comma separator.
        /// </summary>
        public string CommaSeparatorString { get; set; }

        /// <summary>
        /// The string used as dot separator.
        /// </summary>
        public string DotSeparatorString { get; set; }

        /// <summary>
        /// The insertion caret width.
        /// </summary>
        public double InsertionCaretWidth { get; protected set; }

        /// <summary>
        /// Table of brushes to use when drawing.
        /// </summary>
        public IReadOnlyDictionary<BrushSettings, Brush> BrushTable { get; }

        /// <summary>
        /// Table of pens to use when drawing.
        /// </summary>
        public IReadOnlyDictionary<PenSettings, Pen> PenTable { get; }
        #endregion

        #region Implementation of IxxxMeasureContext
        /// <summary>
        /// Gets the width corresponding to a separator between cells in a line.
        /// </summary>
        /// <param name="separator">The separator.</param>
        public virtual Measure GetHorizontalSeparatorWidth(HorizontalSeparators separator)
        {
            return HorizontalSeparatorWidthTable[separator];
        }

        /// <summary>
        /// Gets the height corresponding to a separator between cells in a column.
        /// </summary>
        /// <param name="separator">The separator.</param>
        public virtual Measure GetVerticalSeparatorHeight(VerticalSeparators separator)
        {
            return VerticalSeparatorHeightTable[separator];
        }

        /// <summary>
        /// Measures a string that is not a number.
        /// </summary>
        /// <param name="text">The string to measure.</param>
        /// <param name="textStyle">Style to use for the text.</param>
        /// <param name="maxTextWidth">The maximum width for a line of text. NaN means no limit.</param>
        /// <returns>The size of the string.</returns>
        public virtual Size MeasureText(string text, TextStyles textStyle, Measure maxTextWidth)
        {
            Brush Brush = GetBrush(StyleToForegroundBrush(textStyle));
            FormattedText ft = CreateFormattedText(text, EmSize, Brush);
            if (!maxTextWidth.IsFloating)
                ft.MaxTextWidth = maxTextWidth.Draw;

            double TextWidth = ft.WidthIncludingTrailingWhitespace;
            if (TextWidth < InsertionCaretWidth)
                TextWidth = InsertionCaretWidth;

            Measure Width = new Measure() { Draw = TextWidth, Print = text.Length };
            Measure Height = LineHeight;
            return new Size(Width, Height);
        }

        /// <summary>
        /// Measures a number string.
        /// </summary>
        /// <param name="text">The string to measure.</param>
        /// <returns>The size of the string.</returns>
        public virtual Size MeasureNumber(string text)
        {
            FormattedNumber fn = FormattedNumber.Parse(text);
            string SignificandPart = fn.SignificandPart;
            string ExponentPart = fn.ExponentPart;
            string InvalidText = fn.InvalidText;

            Brush Brush;

            Brush = GetBrush(BrushSettings.NumberSignificand);
            FormattedText ftSignificand = CreateFormattedText(SignificandPart, EmSize, Brush);

            Brush = GetBrush(BrushSettings.NumberExponent);
            FormattedText ftExponent = CreateFormattedText(ExponentPart, EmSize * SubscriptRatio, Brush);

            Brush = GetBrush(BrushSettings.NumberInvalid);
            FormattedText ftInvalid = CreateFormattedText(InvalidText, EmSize, Brush);

            Measure Width = new Measure() { Draw = ftSignificand.WidthIncludingTrailingWhitespace + ftExponent.WidthIncludingTrailingWhitespace + ftInvalid.WidthIncludingTrailingWhitespace, Print = text.Length };
            Measure Height = LineHeight;
            return new Size(Width, Height);
        }

        /// <summary>
        /// Measures a symbol.
        /// </summary>
        /// <param name="symbol">The symbol to measure.</param>
        /// <returns>The size of the symbol.</returns>
        public virtual Size MeasureSymbolSize(Symbols symbol)
        {
            string Text = SymbolToText(symbol);
            FormattedText ft = CreateFormattedText(Text, EmSize, GetBrush(BrushSettings.Symbol));

            switch (symbol)
            {
                default:
                case Symbols.LeftArrow:
                case Symbols.Dot:
                case Symbols.InsertSign:
                    return new Size(new Measure() { Draw = ft.Width, Print = Text.Length }, LineHeight);
                case Symbols.LeftBracket:
                case Symbols.RightBracket:
                case Symbols.LeftCurlyBracket:
                case Symbols.RightCurlyBracket:
                case Symbols.LeftParenthesis:
                case Symbols.RightParenthesis:
                    return new Size(new Measure() { Draw = ft.Width, Print = Text.Length }, Measure.Floating);
                case Symbols.HorizontalLine:
                    return new Size(Measure.Floating, VerticalSeparatorHeightTable[VerticalSeparators.Line]);
            }
        }

        /// <summary></summary>
        protected virtual BrushSettings StyleToForegroundBrush(TextStyles textStyle)
        {
            switch (textStyle)
            {
                default:
                case TextStyles.Default:
                    return BrushSettings.Default;
                case TextStyles.Character:
                    return BrushSettings.Character;
                case TextStyles.Discrete:
                    return BrushSettings.Discrete;
                case TextStyles.Keyword:
                    return BrushSettings.Keyword;
                case TextStyles.Type:
                    return BrushSettings.TypeIdentifier;
                case TextStyles.Comment:
                    return BrushSettings.CommentForeground;
                case TextStyles.LineNumber:
                    return BrushSettings.LineNumberForeground;
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
                    Text = DotSeparatorString;
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
            int LeftSpacing = 0;
            int RightSpacing = 0;

            switch (leftMargin)
            {
                case Margins.ThinSpace:
                    LeftPadding = WhitespaceWidth / 2;
                    break;
                case Margins.Whitespace:
                    LeftPadding = WhitespaceWidth;
                    LeftSpacing = 1;
                    break;
            }

            switch (rightMargin)
            {
                case Margins.ThinSpace:
                    RightPadding = WhitespaceWidth / 2;
                    break;
                case Margins.Whitespace:
                    RightPadding = WhitespaceWidth;
                    RightSpacing = 1;
                    break;
            }

            if (size.IsEmpty)
                padding = Padding.Empty;
            else
            {
                size = new Size(new Measure() { Draw = size.Width.Draw + LeftPadding + RightPadding, Print = size.Width.Print + LeftSpacing + RightSpacing }, size.Height);
                padding = new Padding(new Measure() { Draw = LeftPadding, Print = LeftSpacing }, Measure.Zero, new Measure() { Draw = RightPadding, Print = RightSpacing }, Measure.Zero);
            }
        }

        private protected virtual Brush GetBrush(BrushSettings key)
        {
            if (BrushTable.ContainsKey(key))
                return BrushTable[key];
            else
                return DefaultBrushTable[key];
        }

        private protected virtual Pen GetPen(PenSettings key)
        {
            if (PenTable.ContainsKey(key))
                return PenTable[key];
            else
                return DefaultPenTable[key];
        }
        #endregion

        #region Client Interface
        /// <summary>
        /// Recalculate internal constants.
        /// To call after a property was changed.
        /// </summary>
        public virtual void Update()
        {
            FormattedText ft;

            ft = CreateFormattedText(" ", EmSize, GetBrush(BrushSettings.Default));
            WhitespaceWidth = ft.WidthIncludingTrailingWhitespace;
            InsertionCaretWidth = WhitespaceWidth / 4;
            LineHeight = new Measure() { Draw = ft.Height, Print = 1 };
            int TabulationLength = 3;
            TabulationWidth = new Measure() { Draw = WhitespaceWidth * TabulationLength, Print = TabulationLength };
            BlockGeometryWidth = new Measure() { Draw = WhitespaceWidth, Print = 1 };
            BlockGeometryHeight = new Measure() { Draw = LineHeight.Draw, Print = 1 };
            VerticalSeparatorHeightTable[VerticalSeparators.Line] = new Measure() { Draw = LineHeight.Draw * 3, Print = 3 };

            ft = CreateFormattedText(CommaSeparatorString, EmSize, GetBrush(BrushSettings.Symbol));
            HorizontalSeparatorWidthTable[HorizontalSeparators.Comma] = new Measure() { Draw = ft.WidthIncludingTrailingWhitespace, Print = CommaSeparatorString.Length };

            ft = CreateFormattedText(DotSeparatorString, EmSize, GetBrush(BrushSettings.Symbol));
            HorizontalSeparatorWidthTable[HorizontalSeparators.Dot] = new Measure() { Draw = ft.WidthIncludingTrailingWhitespace, Print = DotSeparatorString.Length };
        }

        /// <summary></summary>
        protected double WhitespaceWidth { get; set; }

        /// <summary></summary>
        protected Dictionary<HorizontalSeparators, Measure> HorizontalSeparatorWidthTable { get; } = new Dictionary<HorizontalSeparators, Measure>()
        {
            { HorizontalSeparators.None, Measure.Zero },
            { HorizontalSeparators.Comma, Measure.Zero },
            { HorizontalSeparators.Dot, Measure.Zero },
        };

        /// <summary></summary>
        protected Dictionary<VerticalSeparators, Measure> VerticalSeparatorHeightTable { get; } = new Dictionary<VerticalSeparators, Measure>()
        {
            { VerticalSeparators.None, Measure.Zero },
            { VerticalSeparators.Line, Measure.Zero },
        };
        #endregion

        #region Tools
        /// <summary>
        /// Creates a <see cref="FormattedText"/> with the specified text, font size and brush.
        /// </summary>
        /// <param name="textToFormat">The text to be displayed.</param>
        /// <param name="emSize">The font size the text should be formatted at.</param>
        /// <param name="foreground">The brush used to paint the each glyph.</param>
        protected FormattedText CreateFormattedText(string textToFormat, double emSize, Brush foreground)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            FormattedText Result = new FormattedText(textToFormat, Culture, FlowDirection, Typeface, emSize, foreground);
#pragma warning restore CS0618 // Type or member is obsolete

            return Result;
        }
        #endregion
    }
}
