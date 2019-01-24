using EaslyController.Frame;
using EaslyController.Writeable;
using System;

namespace EaslyController.Focus
{
    /// <summary>
    /// Operation details for moving a node in a list or block list.
    /// </summary>
    public interface IFocusMoveNodeOperation : IFrameMoveNodeOperation, IFocusOperation
    {
        /// <summary>
        /// Inner where the move is taking place.
        /// </summary>
        new IFocusCollectionInner<IFocusBrowsingCollectionNodeIndex> Inner { get; }

        /// <summary>
        /// Index of the moved node.
        /// </summary>
        new IFocusBrowsingCollectionNodeIndex NodeIndex { get; }

        /// <summary>
        /// State moved.
        /// </summary>
        new IFocusPlaceholderNodeState State { get; }
    }

    /// <summary>
    /// Operation details for moving a node in a list or block list.
    /// </summary>
    public class FocusMoveNodeOperation : FrameMoveNodeOperation, IFocusMoveNodeOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of <see cref="FocusMoveNodeOperation"/>.
        /// </summary>
        /// <param name="inner">Inner where the move is taking place.</param>
        /// <param name="nodeIndex">Position where the node is moved.</param>
        /// <param name="direction">The change in position, relative to the current position.</param>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public FocusMoveNodeOperation(IFocusCollectionInner<IFocusBrowsingCollectionNodeIndex> inner, IFocusBrowsingCollectionNodeIndex nodeIndex, int direction, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
            : base(inner, nodeIndex, direction, handlerRedo, handlerUndo, isNested)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Inner where the move is taking place.
        /// </summary>
        public new IFocusCollectionInner<IFocusBrowsingCollectionNodeIndex> Inner { get { return (IFocusCollectionInner<IFocusBrowsingCollectionNodeIndex>)base.Inner; } }

        /// <summary>
        /// Index of the moved node.
        /// </summary>
        public new IFocusBrowsingCollectionNodeIndex NodeIndex { get { return (IFocusBrowsingCollectionNodeIndex)base.NodeIndex; } }

        /// <summary>
        /// State moved.
        /// </summary>
        public new IFocusPlaceholderNodeState State { get { return (IFocusPlaceholderNodeState)base.State; } }
        #endregion
    }
}
