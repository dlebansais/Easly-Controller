namespace EaslyDraw
{
    /// <summary>
    /// Type of scaling to apply to a geometry.
    /// </summary>
    public enum GeometryScalings
    {
        /// <summary>
        /// No scaling.
        /// </summary>
        None,

        /// <summary>
        /// Stretch all the way.
        /// </summary>
        Stretch,

        /// <summary>
        /// Stretch assuming the geometry is from a font.
        /// </summary>
        Font,
    }
}
