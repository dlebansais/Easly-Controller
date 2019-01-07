using EaslyController.ReadOnly;

namespace EaslyController.Writeable
{
    /// <summary>
    /// Base inner for a single node inner.
    /// </summary>
    public interface IWriteableSingleInner : IReadOnlySingleInner, IWriteableInner
    {
        /// <summary>
        /// State of the node.
        /// </summary>
        new IWriteableNodeState ChildState { get; }
    }

    /// <summary>
    /// Base inner for a single node inner.
    /// </summary>
    public interface IWriteableSingleInner<out IIndex> : IReadOnlySingleInner<IIndex>, IWriteableInner<IIndex>
        where IIndex : IWriteableBrowsingChildIndex
    {
        /// <summary>
        /// State of the node.
        /// </summary>
        new IWriteableNodeState ChildState { get; }
    }

    /// <summary>
    /// Base inner for a single node inner.
    /// </summary>
    public abstract class WriteableSingleInner<IIndex> : ReadOnlySingleInner<IIndex>, IWriteableSingleInner<IIndex>, IWriteableSingleInner
        where IIndex : IWriteableBrowsingChildIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableSingleInner{IIndex}"/> class.
        /// </summary>
        /// <param name="owner">Parent containing the inner.</param>
        /// <param name="propertyName">Property name of the inner in <paramref name="owner"/>.</param>
        public WriteableSingleInner(IWriteableNodeState owner, string propertyName)
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
        /// State of the node.
        /// </summary>
        public new IWriteableNodeState ChildState { get { return (IWriteableNodeState)base.ChildState; } }
        #endregion

        #region Client Interface
        /// <summary>
        /// Replaces a node.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        /// <param name="nodeIndex">Index of the node to insert.</param>
        /// <param name="oldBrowsingIndex">Index of the replaced node upon return.</param>
        /// <param name="newBrowsingIndex">Index of the inserted node upon return.</param>
        /// <param name="childState">State of the inserted node upon return.</param>
        public abstract void Replace(IWriteableReplaceOperation operation, IWriteableInsertionChildIndex nodeIndex, out IWriteableBrowsingChildIndex oldBrowsingIndex, out IWriteableBrowsingChildIndex newBrowsingIndex, out IWriteableNodeState childState);
        #endregion
    }
}
