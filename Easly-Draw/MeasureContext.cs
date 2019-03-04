namespace EaslyDraw
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Windows.Media;
    using EaslyController.Constants;
    using EaslyController.Controller;
    using EaslyController.Layout;

    /// <summary>
    /// An implementation of IxxxMeasureContext for WPF.
    /// </summary>
    public class MeasureContext : ILayoutMeasureContext
    {
        #region Init
        /// <summary>
        /// Creates and initializes a new context.
        /// </summary>
        public static MeasureContext CreateMeasureContext()
        {
            MeasureContext Result = new MeasureContext();
            Result.Update();
            return Result;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MeasureContext"/> class.
        /// </summary>
        protected MeasureContext()
        {
            Typeface = new Typeface("Consolas");
            FontSize = 10;
            Culture = CultureInfo.CurrentCulture;
            FlowDirection = System.Windows.FlowDirection.LeftToRight;
            CommaSeparatorString = ", ";
            DotSeparatorString = "·";

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
        }
        #endregion

        #region Properties
        /// <summary>
        /// Width of a tabulation margin.
        /// </summary>
        public double TabulationWidth { get; private set; }

        /// <summary>
        /// Width of a tabulation margin, in number of whitespace.
        /// </summary>
        public int TabulationLength { get; private set; }

        /// <summary>
        /// Height of a line of text.
        /// </summary>
        public double LineHeight { get; private set; }

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
        /// The string used as comma separator.
        /// </summary>
        public string CommaSeparatorString { get; set; }

        /// <summary>
        /// The string used as dot separator.
        /// </summary>
        public string DotSeparatorString { get; set; }

        /// <summary>
        /// Table of brushes to use when drawing.
        /// </summary>
        public IDictionary<BrushSettings, Brush> BrushTable { get; }
        #endregion

        #region Implementation of IxxxMeasureContext
        /// <summary>
        /// Gets the width corresponding to a separator between cells in a line.
        /// </summary>
        /// <param name="separator">The separator.</param>
        public virtual double GetHorizontalSeparatorWidth(HorizontalSeparators separator)
        {
            return HorizontalSeparatorWidthTable[separator];
        }

        /// <summary>
        /// Gets the width corresponding to a separator between cells in a line, in number of whitespace.
        /// </summary>
        /// <param name="separator">The separator.</param>
        public virtual int GetHorizontalSeparatorLength(HorizontalSeparators separator)
        {
            switch (separator)
            {
                default:
                case HorizontalSeparators.None:
                    return 0;

                case HorizontalSeparators.Comma:
                    return CommaSeparatorString.Length;

                case HorizontalSeparators.Dot:
                    return DotSeparatorString.Length;
            }
        }

        /// <summary>
        /// Gets the height corresponding to a separator between cells in a column.
        /// </summary>
        /// <param name="separator">The separator.</param>
        public virtual SeparatorLength GetVerticalSeparatorHeight(VerticalSeparators separator)
        {
            return VerticalSeparatorWidthTable[separator];
        }

        /// <summary>
        /// Gets the height corresponding to a separator between cells in a column, in number of whitespace.
        /// </summary>
        /// <param name="separator">The separator.</param>
        public virtual int GetVerticalSeparatorLineCount(VerticalSeparators separator)
        {
            switch (separator)
            {
                default:
                case VerticalSeparators.None:
                    return 0;

                case VerticalSeparators.Line:
                    return 1;
            }
        }

        /// <summary>
        /// Measures a string.
        /// </summary>
        /// <param name="text">The string to measure.</param>
        /// <param name="textStyle">Style to use for the text.</param>
        /// <param name="maxTextWidth">The maximum width for a line of text. NaN means no limit.</param>
        /// <returns>The size of the string.</returns>
        public virtual Size MeasureTextSize(string text, TextStyles textStyle, double maxTextWidth)
        {
            Brush Brush = BrushTable[StyleToForegroundBrush(textStyle)];
            FormattedText ft = new FormattedText(text, Culture, FlowDirection, Typeface, EmSize, Brush);
            if (!double.IsNaN(maxTextWidth))
                ft.MaxTextWidth = maxTextWidth;

            double Width = ft.Width;
            double Height = LineHeight;
            return new Size(Width, Height);
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
                case TextStyles.Number:
                    return BrushSettings.Number;
                case TextStyles.Type:
                    return BrushSettings.TypeIdentifier;
                case TextStyles.Comment:
                    return BrushSettings.CommentForeground;
            }
        }

        /// <summary>
        /// Measures a string.
        /// </summary>
        /// <param name="text">The string to measure.</param>
        /// <param name="textStyle">Style to use for the text.</param>
        /// <param name="maxTextLength">The maximum width for a line of text. 0 means no limit.</param>
        /// <returns>The size of the string.</returns>
        public virtual Plane MeasureTextPlane(string text, TextStyles textStyle, int maxTextLength)
        {
            int Length;
            int Lines;

            if (maxTextLength <= 0 || text.Length <= maxTextLength)
            {
                Length = text.Length;
                Lines = 1;
            }
            else
            {
                Length = maxTextLength;
                Lines = (text.Length + maxTextLength - 1) / maxTextLength;
            }

            return new Plane(Length, Lines);
        }

        /// <summary>
        /// Measures a symbol.
        /// </summary>
        /// <param name="symbol">The symbol to measure.</param>
        /// <returns>The size of the symbol.</returns>
        public virtual Size MeasureSymbolSize(Symbols symbol)
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

        /// <summary>
        /// Measures a symbol.
        /// </summary>
        /// <param name="symbol">The symbol to measure.</param>
        /// <returns>The size of the symbol.</returns>
        public virtual Plane MeasureSymbolPlane(Symbols symbol)
        {
            string Text = SymbolToText(symbol);

            switch (symbol)
            {
                default:
                case Symbols.LeftArrow:
                case Symbols.Dot:
                case Symbols.InsertSign:
                    return new Plane(Text.Length, 1);
                case Symbols.LeftBracket:
                case Symbols.RightBracket:
                case Symbols.LeftCurlyBracket:
                case Symbols.RightCurlyBracket:
                case Symbols.LeftParenthesis:
                case Symbols.RightParenthesis:
                    return new Plane(Text.Length, -1);
                case Symbols.HorizontalLine:
                    return new Plane(-1, 1);
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
        /// Extends a size according to the left and right margin settings.
        /// </summary>
        /// <param name="leftMargin">The left margin setting.</param>
        /// <param name="rightMargin">The right margin setting.</param>
        /// <param name="plane">The size to extend with the calculated padding.</param>
        /// <param name="padding">The padding calculated from <paramref name="leftMargin"/> and <paramref name="rightMargin"/>.</param>
        public virtual void UpdatePadding(Margins leftMargin, Margins rightMargin, ref Plane plane, out SpacePadding padding)
        {
            int LeftPadding = 0;
            int RightPadding = 0;

            switch (leftMargin)
            {
                case Margins.ThinSpace:
                    break;
                case Margins.Whitespace:
                    LeftPadding = 1;
                    break;
            }

            switch (rightMargin)
            {
                case Margins.ThinSpace:
                    break;
                case Margins.Whitespace:
                    RightPadding = 1;
                    break;
            }

            plane = new Plane(plane.Width + LeftPadding + RightPadding, plane.Height);
            padding = new SpacePadding(LeftPadding, RightPadding);
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

            ft = new FormattedText(" ", Culture, FlowDirection, Typeface, EmSize, BrushTable[BrushSettings.Default]);
            WhitespaceWidth = ft.WidthIncludingTrailingWhitespace;
            LineHeight = ft.Height;
            TabulationLength = 3;
            TabulationWidth = WhitespaceWidth * TabulationLength;
            VerticalSeparatorWidthTable[VerticalSeparators.Line] = LineHeight;

            ft = new FormattedText(CommaSeparatorString, Culture, FlowDirection, Typeface, EmSize, BrushTable[BrushSettings.Symbol]);
            HorizontalSeparatorWidthTable[HorizontalSeparators.Comma] = ft.WidthIncludingTrailingWhitespace;

            ft = new FormattedText(DotSeparatorString, Culture, FlowDirection, Typeface, EmSize, BrushTable[BrushSettings.Symbol]);
            HorizontalSeparatorWidthTable[HorizontalSeparators.Dot] = ft.WidthIncludingTrailingWhitespace;
        }

        /// <summary></summary>
        protected double WhitespaceWidth { get; private set; }

        /// <summary></summary>
        protected Dictionary<HorizontalSeparators, double> HorizontalSeparatorWidthTable { get; } = new Dictionary<HorizontalSeparators, double>()
        {
            { HorizontalSeparators.None, 0 },
            { HorizontalSeparators.Comma, 0 },
            { HorizontalSeparators.Dot, 0 },
        };

        /// <summary></summary>
        protected Dictionary<VerticalSeparators, SeparatorLength> VerticalSeparatorWidthTable { get; } = new Dictionary<VerticalSeparators, SeparatorLength>()
        {
            { VerticalSeparators.None, SeparatorLength.Empty },
            { VerticalSeparators.Line, new SeparatorLength() { Draw = 30, Print = 3 } },
        };
        #endregion
    }
}
