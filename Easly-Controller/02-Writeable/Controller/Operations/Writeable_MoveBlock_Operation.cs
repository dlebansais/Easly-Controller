using System.Diagnostics;

namespace EaslyController.Writeable
{
    /// <summary>
    /// Operation details for moving a block in a block list.
    /// </summary>
    public interface IWriteableMoveBlockOperation : IWriteableOperation
    {
        /// <summary>
        /// Inner where the block is moved.
        /// </summary>
        IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> Inner { get; }

        /// <summary>
        /// Index of the moved block.
        /// </summary>
        int BlockIndex { get; }

        /// <summary>
        /// The change in position, relative to the current position.
        /// </summary>
        int Direction { get; }

        /// <summary>
        /// The moved block state.
        /// </summary>
        IWriteableBlockState BlockState { get; }

        /// <summary>
        /// Update the operation with details.
        /// </summary>
        /// <param name="blockState">The moved block state.</param>
        void Update(IWriteableBlockState blockState);
    }

    /// <summary>
    /// Operation details for moving a block in a block list.
    /// </summary>
    public class WriteableMoveBlockOperation : WriteableOperation, IWriteableMoveBlockOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of <see cref="WriteableMoveBlockOperation"/>.
        /// </summary>
        /// <param name="inner">Inner where the block is move.</param>
        /// <param name="blockIndex">Index of the moved block.</param>
        /// <param name="direction">The change in position, relative to the current position.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public WriteableMoveBlockOperation(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> inner, int blockIndex, int direction, bool isNested)
            : base(isNested)
        {
            Inner = inner;
            BlockIndex = blockIndex;
            Direction = direction;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Inner where the block is moved.
        /// </summary>
        public IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> Inner { get; }

        /// <summary>
        /// Index of the moved block.
        /// </summary>
        public int BlockIndex { get; }

        /// <summary>
        /// The change in position, relative to the current position.
        /// </summary>
        public int Direction { get; }

        /// <summary>
        /// The moved block state.
        /// </summary>
        public IWriteableBlockState BlockState { get; private set; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Update the operation with details.
        /// </summary>
        /// <param name="blockState">The moved block state.</param>
        public virtual void Update(IWriteableBlockState blockState)
        {
            Debug.Assert(blockState != null);

            BlockState = blockState;
        }
        #endregion
    }
}
