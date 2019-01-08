using EaslyController.ReadOnly;

namespace EaslyController.Writeable
{
    /// <summary>
    /// Base inner for a list or a block list.
    /// </summary>
    public interface IWriteableCollectionInner : IReadOnlyCollectionInner, IWriteableInner
    {
        /// <summary>
        /// First node state that can be enumerated in the inner.
        /// </summary>
        new IWriteablePlaceholderNodeState FirstNodeState { get; }

        /// <summary>
        /// Inserts a new node in a list or block list.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        void Insert(IWriteableInsertNodeOperation operation);

        /// <summary>
        /// Removes a node from a list or block list.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        void Remove(IWriteableRemoveNodeOperation operation);

        /// <summary>
        /// Moves a node around in a list or block list. In a block list, the node stays in same block.
        /// </summary>
        /// <param name="nodeOperation">Details of the operation performed.</param>
        /// <param name="nodeIndex">Index for the moved node.</param>
        void Move(IWriteableMoveNodeOperation operation, IWriteableBrowsingCollectionNodeIndex nodeIndex);
    }

    /// <summary>
    /// Base inner for a list or a block list.
    /// </summary>
    public interface IWriteableCollectionInner<out IIndex> : IReadOnlyCollectionInner<IIndex>, IWriteableInner<IIndex>
        where IIndex : IWriteableBrowsingCollectionNodeIndex
    {
        /// <summary>
        /// First node state that can be enumerated in the inner.
        /// </summary>
        new IWriteablePlaceholderNodeState FirstNodeState { get; }

        /// <summary>
        /// Inserts a new node in a list or block list.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        void Insert(IWriteableInsertNodeOperation operation);

        /// <summary>
        /// Removes a node from a list or block list.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        void Remove(IWriteableRemoveNodeOperation operation);

        /// <summary>
        /// Moves a node around in a list or block list. In a block list, the node stays in same block.
        /// </summary>
        /// <param name="nodeOperation">Details of the operation performed.</param>
        /// <param name="nodeIndex">Index for the moved node.</param>
        void Move(IWriteableMoveNodeOperation operation, IWriteableBrowsingCollectionNodeIndex nodeIndex);
    }

    /// <summary>
    /// Base inner for a list or a block list.
    /// </summary>
    public abstract class WriteableCollectionInner<IIndex, TIndex> : ReadOnlyCollectionInner<IIndex, TIndex>, IWriteableCollectionInner<IIndex>, IWriteableCollectionInner
        where IIndex : IWriteableBrowsingCollectionNodeIndex
        where TIndex : WriteableBrowsingCollectionNodeIndex, IIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableCollectionInner{IIndex, TIndex}"/> class.
        /// </summary>
        /// <param name="owner">Parent containing the inner.</param>
        /// <param name="propertyName">Property name of the inner in <paramref name="owner"/>.</param>
        public WriteableCollectionInner(IWriteableNodeState owner, string propertyName)
            : base(owner, propertyName)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Parent containing the inner.
        /// </summary>
        public new IWriteableNodeState Owner { get { return (IWriteableNodeState)base.Owner; } }

        /// <summary>
        /// First node state that can be enumerated in the inner.
        /// </summary>
        public new IWriteablePlaceholderNodeState FirstNodeState { get { return (IWriteablePlaceholderNodeState)base.FirstNodeState; } }
        #endregion

        #region Client Interface
        /// <summary>
        /// Inserts a new node in a list or block list.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        public abstract void Insert(IWriteableInsertNodeOperation operation);

        /// <summary>
        /// Removes a node from a list or block list.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        public abstract void Remove(IWriteableRemoveNodeOperation operation);

        /// <summary>
        /// Replaces a node.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        public abstract void Replace(IWriteableReplaceOperation operation);

        /// <summary>
        /// Moves a node around in a list or block list. In a block list, the node stays in same block.
        /// </summary>
        /// <param name="nodeOperation">Details of the operation performed.</param>
        /// <param name="nodeIndex">Index for the moved node.</param>
        public abstract void Move(IWriteableMoveNodeOperation operation, IWriteableBrowsingCollectionNodeIndex nodeIndex);
        #endregion
    }
}
