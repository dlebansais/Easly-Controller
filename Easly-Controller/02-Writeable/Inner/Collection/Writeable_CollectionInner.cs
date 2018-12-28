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
        /// <param name="nodeIndex">Index of the node to insert.</param>
        /// <param name="browsingIndex">Index of the inserted node upon return.</param>
        /// <param name="childState">The inserted node state upon return.</param>
        void Insert(IWriteableInsertionCollectionNodeIndex nodeIndex, out IWriteableBrowsingCollectionNodeIndex browsingIndex, out IWriteablePlaceholderNodeState childState);
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
        /// <param name="nodeIndex">Index of the node to insert.</param>
        /// <param name="browsingIndex">Index of the inserted node upon return.</param>
        /// <param name="childState">The inserted node state upon return.</param>
        void Insert(IWriteableInsertionCollectionNodeIndex nodeIndex, out IWriteableBrowsingCollectionNodeIndex browsingIndex, out IWriteablePlaceholderNodeState childState);
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
        /// <param name="nodeIndex">Index of the node to insert.</param>
        /// <param name="browsingIndex">Index of the inserted node upon return.</param>
        /// <param name="childState">The inserted node state upon return.</param>
        public abstract void Insert(IWriteableInsertionCollectionNodeIndex nodeIndex, out IWriteableBrowsingCollectionNodeIndex browsingIndex, out IWriteablePlaceholderNodeState childState);

        /// <summary>
        /// Replaces a node.
        /// </summary>
        /// <param name="nodeIndex">Index of the node to insert.</param>
        /// <param name="oldBrowsingIndex">Index of the replaced node upon return.</param>
        /// <param name="newBrowsingIndex">Index of the inserted node upon return.</param>
        /// <param name="childState">State of the inserted node upon return.</param>
        public abstract void Replace(IWriteableInsertionChildIndex nodeIndex, out IWriteableBrowsingChildIndex oldBrowsingIndex, out IWriteableBrowsingChildIndex newBrowsingIndex, out IWriteableNodeState childState);
        #endregion
    }
}
