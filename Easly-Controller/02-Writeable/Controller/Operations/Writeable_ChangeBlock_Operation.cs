using BaseNode;
using System;
using System.Diagnostics;

namespace EaslyController.Writeable
{
    /// <summary>
    /// Operation details for changing a block.
    /// </summary>
    public interface IWriteableChangeBlockOperation : IWriteableOperation
    {
        /// <summary>
        /// Inner where the block change is taking place.
        /// </summary>
        IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> Inner { get; }

        /// <summary>
        /// Index of the changed block.
        /// </summary>
        int BlockIndex { get; }

        /// <summary>
        /// New replication value.
        /// </summary>
        ReplicationStatus Replication { get; }

        /// <summary>
        /// Block state changed.
        /// </summary>
        IWriteableBlockState BlockState { get; }

        /// <summary>
        /// Update the operation with details.
        /// </summary>
        /// <param name="blockState">Block state changed.</param>
        void Update(IWriteableBlockState blockState);
    }

    /// <summary>
    /// Operation details for changing a node.
    /// </summary>
    public class WriteableChangeBlockOperation : WriteableOperation, IWriteableChangeBlockOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of <see cref="WriteableChangeBlockOperation"/>.
        /// </summary>
        /// <param name="inner">Inner where the block change is taking place.</param>
        /// <param name="blockIndex">Index of the changed block.</param>
        /// <param name="replication">New replication value.</param>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public WriteableChangeBlockOperation(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> inner, int blockIndex, ReplicationStatus replication, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
            : base(handlerRedo, handlerUndo, isNested)
        {
            Inner = inner;
            BlockIndex = blockIndex;
            Replication = replication;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Inner where the block change is taking place.
        /// </summary>
        public IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> Inner { get; }

        /// <summary>
        /// Index of the changed block.
        /// </summary>
        public int BlockIndex { get; }

        /// <summary>
        /// New replication value.
        /// </summary>
        public ReplicationStatus Replication { get; }

        /// <summary>
        /// Block state changed.
        /// </summary>
        public IWriteableBlockState BlockState { get; private set; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Update the operation with details.
        /// </summary>
        /// <param name="blockState">Block state changed.</param>
        public virtual void Update(IWriteableBlockState blockState)
        {
            Debug.Assert(blockState != null);

            BlockState = blockState;
        }
        #endregion
    }
}
