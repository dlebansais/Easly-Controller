namespace EaslyController.Layout
{
    using EaslyController.Controller;

    /// <summary>
    /// Frames that have associated cells that can be displayed.
    /// </summary>
    public interface ILayoutDrawableFrame : ILayoutFrame
    {
        /// <summary>
        /// Draws a cell created with this frame.
        /// </summary>
        /// <param name="drawContext">The context used to draw the cell.</param>
        /// <param name="cellView">The cell to draw.</param>
        /// <param name="origin">The location where to start drawing.</param>
        /// <param name="size">The drawing size, padding included.</param>
        /// <param name="padding">The padding to use when drawing.</param>
        void Draw(ILayoutDrawContext drawContext, ILayoutCellView cellView, Point origin, Size size, Padding padding);
    }
}
