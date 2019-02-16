namespace EaslyController.Frame
{
    /// <summary>
    /// A state view that can be replaced.
    /// </summary>
    public interface IFrameReplaceableStateView
    {
        /// <summary>
        /// Assign the cell view corresponding to an inner.
        /// </summary>
        /// <param name="propertyName">The property name of the inner.</param>
        /// <param name="cellView">The assigned cell view.</param>
        void AssignCellViewTable(string propertyName, IFrameAssignableCellView cellView);
    }
}
