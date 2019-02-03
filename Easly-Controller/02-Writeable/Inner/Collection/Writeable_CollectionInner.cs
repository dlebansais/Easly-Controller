namespace EaslyController.Writeable
{
    using EaslyController.ReadOnly;

    /// <summary>
    /// Base inner for a list or a block list.
    /// </summary>
    public interface IWriteableCollectionInner : IReadOnlyCollectionInner, IWriteableInner
    {
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
        /// Checks whether a node can be moved in a list or block list.
        /// </summary>
        /// <param name="nodeIndex">Index of the node that would be moved.</param>
        /// <param name="direction">Direction of the move, relative to the current position of the item.</param>
        bool IsMoveable(IWriteableBrowsingCollectionNodeIndex nodeIndex, int direction);

        /// <summary>
        /// Moves a node around in a list or block list. In a block list, the node stays in same block.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        void Move(IWriteableMoveNodeOperation operation);
    }

    /// <summary>
    /// Base inner for a list or a block list.
    /// </summary>
    /// <typeparam name="IIndex">Type of the index.</typeparam>
    internal interface IWriteableCollectionInner<out IIndex> : IReadOnlyCollectionInner<IIndex>, IWriteableInner<IIndex>
        where IIndex : IWriteableBrowsingCollectionNodeIndex
    {
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
        /// Checks whether a node can be moved in a list or block list.
        /// </summary>
        /// <param name="nodeIndex">Index of the node that would be moved.</param>
        /// <param name="direction">Direction of the move, relative to the current position of the item.</param>
        bool IsMoveable(IWriteableBrowsingCollectionNodeIndex nodeIndex, int direction);

        /// <summary>
        /// Moves a node around in a list or block list. In a block list, the node stays in same block.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        void Move(IWriteableMoveNodeOperation operation);
    }

    /// <summary>
    /// Base inner for a list or a block list.
    /// </summary>
    /// <typeparam name="IIndex">Type of the index as interface.</typeparam>
    /// <typeparam name="TIndex">Type of the index as class.</typeparam>
    internal abstract class WriteableCollectionInner<IIndex, TIndex> : ReadOnlyCollectionInner<IIndex, TIndex>, IWriteableCollectionInner<IIndex>, IWriteableCollectionInner
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
        /// Checks whether a node can be moved in a list or block list.
        /// </summary>
        /// <param name="nodeIndex">Index of the node that would be moved.</param>
        /// <param name="direction">Direction of the move, relative to the current position of the item.</param>
        public abstract bool IsMoveable(IWriteableBrowsingCollectionNodeIndex nodeIndex, int direction);

        /// <summary>
        /// Moves a node around in a list or block list. In a block list, the node stays in same block.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        public abstract void Move(IWriteableMoveNodeOperation operation);
        #endregion
    }
}
