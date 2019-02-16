namespace EaslyDraw
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Media;

    /// <summary>
    /// A geometry parameterized to scale at the edges.
    /// </summary>
    public class ScalableGeometry
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="ScalableGeometry"/> class.
        /// </summary>
        /// <param name="figure">The path figure that constitutes the geometry.</param>
        /// <param name="bounds">The region to scale within.</param>
        /// <param name="isWidthScaled">True if the goemetry must be scaled horizontally.</param>
        /// <param name="isHeightScaled">True if the goemetry must be scaled vertically.</param>
        public ScalableGeometry(PathFigure figure, Rect bounds, bool isWidthScaled, bool isHeightScaled)
        {
            Figure = figure;
            Bounds = bounds;

            if (isWidthScaled)
                WidthScale = new GeometryScale(bounds.Left < 0 ? -bounds.Left : 0, bounds.Right > 1 ? bounds.Right - 1 : 0);
            else
                WidthScale = null;

            if (isHeightScaled)
                HeightScale = new GeometryScale(bounds.Top < 0 ? -bounds.Top : 0, bounds.Bottom > 1 ? bounds.Bottom - 1 : 0);
            else
                HeightScale = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScalableGeometry"/> class.
        /// </summary>
        /// <param name="glyphGeometry">The base geometry.</param>
        /// <param name="bounds">The region to scale within.</param>
        /// <param name="isWidthScaled">True if the width should be scaled.</param>
        /// <param name="leftPercent">The fraction of the left side to scale if <paramref name="isWidthScaled"/> is true.</param>
        /// <param name="rightPercent">The fraction of the right side to scale if <paramref name="isWidthScaled"/> is true.</param>
        /// <param name="isHeightScaled">True if the height should be scaled.</param>
        /// <param name="topPercent">The fraction of the top side to scale if <paramref name="isHeightScaled"/> is true.</param>
        /// <param name="bottomPercent">The fraction of the bottom side to scale if <paramref name="isHeightScaled"/> is true.</param>
        public ScalableGeometry(Geometry glyphGeometry, Rect bounds, bool isWidthScaled, double leftPercent, double rightPercent, bool isHeightScaled, double topPercent, double bottomPercent)
        {
            PathFigure GlyphFigure;
            if (GeometryToFigure(glyphGeometry, out GlyphFigure))
            {
                Bounds = bounds;

                if (isWidthScaled)
                {
                    double GlyphWidth = bounds.Width;
                    WidthScale = new GeometryScale(GlyphWidth * leftPercent, GlyphWidth * rightPercent);
                }
                else
                    WidthScale = null;

                if (isHeightScaled)
                {
                    double GlyphHeight = bounds.Height;
                    HeightScale = new GeometryScale(GlyphHeight * topPercent, GlyphHeight * bottomPercent);
                }
                else
                    HeightScale = null;

                if (WidthScale != null || HeightScale != null)
                    Figure = PrescaledPathFigure(GlyphFigure, bounds, WidthScale, HeightScale);
                else
                {
                    Figure = GlyphFigure;
                    HeightScale = null;
                }
            }
            else
            {
                Figure = null;
                WidthScale = null;
                HeightScale = null;
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// The path figure that constitutes the geometry.
        /// </summary>
        public PathFigure Figure { get; private set; }

        /// <summary>
        /// The region to scale within.
        /// </summary>
        public Rect Bounds { get; private set; }

        /// <summary>
        /// True if the width is scaled.
        /// </summary>
        public bool IsWidthScaled { get { return WidthScale != null; } }

        /// <summary>
        /// True if the height is scaled.
        /// </summary>
        public bool IsHeightScaled { get { return HeightScale != null; } }
        #endregion

        #region Figures
        private bool GeometryToFigure(Geometry geometry, out PathFigure figure)
        {
            GeometryGroup AsGeometryGroup;
            PathGeometry AsPathGeometry;

            if ((AsGeometryGroup = geometry as GeometryGroup) != null)
                return GeometryToFigure(AsGeometryGroup.Children[0], out figure);

            else if ((AsPathGeometry = geometry as PathGeometry) != null)
            {
                figure = AsPathGeometry.Figures[0];
                return true;
            }

            else
            {
                figure = null;
                return false;
            }
        }

        private static Rect CalculateEnvelope(PathFigure figure)
        {
            Rect Envelope = new Rect(figure.StartPoint, figure.StartPoint);

            foreach (PathSegment Segment in figure.Segments)
            {
                LineSegment AsLineSegment;
                PolyLineSegment AsPolyLineSegment;
                BezierSegment AsBezierSegment;
                PolyBezierSegment AsPolyBezierSegment;

                if ((AsLineSegment = Segment as LineSegment) != null)
                    EnlargeEnvelope(AsLineSegment.Point, ref Envelope);

                else if ((AsPolyLineSegment = Segment as PolyLineSegment) != null)
                    EnlargeEnvelope(AsPolyLineSegment.Points, ref Envelope);

                else if ((AsBezierSegment = Segment as BezierSegment) != null)
                {
                    EnlargeEnvelope(AsBezierSegment.Point1, ref Envelope);
                    EnlargeEnvelope(AsBezierSegment.Point2, ref Envelope);
                    EnlargeEnvelope(AsBezierSegment.Point3, ref Envelope);
                }

                else if ((AsPolyBezierSegment = Segment as PolyBezierSegment) != null)
                    EnlargeEnvelope(AsPolyBezierSegment.Points, ref Envelope);
            }

            return Envelope;
        }

        private static void EnlargeEnvelope(IList<Point> pointList, ref Rect envelope)
        {
            foreach (Point point in pointList)
                EnlargeEnvelope(point, ref envelope);
        }

        private static void EnlargeEnvelope(Point point, ref Rect envelope)
        {
            if (envelope.Left > point.X)
                envelope = new Rect(new Point(point.X, envelope.Top), new Point(envelope.Right, envelope.Bottom));

            if (envelope.Top > point.Y)
                envelope = new Rect(new Point(envelope.Left, point.Y), new Point(envelope.Right, envelope.Bottom));

            if (envelope.Right < point.X)
                envelope = new Rect(new Point(envelope.Left, envelope.Top), new Point(point.X, envelope.Bottom));

            if (envelope.Bottom < point.Y)
                envelope = new Rect(new Point(envelope.Left, envelope.Top), new Point(envelope.Right, point.Y));
        }
        #endregion

        #region Prescaling
        private static PathFigure PrescaledPathFigure(PathFigure figure, Rect bounds, GeometryScale widthScale, GeometryScale heightScale)
        {
            List<PathSegment> SegmentList = new List<PathSegment>();
            foreach (PathSegment Item in figure.Segments)
                SegmentList.Add(PrescaledPathSegment(Item, bounds, widthScale, heightScale));

            return new PathFigure(PrescaledPoint(figure.StartPoint, bounds, widthScale, heightScale), SegmentList, figure.IsClosed);
        }

        private static PathSegment PrescaledPathSegment(PathSegment segment, Rect bounds, GeometryScale widthScale, GeometryScale heightScale)
        {
            LineSegment AsLineSegment;
            PolyLineSegment AsPolyLineSegment;
            BezierSegment AsBezierSegment;
            PolyBezierSegment AsPolyBezierSegment;

            if ((AsLineSegment = segment as LineSegment) != null)
                return new LineSegment(PrescaledPoint(AsLineSegment.Point, bounds, widthScale, heightScale), AsLineSegment.IsStroked);

            else if ((AsPolyLineSegment = segment as PolyLineSegment) != null)
                return new PolyLineSegment(PrescaledPointList(AsPolyLineSegment.Points, bounds, widthScale, heightScale), AsPolyLineSegment.IsStroked);

            else if ((AsBezierSegment = segment as BezierSegment) != null)
                return new BezierSegment(PrescaledPoint(AsBezierSegment.Point1, bounds, widthScale, heightScale), PrescaledPoint(AsBezierSegment.Point2, bounds, widthScale, heightScale), PrescaledPoint(AsBezierSegment.Point3, bounds, widthScale, heightScale), AsBezierSegment.IsStroked);

            else if ((AsPolyBezierSegment = segment as PolyBezierSegment) != null)
                return new PolyBezierSegment(PrescaledPointList(AsPolyBezierSegment.Points, bounds, widthScale, heightScale), AsPolyBezierSegment.IsStroked);

            else
                return segment.Clone();
        }

        private static IList<Point> PrescaledPointList(IList<Point> pointList, Rect bounds, GeometryScale widthScale, GeometryScale heightScale)
        {
            List<Point> Result = new List<Point>();

            foreach (Point point in pointList)
                Result.Add(PrescaledPoint(point, bounds, widthScale, heightScale));

            return Result;
        }

        private static Point PrescaledPoint(Point point, Rect bounds, GeometryScale widthScale, GeometryScale heightScale)
        {
            double X = point.X;
            double Y = point.Y;

            if (widthScale != null)
            {
                double Width = bounds.Width;

                X -= bounds.Top;

                if (X < widthScale.Low)
                    X = X - widthScale.Low;

                else if (X > Width - widthScale.High)
                    X = X - (Width - widthScale.High) + 1;

                else
                    X = (X - widthScale.Low) / (Width - widthScale.High - widthScale.Low);
            }

            if (heightScale != null)
            {
                double Height = bounds.Height;

                Y -= bounds.Top;

                if (Y < heightScale.Low)
                    Y = Y - heightScale.Low;

                else if (Y > Height - heightScale.High)
                    Y = Y - (Height - heightScale.High) + 1;

                else
                    Y = (Y - heightScale.Low) / (Height - heightScale.High - heightScale.Low);
            }

            return new Point(X, Y);
        }
        #endregion

        #region Final Scaling
        /// <summary>
        /// Returns the geometry scaled to the provided size.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public Geometry Scaled(double width, double height)
        {
            StreamGeometry Result = new StreamGeometry();

            GeometryScale ScaledWidth = (WidthScale != null) ? new GeometryScale(WidthScale, width) : null;
            GeometryScale ScaledHeight = (HeightScale != null) ? new GeometryScale(HeightScale, height) : null;

            using (StreamGeometryContext sgc = Result.Open())
            {
                sgc.BeginFigure(ScaledPoint(Figure.StartPoint, ScaledWidth, ScaledHeight), Figure.IsFilled, Figure.IsClosed);

                foreach (PathSegment Segment in Figure.Segments)
                {
                    LineSegment AsLineSegment;
                    PolyLineSegment AsPolyLineSegment;
                    BezierSegment AsBezierSegment;
                    PolyBezierSegment AsPolyBezierSegment;

                    if ((AsLineSegment = Segment as LineSegment) != null)
                        sgc.LineTo(ScaledPoint(AsLineSegment.Point, ScaledWidth, ScaledHeight), AsLineSegment.IsStroked, AsLineSegment.IsSmoothJoin);

                    else if ((AsPolyLineSegment = Segment as PolyLineSegment) != null)
                        sgc.PolyLineTo(ScaledPointList(AsPolyLineSegment.Points, ScaledWidth, ScaledHeight), AsPolyLineSegment.IsStroked, AsPolyLineSegment.IsSmoothJoin);

                    else if ((AsBezierSegment = Segment as BezierSegment) != null)
                        sgc.BezierTo(ScaledPoint(AsBezierSegment.Point1, ScaledWidth, ScaledHeight), ScaledPoint(AsBezierSegment.Point2, ScaledWidth, ScaledHeight), ScaledPoint(AsBezierSegment.Point3, ScaledWidth, ScaledHeight), AsBezierSegment.IsStroked, AsBezierSegment.IsSmoothJoin);

                    else if ((AsPolyBezierSegment = Segment as PolyBezierSegment) != null)
                        sgc.PolyBezierTo(ScaledPointList(AsPolyBezierSegment.Points, ScaledWidth, ScaledHeight), AsPolyBezierSegment.IsStroked, AsPolyBezierSegment.IsSmoothJoin);
                }
            }

            return Result;
        }

        private static List<Point> ScaledPointList(IList<Point> pointList, GeometryScale scaledWidth, GeometryScale scaledHeight)
        {
            List<Point> Result = new List<Point>();
            foreach (Point Point in pointList)
                Result.Add(ScaledPoint(Point, scaledWidth, scaledHeight));

            return Result;
        }

        private static Point ScaledPoint(Point point, GeometryScale scaledWidth, GeometryScale scaledHeight)
        {
            double X = point.X;
            double Y = point.Y;

            if (scaledWidth != null)
            {
                if (X < 0)
                    X = X + scaledWidth.Low;

                else if (X > 1)
                    X = X + scaledWidth.Scale - scaledWidth.High - 1;

                else
                    X = (X * (scaledWidth.Scale - scaledWidth.Low - scaledWidth.High)) + scaledWidth.Low;
            }

            if (scaledHeight != null)
            {
                if (Y < 0)
                    Y = Y + scaledHeight.Low;

                else if (Y > 1)
                    Y = Y + scaledHeight.Scale - scaledHeight.High - 1;

                else
                    Y = (Y * (scaledHeight.Scale - scaledHeight.Low - scaledHeight.High)) + scaledHeight.Low;
            }

            return new Point(X, Y);
        }

        private GeometryScale WidthScale;
        private GeometryScale HeightScale;
        #endregion
    }
}
