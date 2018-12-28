using EaslyController.ReadOnly;

namespace EaslyController.Writeable
{
    /// <summary>
    /// Interface for all inners.
    /// </summary>
    public interface IWriteableInner : IReadOnlyInner
    {
        /// <summary>
        /// Parent containing the inner.
        /// </summary>
        new IWriteableNodeState Owner { get; }

        /// <summary>
        /// Replaces a node.
        /// </summary>
        /// <param name="nodeIndex">Index of the node to insert.</param>
        /// <param name="oldBrowsingIndex">Index of the replaced node upon return.</param>
        /// <param name="newBrowsingIndex">Index of the inserted node upon return.</param>
        /// <param name="childState">State of the inserted node upon return.</param>
        void Replace(IWriteableInsertionChildIndex nodeIndex, out IWriteableBrowsingChildIndex oldBrowsingIndex, out IWriteableBrowsingChildIndex newBrowsingIndex, out IWriteableNodeState childState);
    }

    /// <summary>
    /// Interface for all inners.
    /// </summary>
    public interface IWriteableInner<out IIndex> : IReadOnlyInner<IIndex>
        where IIndex : IWriteableBrowsingChildIndex
    {
        /// <summary>
        /// Parent containing the inner.
        /// </summary>
        new IWriteableNodeState Owner { get; }

        /// <summary>
        /// Replaces a node.
        /// </summary>
        /// <param name="nodeIndex">Index of the node to insert.</param>
        /// <param name="oldBrowsingIndex">Index of the replaced node upon return.</param>
        /// <param name="newBrowsingIndex">Index of the inserted node upon return.</param>
        /// <param name="childState">State of the inserted node upon return.</param>
        void Replace(IWriteableInsertionChildIndex nodeIndex, out IWriteableBrowsingChildIndex oldBrowsingIndex, out IWriteableBrowsingChildIndex newBrowsingIndex, out IWriteableNodeState childState);
    }

    /// <summary>
    /// Interface for all inners.
    /// </summary>
    public abstract class WriteableInner<IIndex> : ReadOnlyInner<IIndex>, IWriteableInner<IIndex>, IWriteableInner
        where IIndex : IWriteableBrowsingChildIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableInner{IIndex, TIndex}"/> class.
        /// </summary>
        /// <param name="owner">Parent containing the inner.</param>
        /// <param name="propertyName">Property name of the inner in <paramref name="owner"/>.</param>
        public WriteableInner(IWriteableNodeState owner, string propertyName)
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
