namespace EaslyController.Controller
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// Implements a structure that is used to describe a 2-dimensional size of displayed characters.
    /// </summary>
    public struct Plane : IFormattable
    {
        #region Init
        static Plane()
        {
            Empty = new Plane(0, 0);
        }

        /// <summary>
        /// The empty (0,0) size.
        /// </summary>
        public static Plane Empty { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Plane"/> struct.
        /// Use -1 for a floating value.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public Plane(int width, int height)
        {
            Debug.Assert(width >= -1);
            Debug.Assert(height >= -1);

            Width = width;
            Height = height;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Width.
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// Height.
        /// </summary>
        public int Height { get; }

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
        /// Compares two planes.
        /// </summary>
        /// <param name="plane1">The first plane.</param>
        /// <param name="plane2">The second plane.</param>
        public static bool IsEqual(Plane plane1, Plane plane2)
        {
            return plane1.Width == plane2.Width && plane1.Height == plane2.Height;
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
