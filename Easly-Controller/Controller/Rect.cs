namespace EaslyController.Controller
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// Implements a structure that is used to describe a rectangular region.
    /// </summary>
    public struct Rect : IFormattable
    {
        #region Init
        static Rect()
        {
            Empty = new Rect(0, 0, 0, 0);
        }

        /// <summary>
        /// The empty (0,0,0,0) region.
        /// </summary>
        public static Rect Empty { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Rect"/> struct.
        /// </summary>
        /// <param name="x">The horizontal coordinate of the top left corner.</param>
        /// <param name="y">The vertical coordinate of the top left corner.</param>
        /// <param name="width">The region width.</param>
        /// <param name="height">The region height.</param>
        public Rect(double x, double y, double width, double height)
        {
            Debug.Assert(x >= 0 && y >= 0 && ((width >= 0 && height >= 0) || (double.IsNaN(width) && height >= 0) || (width >= 0 && double.IsNaN(height))));

            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Rect"/> struct.
        /// </summary>
        /// <param name="origin">The location of the top left corner.</param>
        /// <param name="size">The region size.</param>
        public Rect(Point origin, Size size)
        {
            Debug.Assert(origin.X.Draw >= 0 && origin.Y.Draw >= 0 && ((size.Width.Draw >= 0 && size.Height.Draw >= 0) || (double.IsNaN(size.Width.Draw) && size.Height.Draw >= 0) || (size.Width.Draw >= 0 && double.IsNaN(size.Height.Draw))));

            X = origin.X.Draw;
            Y = origin.Y.Draw;
            Width = size.Width.Draw;
            Height = size.Height.Draw;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Horizontal coordinate of the top left corner.
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Vertical coordinate of the top left corner.
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// Origin of the top left corner.
        /// </summary>
        public Point Origin { get { return new Point(new Measure() { Draw = X }, new Measure() { Draw = Y }); } }

        /// <summary>
        /// The region Width.
        /// </summary>
        public double Width { get; }

        /// <summary>
        /// The region Height.
        /// </summary>
        public double Height { get; }

        /// <summary>
        /// The region size
        /// </summary>
        public Size Size { get { return new Size(new Measure() { Draw = Width }, new Measure() { Draw = Height }); } }

        /// <summary>
        /// True if the object is the empty region.
        /// </summary>
        public bool IsEmpty { get { return X == 0 && Y == 0 && Width == 0 && Height == 0; } }

        /// <summary>
        /// True if the object represents a visible region.
        /// </summary>
        public bool IsVisible { get { return Width > 0 && Height > 0; } }

        /// <summary>
        /// Location of the rectangle center.
        /// </summary>
        public Point Center { get { return new Point(new Measure() { Draw = X + (Width / 2) }, new Measure() { Draw = Y + (Height / 2) }); } }
        #endregion

        #region Client Interface
        /// <summary>
        /// Checks if a point is within the rectangular region.
        /// </summary>
        /// <param name="x">X-coordinate of the point to check.</param>
        /// <param name="y">Y-coordinate of the point to check.</param>
        public bool IsPointInRect(double x, double y)
        {
            return x >= X && x < X + Width && y >= Y && y < Y + Height;
        }

        /// <summary>
        /// Returns the union of the visible surface of two rectangles.
        /// </summary>
        /// <param name="rect1">The first rectangle.</param>
        /// <param name="rect2">The second rectangle.</param>
        public static Rect VisibleUnion(Rect rect1, Rect rect2)
        {
            Debug.Assert(RegionHelper.IsFixed(rect1));
            Debug.Assert(RegionHelper.IsFixed(rect2));

            GetUnionX(rect1, rect2, out double UnionX, out double UnionCX);
            GetUnionY(rect1, rect2, out double UnionY, out double UnionCY);

            return new Rect(UnionX, UnionY, UnionCX - UnionX, UnionCY - UnionY);
        }

        private static void GetUnionX(Rect rect1, Rect rect2, out double unionX, out double unionCX)
        {
            if (rect1.Width > 0 && rect2.Width > 0)
            {
                unionX = rect1.X < rect2.X ? rect1.X : rect2.X;
                unionCX = rect1.X + rect1.Width > rect2.X + rect2.Width ? rect1.X + rect1.Width : rect2.X + rect2.Width;
            }
            else if (rect1.Width > 0)
            {
                unionX = rect1.X;
                unionCX = rect1.X + rect1.Width;
            }
            else if (rect2.Width > 0)
            {
                unionX = rect2.X;
                unionCX = rect2.X + rect2.Width;
            }
            else
            {
                unionX = rect1.X < rect2.X ? rect1.X : rect2.X;
                unionCX = unionX;
            }
        }

        private static void GetUnionY(Rect rect1, Rect rect2, out double unionY, out double unionCY)
        {
            if (rect1.Height > 0 && rect2.Height > 0)
            {
                unionY = rect1.Y < rect2.Y ? rect1.Y : rect2.Y;
                unionCY = rect1.Y + rect1.Height > rect2.Y + rect2.Height ? rect1.Y + rect1.Height : rect2.Y + rect2.Height;
            }
            else if (rect1.Height > 0)
            {
                unionY = rect1.Y;
                unionCY = rect1.Y + rect1.Height;
            }
            else if (rect2.Height > 0)
            {
                unionY = rect2.Y;
                unionCY = rect2.Y + rect2.Height;
            }
            else
            {
                unionY = rect1.Y < rect2.Y ? rect1.Y : rect2.Y;
                unionCY = unionY;
            }
        }

        /// <summary>
        /// Compares two regions.
        /// </summary>
        /// <param name="rect1">The first region.</param>
        /// <param name="rect2">The second region.</param>
        public static bool IsEqual(Rect rect1, Rect rect2)
        {
            double DiffX = Math.Abs(rect2.X - rect1.X);
            double DiffY = Math.Abs(rect2.Y - rect1.Y);
            double DiffCX = Math.Abs(rect2.Width - rect1.Width);
            double DiffCY = Math.Abs(rect2.Height - rect1.Height);

            return RegionHelper.IsZero(DiffX) && RegionHelper.IsZero(DiffY) && RegionHelper.IsZero(DiffCX) && RegionHelper.IsZero(DiffCY);
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
#nullable disable
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return $"{X},{Y},{Width},{Height}";
        }
#nullable restore
        #endregion
    }
}
