namespace EaslyController.Frame
{
    /// <summary>
    /// A state view that can be replaced.
    /// </summary>
    public interface IFrameReplaceableStateView
    {
        /// <summary>
        /// Replaces the cell view for the given property.
        /// </summary>
        /// <param name="propertyName">The property name.</param>
        /// <param name="cellView">The new cell view.</param>
        void ReplaceCellView(string propertyName, IFrameContainerCellView cellView);
    }
}
