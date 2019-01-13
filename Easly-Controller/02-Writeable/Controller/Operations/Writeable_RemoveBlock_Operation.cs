using System.Diagnostics;

namespace EaslyController.Writeable
{
    /// <summary>
    /// Operation details for removing a block from a block list.
    /// </summary>
    public interface IWriteableRemoveBlockOperation : IWriteableRemoveOperation
    {
        /// <summary>
        /// Inner where the block removal is taking place.
        /// </summary>
        IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> Inner { get; }

        /// <summary>
        /// Index of the removed block.
        /// </summary>
        IWriteableBrowsingExistingBlockNodeIndex BlockIndex { get; }

        /// <summary>
        /// Block state removed.
        /// </summary>
        IWriteableBlockState BlockState { get; }

        /// <summary>
        /// Update the operation with details.
        /// </summary>
        /// <param name="blockState">Block state removed.</param>
        void Update(IWriteableBlockState blockState);
    }

    /// <summary>
    /// Operation details for removing a block from a block list.
    /// </summary>
    public class WriteableRemoveBlockOperation : WriteableRemoveOperation, IWriteableRemoveBlockOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of <see cref="WriteableRemoveBlockOperation"/>.
        /// </summary>
        /// <param name="inner">Inner where the block removal is taking place.</param>
        /// <param name="blockIndex">index of the removed block.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public WriteableRemoveBlockOperation(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> inner, IWriteableBrowsingExistingBlockNodeIndex blockIndex, bool isNested)
            : base(isNested)
        {
            Inner = inner;
            BlockIndex = blockIndex;
        }
        #endregion

        #region Client Interface
        /// <summary>
        /// Update the operation with details.
        /// </summary>
        /// <param name="blockState">Block state removed.</param>
        public virtual void Update(IWriteableBlockState blockState)
        {
            Debug.Assert(blockState != null);

            BlockState = blockState;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Inner where the block removal is taking place.
        /// </summary>
        public IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> Inner { get; }

        /// <summary>
        /// Index of the removed block.
        /// </summary>
        public IWriteableBrowsingExistingBlockNodeIndex BlockIndex { get; private set; }

        /// <summary>
        /// Block state removed.
        /// </summary>
        public IWriteableBlockState BlockState { get; private set; }
        #endregion
    }
}
