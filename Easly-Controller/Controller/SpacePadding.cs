namespace EaslyController.Controller
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// Implements a structure that is used to describe the padding around a rectangle.
    /// </summary>
    public struct SpacePadding : IFormattable
    {
        #region Init
        static SpacePadding()
        {
            Empty = new SpacePadding(0, 0);
        }

        /// <summary>
        /// The empty (0,0) padding.
        /// </summary>
        public static SpacePadding Empty { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpacePadding"/> struct.
        /// </summary>
        /// <param name="left">The left padding.</param>
        /// <param name="right">The right padding.</param>
        public SpacePadding(int left, int right)
        {
            Debug.Assert(left >= 0);
            Debug.Assert(right >= 0);

            Left = left;
            Right = right;
        }
        #endregion

        #region Properties
        /// <summary>
        /// The left padding.
        /// </summary>
        public int Left { get; set; }

        /// <summary>
        /// The right padding.
        /// </summary>
        public int Right { get; set; }

        /// <summary>
        /// True if the object is the empty padding.
        /// </summary>
        public bool IsEmpty { get { return Left == 0 && Right == 0; } }
        #endregion

        #region Client Interface
        /// <summary>
        /// Compares two sizes.
        /// </summary>
        /// <param name="padding1">The first padding.</param>
        /// <param name="padding2">The second padding.</param>
        public static bool IsEqual(SpacePadding padding1, SpacePadding padding2)
        {
            return padding1.Left == padding2.Left && padding1.Right == padding2.Right;
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
            return $"{Left},{Right}";
        }
        #endregion
    }
}
