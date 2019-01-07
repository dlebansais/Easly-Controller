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
        /// <param name="nodeIndex">Index of the node to insert.</param>
        void Insert(IWriteableInsertNodeOperation operation, IWriteableInsertionCollectionNodeIndex nodeIndex);

        /// <summary>
        /// Removes a node from a list or block list.
        /// </summary>
        /// <param name="nodeOperation">Details of the operation performed.</param>
        /// <param name="nodeIndex">Index of the node to remove.</param>
        void Remove(IWriteableRemoveNodeOperation nodeOperation, IWriteableBrowsingCollectionNodeIndex nodeIndex);

        /// <summary>
        /// Moves a node around in a list or block list. In a block list, the node stays in same block.
        /// </summary>
        /// <param name="nodeIndex">Index for the moved node.</param>
        /// <param name="direction">The change in position, relative to the current position.</param>
        void Move(IWriteableBrowsingCollectionNodeIndex nodeIndex, int direction);
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
        /// <param name="nodeIndex">Index of the node to insert.</param>
        void Insert(IWriteableInsertNodeOperation operation, IWriteableInsertionCollectionNodeIndex nodeIndex);

        /// <summary>
        /// Removes a node from a list or block list.
        /// </summary>
        /// <param name="nodeOperation">Details of the operation performed.</param>
        /// <param name="nodeIndex">Index of the node to remove.</param>
        void Remove(IWriteableRemoveNodeOperation nodeOperation, IWriteableBrowsingCollectionNodeIndex nodeIndex);

        /// <summary>
        /// Moves a node around in a list or block list. In a block list, the node stays in same block.
        /// </summary>
        /// <param name="nodeIndex">Index for the moved node.</param>
        /// <param name="direction">The change in position, relative to the current position.</param>
        void Move(IWriteableBrowsingCollectionNodeIndex nodeIndex, int direction);
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
        /// <param name="nodeIndex">Index of the node to insert.</param>
        public abstract void Insert(IWriteableInsertNodeOperation operation, IWriteableInsertionCollectionNodeIndex nodeIndex);

        /// <summary>
        /// Removes a node from a list or block list.
        /// </summary>
        /// <param name="nodeOperation">Details of the operation performed.</param>
        /// <param name="nodeIndex">Index of the node to remove.</param>
        public abstract void Remove(IWriteableRemoveNodeOperation nodeOperation, IWriteableBrowsingCollectionNodeIndex nodeIndex);

        /// <summary>
        /// Replaces a node.
        /// </summary>
        /// <param name="nodeIndex">Index of the node to insert.</param>
        /// <param name="oldBrowsingIndex">Index of the replaced node upon return.</param>
        /// <param name="newBrowsingIndex">Index of the inserted node upon return.</param>
        /// <param name="childState">State of the inserted node upon return.</param>
        public abstract void Replace(IWriteableInsertionChildIndex nodeIndex, out IWriteableBrowsingChildIndex oldBrowsingIndex, out IWriteableBrowsingChildIndex newBrowsingIndex, out IWriteableNodeState childState);

        /// <summary>
        /// Moves a node around in a list or block list. In a block list, the node stays in same block.
        /// </summary>
        /// <param name="nodeIndex">Index for the moved node.</param>
        /// <param name="direction">The change in position, relative to the current position.</param>
        public abstract void Move(IWriteableBrowsingCollectionNodeIndex nodeIndex, int direction);
        #endregion
    }
}
