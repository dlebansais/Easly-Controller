namespace EaslyDraw
{
    /// <summary>
    /// A geometry made with characters.
    /// </summary>
    public class PrintableGeometry
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PrintableGeometry"/> class.
        /// </summary>
        /// <param name="single">Character to use if the geometry is scaled to one character.</param>
        /// <param name="tip1">Character for the tip of the geometry (left or top).</param>
        /// <param name="tip2">Character for the tip of the geometry (right or bottom).</param>
        /// <param name="middle">Middle character, 0 if none.</param>
        /// <param name="line">Character used to draw a line.</param>
        /// <param name="orientation">Orientation of the geometry.</param>
        public PrintableGeometry(char single, char tip1, char tip2, char middle, char line, PrintOrientations orientation)
        {
            Single = single;
            Tip1 = tip1;
            Tip2 = tip2;
            Middle = middle;
            Line = line;
            Orientation = orientation;
        }

        /// <summary>
        /// Character to use if the geometry is scaled to one character.
        /// </summary>
        public char Single { get; }

        /// <summary>
        /// Character for the tip of the geometry (left or top).
        /// </summary>
        public char Tip1 { get; }

        /// <summary>
        /// Character for the tip of the geometry (right or bottom).
        /// </summary>
        public char Tip2 { get; }

        /// <summary>
        /// Middle character, 0 if none.
        /// </summary>
        public char Middle { get; }

        /// <summary>
        /// Character used to draw a line.
        /// </summary>
        public char Line { get; }

        /// <summary>
        /// Orientation of the geometry.
        /// </summary>
        public PrintOrientations Orientation { get; }

        /// <summary>
        /// Returns the characters corresponding to this geometry scaled as requested.
        /// </summary>
        /// <param name="width">The scaling width, ignored for a vertical geometry.</param>
        /// <param name="height">The scaling height, ignored for an horizontal geometry.</param>
        public virtual char[] Scale(int width, int height)
        {
            int Length = Orientation == PrintOrientations.Horizontal ? width : height;
            char[] Result = new char[Length];

            if (Length == 0)
            { }

            else if (Length == 1)
                Result[0] = Single;

            else if (Length == 2)
            {
                Result[0] = Tip1;
                Result[1] = Tip2;
            }

            else if (Length == 3)
            {
                Result[0] = Tip1;
                Result[1] = Middle;
                Result[2] = Tip2;
            }

            else
            {
                Result[0] = Tip1;
                Result[Length / 2] = Middle;
                Result[Length - 1] = Tip2;

                for (int i = 1; i + 1 < Length; i++)
                    if (i != Length / 2)
                        Result[i] = Line;
            }

            return Result;
        }
    }
}
