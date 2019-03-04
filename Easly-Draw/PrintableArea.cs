namespace EaslyDraw
{
    using System;

    /// <summary>
    /// An area where to print characters with a brush.
    /// </summary>
    public class PrintableArea : IFormattable
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
        public override string ToString()
        {
            return ToString(null, null);
        }

        /// <summary>
        /// Returns a formatted string representation of this instance.
        /// </summary>
        /// <param name="provider">A format provider.</param>
        public string ToString(IFormatProvider provider)
        {
            return ToString(null, provider);
        }

        /// <summary>
        /// Returns a formatted string representation of this instance.
        /// </summary>
        /// <param name="format">A format.</param>
        /// <param name="formatProvider">A format provider.</param>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (format == "rtf")
                return ToRtfString(formatProvider);
            else
                return ToBasicString(formatProvider);
        }
        #endregion

        #region Implementation
        private string ToBasicString(IFormatProvider formatProvider)
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

            string Result = string.Empty;

            foreach (string Line in Lines)
                Result += Line + "\r\n";

            return Result;
        }

        private string ToRtfString(IFormatProvider formatProvider)
        {
            return ToBasicString(formatProvider);
        }
        #endregion
    }
}
