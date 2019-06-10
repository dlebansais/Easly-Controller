namespace EaslyDraw
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Windows.Media;

    /// <summary>
    /// An area where to print characters with a brush.
    /// </summary>
    public class PrintableArea
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="PrintableArea"/> class.
        /// </summary>
        public PrintableArea()
        {
            PrintArea = new PrintableCharacter[0, 0];
        }

        private PrintableCharacter[,] PrintArea;
        #endregion

        #region Properties
        /// <summary>
        /// Actual area width.
        /// </summary>
        public int Width { get { return PrintArea.GetLength(0); } }

        /// <summary>
        /// Actual area height.
        /// </summary>
        public int Height { get { return PrintArea.GetLength(1); } }
        #endregion

        #region Client Interface
        /// <summary>
        /// Prints <paramref name="c"/> at coordinates <paramref name="x"/> and <paramref name="y"/>.
        /// </summary>
        /// <param name="c">The character to print.</param>
        /// <param name="x">X-coordinate in the area.</param>
        /// <param name="y">Y-coordinate in the area.</param>
        /// <param name="brush">The brush to print with.</param>
        public virtual void Print(char c, int x, int y, BrushSettings brush)
        {
            Print(c.ToString(), x, y, brush);
        }

        /// <summary>
        /// Prints <paramref name="text"/> at coordinates <paramref name="x"/> and <paramref name="y"/>.
        /// </summary>
        /// <param name="text">The text to print.</param>
        /// <param name="x">X-coordinate in the area.</param>
        /// <param name="y">Y-coordinate in the area.</param>
        /// <param name="brush">The brush to print with.</param>
        public virtual void Print(string text, int x, int y, BrushSettings brush)
        {
            int CX = Width;
            int CY = Height;

            int MaxCX = x + text.Length > CX ? x + text.Length : CX;
            int MaxCY = y + 1 > CY ? y + 1 : CY;

            if (MaxCX > CX || MaxCY > CY)
            {
                PrintableCharacter[,] NewPrintArea = new PrintableCharacter[MaxCX, MaxCY];
                for (int i = 0; i < CX; i++)
                    for (int j = 0; j < CY; j++)
                        NewPrintArea[i, j] = PrintArea[i, j];

                PrintArea = NewPrintArea;
            }

            for (int n = 0; n < text.Length; n++)
                PrintArea[x + n, y] = new PrintableCharacter(text[n], brush);
        }

        /// <summary>
        /// Returns a string representation of this instance.
        /// </summary>
        public virtual string ToStringContent(bool isSingleLine)
        {
            string[] Lines = new string[Height];

            for (int i = 0; i < Height; i++)
            {
                int Length;
                for (Length = Width; Length > 0; Length--)
                {
                    PrintableCharacter c = PrintArea[Length - 1, i];
                    if (c != null)
                        break;
                }

                string Line = string.Empty;

                for (int j = 0; j < Length; j++)
                {
                    PrintableCharacter c = PrintArea[j, i];
                    if (c != null)
                        Line += c.C;
                    else
                        Line += " ";
                }

                Lines[i] = Line;
            }

            string Result;

            if (isSingleLine && Lines.Length == 1)
                Result = Lines[0];
            else
            {
                Result = string.Empty;

                foreach (string Line in Lines)
                    Result += Line + "\r\n";
            }

            return Result;
        }

        /// <summary>
        /// Returns a string representation of this instance in RTF format.
        /// </summary>
        public virtual string ToRtfContent(IReadOnlyDictionary<BrushSettings, Brush> brushTable)
        {
            string Result = string.Empty;

            Result += "{\\rtf\\ansi";
            Result += "{\\fonttbl{\\f0 Consolas;}}";

            Result += "{\\colortbl;";
            for (BrushSettings Index = 0; (int)Index < typeof(BrushSettings).GetEnumValues().Length; Index++)
            {
                SolidColorBrush b = brushTable.ContainsKey(Index) ? brushTable[Index] as SolidColorBrush : Brushes.Black;

                Result += $"\\red{b.Color.R}\\green{b.Color.G}\\blue{b.Color.B};";
            }
            Result += @"}";

            Result += "\\f0 \\fs19 \\cf1 \\cb0 \\highlight0";

            string[] Lines = new string[Height];

            for (int i = 0; i < Height; i++)
            {
                int Length;
                for (Length = Width; Length > 0; Length--)
                {
                    PrintableCharacter c = PrintArea[Length - 1, i];
                    if (c != null)
                        break;
                }

                BrushSettings CurrentBrush = BrushSettings.Default;
                string Line = $"\\cf{((int)CurrentBrush) + 1} ";

                for (int j = 0; j < Length; j++)
                {
                    PrintableCharacter Character = PrintArea[j, i];

                    if (Character != null)
                    {
                        if (CurrentBrush != Character.Brush)
                        {
                            CurrentBrush = Character.Brush;
                            Line += $"\\cf{((int)CurrentBrush) + 1} ";
                        }

                        char C = Character.C;

                        if (C <= 0x7F)
                            Line += Character.C;
                        else
                            Line += $"\\u{Convert.ToInt32(C)}?";
                    }
                    else
                        Line += " ";
                }

                Lines[i] = Line;
            }

            foreach (string Line in Lines)
                Result += Line + @"\par ";

            Result += "}";

            return Result;
        }

        /// <summary>
        /// Returns a string representation of this instance in HTML format.
        /// </summary>
        public virtual string ToHtmlContent(IReadOnlyDictionary<BrushSettings, Brush> brushTable)
        {
            string Fragment = ToRawHtmlContent(brushTable);

            int StartHTML = 163;
            int StartFragment = 199;
            int EndFragment = StartFragment + Fragment.Length + 6;
            int EndHTML = EndFragment + 36;

            string StartHTMLText = StartHTML.ToString("D10");
            string StartFragmentText = StartFragment.ToString("D10");
            string EndFragmentText = EndFragment.ToString("D10");
            string EndHTMLText = EndHTML.ToString("D10");

            string Result = $@"
Version:0.9
StartHTML:{StartHTMLText}
EndHTML:{EndHTMLText}
StartFragment:{StartFragmentText}
EndFragment:{EndFragmentText}
SourceURL:https://www.easly.org
<html>
<body>
<!--StartFragment-->{Fragment}<!--EndFragment-->
</body>
</html>
";

            return Result;
        }

        /// <summary>
        /// Returns a string representation of this instance in raw HTML format.
        /// </summary>
        public virtual string ToRawHtmlContent(IReadOnlyDictionary<BrushSettings, Brush> brushTable)
        {
            string Result = string.Empty;

            Result += "<div style=\"margin-top: 10px; line-height: 0; background-color: whitesmoke; border-style: solid; border-color: lightgray; border-width: thin; font-family: Courier\">\n";
            Result += "<div style=\"margin-left: 10px\">\n";
            Result += "<br/>\n";

            string[] Lines = new string[Height];

            for (int i = 0; i < Height; i++)
            {
                int Length;
                for (Length = Width; Length > 0; Length--)
                {
                    PrintableCharacter c = PrintArea[Length - 1, i];
                    if (c != null)
                        break;
                }

                BrushSettings CurrentBrush = BrushSettings.Default;
                string Line = $"<p><span style=\"color: {BrushToColorText(brushTable[CurrentBrush])};\">";

                for (int j = 0; j < Length; j++)
                {
                    PrintableCharacter Character = PrintArea[j, i];

                    if (Character != null)
                    {
                        if (CurrentBrush != Character.Brush)
                        {
                            CurrentBrush = Character.Brush;
                            Line += $"</span><span style=\"color: {BrushToColorText(brushTable[CurrentBrush])};\">";
                        }

                        char C = Character.C;

                        if (C == ' ')
                            Line += "&nbsp;";
                        else if (C <= 0x7F)
                            Line += Character.C;
                        else
                            Line += $"&#{Convert.ToInt32(C)};";
                    }
                    else
                        Line += "&nbsp;";
                }

                Line += "</span></p>";

                Lines[i] = Line;
            }

            foreach (string Line in Lines)
                Result += Line + "\n";

            Result += "<br/>\n";
            Result += "</div>\n";
            Result += "</div>\n";

            return Result;
        }

        private static string BrushToColorText(Brush b)
        {
            if (b is SolidColorBrush AsSolidColorBrush)
            {
                Color Color = AsSolidColorBrush.Color;
                return $"#{Color.R.ToString("X02")}{Color.G.ToString("X02")}{Color.B.ToString("X02")}";
            }
            else
                return "black";
        }

        /// <summary>
        /// Returns a string representation of this instance in markdown-compatible HTML format.
        /// </summary>
        public virtual string ToMarkdownHtmlContent(IReadOnlyDictionary<BrushSettings, Brush> brushTable)
        {
            string Result = string.Empty;

            Result += "<pre>\n";

            string[] Lines = new string[Height];

            for (int i = 0; i < Height; i++)
            {
                int Length;
                for (Length = Width; Length > 0; Length--)
                {
                    PrintableCharacter c = PrintArea[Length - 1, i];
                    if (c != null)
                        break;
                }

                BrushSettings CurrentBrush = BrushSettings.Default;
                string Line = string.Empty;
                string CurrentTag = StartBrushTag(CurrentBrush);

                Line += CurrentTag;

                for (int j = 0; j < Length; j++)
                {
                    PrintableCharacter Character = PrintArea[j, i];

                    if (Character != null)
                    {
                        if (CurrentBrush != Character.Brush)
                        {
                            BrushSettings NewBrush = Character.Brush;
                            string NewTag = StartBrushTag(NewBrush);

                            if (NewTag != CurrentTag)
                            {
                                Line += EndBrushTag(CurrentBrush);
                                Line += NewTag;
                            }

                            CurrentBrush = NewBrush;
                            CurrentTag = NewTag;
                        }

                        char C = Character.C;

                        if (C == ' ')
                            Line += "&nbsp;";
                        else if (C <= 0x7F)
                            Line += Character.C;
                        else
                            Line += $"&#{Convert.ToInt32(C)};";
                    }
                    else
                        Line += "&nbsp;";
                }

                Line += EndBrushTag(CurrentBrush);

                Lines[i] = Line;
            }

            foreach (string Line in Lines)
                Result += Line + "\n";

            Result += "</pre>\n";

            return Result;
        }

        private static string StartBrushTag(BrushSettings brush)
        {
            if (BrushTagTable.ContainsKey(brush))
                return $"<{BrushTagTable[brush]}>";
            else
                return string.Empty;
        }

        private static string EndBrushTag(BrushSettings brush)
        {
            if (BrushTagTable.ContainsKey(brush))
                return $"</{BrushTagTable[brush]}>";
            else
                return string.Empty;
        }

        private static Dictionary<BrushSettings, string> BrushTagTable = new Dictionary<BrushSettings, string>()
        {
            { BrushSettings.Keyword, "b" },
            { BrushSettings.Symbol, "b" },
            { BrushSettings.TypeIdentifier, "i" },
        };
        #endregion
    }
}
