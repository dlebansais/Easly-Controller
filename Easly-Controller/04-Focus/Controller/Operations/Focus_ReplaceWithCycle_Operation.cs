using EaslyController.Writeable;
using System;

namespace EaslyController.Focus
{
    /// <summary>
    /// Operation details for replacing a node with another from a cycle.
    /// </summary>
    public interface IFocusReplaceWithCycleOperation : IFocusReplaceOperation
    {
        /// <summary>
        /// Cycle of nodes that can replace the current node.
        /// </summary>
        IFocusInsertionChildIndexList CycleIndexList { get; }

        /// <summary>
        /// New position in the cycle.
        /// </summary>
        int CyclePosition { get; }
    }

    /// <summary>
    /// Operation details for replacing a node with another from a cycle.
    /// </summary>
    public class FocusReplaceWithCycleOperation : FocusReplaceOperation, IFocusReplaceWithCycleOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of <see cref="FocusReplaceWithCycleOperation"/>.
        /// </summary>
        /// <param name="inner">Inner where the replacement is taking place.</param>
        /// <param name="cycleIndexList">Cycle of nodes that can replace the current node.</param>
        /// <param name="cyclePosition">New position in the cycle.</param>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public FocusReplaceWithCycleOperation(IFocusInner<IFocusBrowsingChildIndex> inner, IFocusInsertionChildIndexList cycleIndexList, int cyclePosition, Action<IWriteableOperation> handlerRedo, bool isNested)
            : base(inner, cycleIndexList[cyclePosition], handlerRedo, isNested)
        {
            CycleIndexList = cycleIndexList;
            CyclePosition = cyclePosition;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Cycle of nodes that can replace the current node.
        /// </summary>
        public IFocusInsertionChildIndexList CycleIndexList { get; }

        /// <summary>
        /// New position in the cycle.
        /// </summary>
        public int CyclePosition { get; }
        #endregion
    }
}
