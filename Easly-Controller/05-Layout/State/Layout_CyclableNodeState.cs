namespace EaslyController.Layout
{
    using EaslyController.Focus;

    /// <summary>
    /// Base interface for the state of a node.
    /// </summary>
    public interface ILayoutCyclableNodeState : IFocusCyclableNodeState, ILayoutNodeState
    {
        /// <summary>
        /// List of node indexes that can replace the current node. Can be null.
        /// Applies only to bodies and features.
        /// </summary>
        new ILayoutInsertionChildNodeIndexList CycleIndexList { get; }
    }
}
