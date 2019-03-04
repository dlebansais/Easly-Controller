namespace EaslyController.Layout
{
    using EaslyController.Controller;

    /// <summary>
    /// Frames that have associated cells that can be displayed.
    /// </summary>
    public interface ILayoutPrintableFrame : ILayoutFrame
    {
        /// <summary>
        /// Prints a cell created with this frame.
        /// </summary>
        /// <param name="printContext">The context used to print the cell.</param>
        /// <param name="cellView">The cell to print.</param>
        /// <param name="origin">The location where to start printing.</param>
        /// <param name="size">The printing size, padding included.</param>
        /// <param name="padding">The padding to use when printing.</param>
        void Print(ILayoutPrintContext printContext, ILayoutCellView cellView, Point origin, Size size, Padding padding);
    }
}
