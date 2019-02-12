namespace EaslyController.Controller
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// Implements a structure that is used to describe the padding around a rectangle.
    /// </summary>
    public struct Padding : IFormattable
    {
        #region Init
        static Padding()
        {
            Empty = new Padding(0, 0, 0, 0);
        }

        /// <summary>
        /// The empty (0,0,0,0) padding.
        /// </summary>
        public static Padding Empty { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Padding"/> struct.
        /// </summary>
        /// <param name="left">The left padding.</param>
        /// <param name="top">The top padding.</param>
        /// <param name="right">The right padding.</param>
        /// <param name="bottom">The botton padding.</param>
        public Padding(double left, double top, double right, double bottom)
        {
            Debug.Assert(left >= 0);
            Debug.Assert(top >= 0);
            Debug.Assert(right >= 0);
            Debug.Assert(bottom >= 0);

            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }
        #endregion

        #region Properties
        /// <summary>
        /// The left padding.
        /// </summary>
        public double Left { get; set; }

        /// <summary>
        /// The top padding.
        /// </summary>
        public double Top { get; set; }

        /// <summary>
        /// The right padding.
        /// </summary>
        public double Right { get; set; }

        /// <summary>
        /// The bottom padding.
        /// </summary>
        public double Bottom { get; set; }

        /// <summary>
        /// True if the object is the empty padding.
        /// </summary>
        public bool IsEmpty { get { return Left == 0 && Top == 0 && Right == 0 && Bottom == 0; } }
        #endregion

        #region Client Interface
        /// <summary>
        /// Compares two sizes.
        /// </summary>
        /// <param name="padding1">The first padding.</param>
        /// <param name="padding2">The second padding.</param>
        public static bool IsEqual(Padding padding1, Padding padding2)
        {
            return padding1.Left == padding2.Left && padding1.Top == padding2.Top && padding1.Right == padding2.Right && padding1.Bottom == padding2.Bottom;
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
            return $"{Left},{Top},{Right},{Bottom}";
        }
        #endregion
    }
}
