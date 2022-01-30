namespace EaslyController.Layout
{
    using EaslyController.Focus;

    /// <summary>
    /// Selects specific frames in the remaining of the cell view tree.
    /// </summary>
    public interface ILayoutFrameSelector : IFocusFrameSelector
    {
    }

    /// <summary>
    /// Selects specific frames in the remaining of the cell view tree.
    /// </summary>
    public class LayoutFrameSelector : FocusFrameSelector, ILayoutFrameSelector
    {
        #region Init
        /// <summary>
        /// Gets the empty <see cref="LayoutFrameSelector"/> object.
        /// </summary>
        public static new LayoutFrameSelector Empty { get; } = new LayoutFrameSelector();
        #endregion
    }
}
