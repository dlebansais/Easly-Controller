using BaseNode;
using EaslyController.Writeable;
using System;

namespace EaslyController.Frame
{
    /// <summary>
    /// Operation details for changing a block.
    /// </summary>
    public interface IFrameChangeBlockOperation : IWriteableChangeBlockOperation, IFrameOperation
    {
        /// <summary>
        /// Inner where the block change is taking place.
        /// </summary>
        new IFrameBlockListInner<IFrameBrowsingBlockNodeIndex> Inner { get; }

        /// <summary>
        /// Block state changed.
        /// </summary>
        new IFrameBlockState BlockState { get; }
    }

    /// <summary>
    /// Operation details for changing a node.
    /// </summary>
    public class FrameChangeBlockOperation : WriteableChangeBlockOperation, IFrameChangeBlockOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of <see cref="FrameChangeBlockOperation"/>.
        /// </summary>
        /// <param name="inner">Inner where the block change is taking place.</param>
        /// <param name="blockIndex">Index of the changed block.</param>
        /// <param name="replication">New replication value.</param>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public FrameChangeBlockOperation(IFrameBlockListInner<IFrameBrowsingBlockNodeIndex> inner, int blockIndex, ReplicationStatus replication, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
            : base(inner, blockIndex, replication, handlerRedo, handlerUndo, isNested)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Inner where the block change is taking place.
        /// </summary>
        public new IFrameBlockListInner<IFrameBrowsingBlockNodeIndex> Inner { get { return (IFrameBlockListInner<IFrameBrowsingBlockNodeIndex>)base.Inner; } }

        /// <summary>
        /// Block state changed.
        /// </summary>
        public new IFrameBlockState BlockState { get { return (IFrameBlockState)base.BlockState; } }
        #endregion
    }
}
