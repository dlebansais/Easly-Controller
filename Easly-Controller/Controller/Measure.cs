namespace EaslyController.Controller
{
    /// <summary>
    /// A measure used for drawing and printing.
    /// </summary>
    public struct Measure
    {
        #region Init
        /// <summary>
        /// The zero value.
        /// </summary>
        public static Measure Zero = default;

        /// <summary>
        /// The floating value.
        /// </summary>
        public static Measure Floating = new Measure() { Draw = double.NaN, Print = -1 };
        #endregion

        #region Properties
        /// <summary>
        /// The value for drawing.
        /// </summary>
        public double Draw;

        /// <summary>
        /// The value for printing.
        /// </summary>
        public int Print;

        /// <summary>
        /// True if this measure is the zero value.
        /// </summary>
        public bool IsZero { get { return Draw == 0; } }

        /// <summary>
        /// True if this measure is floating.
        /// </summary>
        public bool IsFloating { get { return double.IsNaN(Draw); } }

        /// <summary>
        /// True if this measure is zero or above.
        /// </summary>
        public bool IsPositive { get { return Draw >= 0; } }

        /// <summary>
        /// True if this measure is above zero.
        /// </summary>
        public bool IsStrictlyPositive { get { return Draw > 0; } }
        #endregion

        #region Operators
        /// <summary>
        /// Adds two measures.
        /// </summary>
        /// <param name="measure1">The first measure.</param>
        /// <param name="measure2">The second measure.</param>
        public static Measure operator +(Measure measure1, Measure measure2)
        {
            return new Measure() { Draw = measure1.Draw + measure2.Draw, Print = measure1.Print + measure2.Print };
        }

        /// <summary>
        /// Substract two measures.
        /// </summary>
        /// <param name="measure1">The first measure.</param>
        /// <param name="measure2">The second measure.</param>
        public static Measure operator -(Measure measure1, Measure measure2)
        {
            return new Measure() { Draw = measure1.Draw - measure2.Draw, Print = measure1.Print - measure2.Print };
        }

        /// <summary>
        /// Multiplies two measures.
        /// </summary>
        /// <param name="measure1">The first measure.</param>
        /// <param name="measure2">The second measure.</param>
        public static Measure operator *(Measure measure1, Measure measure2)
        {
            return new Measure() { Draw = measure1.Draw * measure2.Draw, Print = measure1.Print * measure2.Print };
        }
        #endregion

        #region Debugging
        /// <summary></summary>
        public override string ToString()
        {
            return Draw.ToString();
        }
        #endregion
    }
}
