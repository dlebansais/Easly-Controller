namespace EaslyController.Focus
{
    /// <summary>
    /// Base interface for the state of a node.
    /// </summary>
    public interface IFocusCyclableNodeState : IFocusNodeState
    {
        /// <summary>
        /// List of node indexes that can replace the current node. Can be null.
        /// Applies only to bodies and features.
        /// </summary>
        FocusInsertionChildNodeIndexList CycleIndexList { get; }

        /// <summary>
        /// Position of the current node in <see cref="CycleIndexList"/>.
        /// </summary>
        int CycleCurrentPosition { get; }

        /// <summary>
        /// Initializes the cycle index list if not already initialized.
        /// </summary>
        void InitializeCycleIndexList();

        /// <summary>
        /// Updates the position of the node in the cycle.
        /// </summary>
        void UpdateCyclePosition();

        /// <summary>
        /// Restores the cycle index list from which this state was created.
        /// </summary>
        /// <param name="cycleIndexList">The list to restore.</param>
        void RestoreCycleIndexList(FocusInsertionChildNodeIndexList cycleIndexList);
    }
}
