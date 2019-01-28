using EaslyController.Frame;
using EaslyController.Writeable;
using System;

namespace EaslyController.Focus
{
    /// <summary>
    /// Operation details for merging blocks in a block list.
    /// </summary>
    public interface IFocusMergeBlocksOperation : IFrameMergeBlocksOperation, IFocusOperation
    {
        /// <summary>
        /// Inner where blocks are merged.
        /// </summary>
        new IFocusBlockListInner<IFocusBrowsingBlockNodeIndex> Inner { get; }

        /// <summary>
        /// Index of the first node in the merged block.
        /// </summary>
        new IFocusBrowsingExistingBlockNodeIndex NodeIndex { get; }
    }

    /// <summary>
    /// Operation details for merging blocks in a block list.
    /// </summary>
    public class FocusMergeBlocksOperation : FrameMergeBlocksOperation, IFocusMergeBlocksOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of <see cref="FocusMergeBlocksOperation"/>.
        /// </summary>
        /// <param name="inner">Inner where the block is split.</param>
        /// <param name="nodeIndex">Index of the last node to stay in the old block.</param>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public FocusMergeBlocksOperation(IFocusBlockListInner<IFocusBrowsingBlockNodeIndex> inner, IFocusBrowsingExistingBlockNodeIndex nodeIndex, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
            : base(inner, nodeIndex, handlerRedo, handlerUndo, isNested)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Inner where blocks are merged.
        /// </summary>
        public new IFocusBlockListInner<IFocusBrowsingBlockNodeIndex> Inner { get { return (IFocusBlockListInner<IFocusBrowsingBlockNodeIndex>)base.Inner; } }

        /// <summary>
        /// Index of the first node in the merged block.
        /// </summary>
        public new IFocusBrowsingExistingBlockNodeIndex NodeIndex { get { return (IFocusBrowsingExistingBlockNodeIndex)base.NodeIndex; } }
        #endregion
    }
}
