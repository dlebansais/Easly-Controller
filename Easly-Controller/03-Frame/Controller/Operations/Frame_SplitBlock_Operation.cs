using EaslyController.Writeable;

namespace EaslyController.Frame
{
    /// <summary>
    /// Operation details for splitting a block in a block list.
    /// </summary>
    public interface IFrameSplitBlockOperation : IWriteableSplitBlockOperation, IFrameOperation
    {
        /// <summary>
        /// Inner where the block is split.
        /// </summary>
        new IFrameBlockListInner<IFrameBrowsingBlockNodeIndex> Inner { get; }

        /// <summary>
        /// Index of the last node to stay in the old block.
        /// </summary>
        new IFrameBrowsingExistingBlockNodeIndex NodeIndex { get; }

        /// <summary>
        /// Block state inserted.
        /// </summary>
        new IFrameBlockState BlockState { get; }
    }

    /// <summary>
    /// Operation details for splitting a block in a block list.
    /// </summary>
    public class FrameSplitBlockOperation : WriteableSplitBlockOperation, IFrameSplitBlockOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of <see cref="FrameSplitBlockOperation"/>.
        /// </summary>
        /// <param name="inner">Inner where the block is split.</param>
        /// <param name="nodeIndex">Index of the last node to stay in the old block.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public FrameSplitBlockOperation(IFrameBlockListInner<IFrameBrowsingBlockNodeIndex> inner, IFrameBrowsingExistingBlockNodeIndex nodeIndex, bool isNested)
            : base(inner, nodeIndex, isNested)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Inner where the block is split.
        /// </summary>
        public new IFrameBlockListInner<IFrameBrowsingBlockNodeIndex> Inner { get { return (IFrameBlockListInner<IFrameBrowsingBlockNodeIndex>)base.Inner; } }

        /// <summary>
        /// Index of the last node to stay in the old block.
        /// </summary>
        public new IFrameBrowsingExistingBlockNodeIndex NodeIndex { get { return (IFrameBrowsingExistingBlockNodeIndex)base.NodeIndex; } }

        /// <summary>
        /// Block state inserted.
        /// </summary>
        public new IFrameBlockState BlockState { get { return (IFrameBlockState)base.BlockState; } }
        #endregion
    }
}
