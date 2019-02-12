namespace EaslyController.Controller
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// Implements a structure that is used to describe the size of an object.
    /// </summary>
    public struct Size : IFormattable
    {
        #region Init
        static Size()
        {
            Empty = new Size(0, 0);
        }

        /// <summary>
        /// The empty (0,0) size.
        /// </summary>
        public static Size Empty { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Size"/> struct.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public Size(double width, double height)
        {
            Debug.Assert(double.IsNaN(width) || width >= 0);
            Debug.Assert(double.IsNaN(height) || height >= 0);

            Width = width;
            Height = height;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Width.
        /// </summary>
        public double Width { get; }

        /// <summary>
        /// Height.
        /// </summary>
        public double Height { get; }

        /// <summary>
        /// True if the object is the empty size.
        /// </summary>
        public bool IsEmpty { get { return Width == 0 && Height == 0; } }

        /// <summary>
        /// True if the object represents a visible region.
        /// </summary>
        public bool IsVisible { get { return Width > 0 && Height > 0; } }
        #endregion

        #region Client Interface
        /// <summary>
        /// Compares two sizes. Stretched sizes are never equal, even to themselves.
        /// </summary>
        /// <param name="size1">The first size.</param>
        /// <param name="size2">The second size.</param>
        public static bool IsEqual(Size size1, Size size2)
        {
            return size1.Width == size2.Width && size1.Height == size2.Height;
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
            return $"{Width},{Height}";
        }
        #endregion
    }
}
