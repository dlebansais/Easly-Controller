namespace EaslyController.Frame
{
    /// <summary>
    /// Cell view that can be assigned to a property in a state view.
    /// </summary>
    public interface IFrameAssignableCellView : IEqualComparable
    {
        /// <summary>
        /// True if the cell is assigned to a property in a cell view table.
        /// </summary>
        bool IsAssignedToTable { get; }

        /// <summary>
        /// Indicates that the cell view is assigned to a property in a cell view table.
        /// </summary>
        void AssignToCellViewTable();
    }
}
