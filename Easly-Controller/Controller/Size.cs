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
            Empty = new Size(Measure.Zero, Measure.Zero);
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
        public Size(Measure width, Measure height)
        {
            Debug.Assert(width.IsFloating || width.IsPositive);
            Debug.Assert(height.IsFloating || height.IsPositive);

            Width = width;
            Height = height;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Width.
        /// </summary>
        public Measure Width { get; }

        /// <summary>
        /// Height.
        /// </summary>
        public Measure Height { get; }

        /// <summary>
        /// True if the object is the empty size.
        /// </summary>
        public bool IsEmpty { get { return Width.IsZero && Height.IsZero; } }

        /// <summary>
        /// True if the object represents a visible region.
        /// </summary>
        public bool IsVisible { get { return Width.IsStrictlyPositive && Height.IsStrictlyPositive; } }
        #endregion

        #region Client Interface
        /// <summary>
        /// Compares two sizes. Stretched sizes are never equal, even to themselves.
        /// </summary>
        /// <param name="size1">The first size.</param>
        /// <param name="size2">The second size.</param>
        public static bool IsEqual(Size size1, Size size2)
        {
            double DiffCX = size1.Width.IsFloating && size2.Width.IsFloating ? 0 : Math.Abs((size2.Width - size1.Width).Draw);
            double DiffCY = size1.Height.IsFloating && size2.Height.IsFloating ? 0 : Math.Abs((size2.Height - size1.Height).Draw);
            Debug.Assert(!double.IsNaN(DiffCX) && !double.IsNaN(DiffCY));

            return RegionHelper.IsZero(DiffCX) && RegionHelper.IsZero(DiffCY);
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
