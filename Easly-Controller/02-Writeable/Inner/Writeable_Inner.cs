namespace EaslyController.Writeable
{
    using EaslyController.ReadOnly;

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
        /// <param name="operation">Details of the operation performed.</param>
        void Replace(IWriteableReplaceOperation operation);
    }

    /// <summary>
    /// Interface for all inners.
    /// </summary>
    /// <typeparam name="IIndex">Type of the index.</typeparam>
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
        /// <param name="operation">Details of the operation performed.</param>
        void Replace(IWriteableReplaceOperation operation);
    }

    /// <summary>
    /// Interface for all inners.
    /// </summary>
    /// <typeparam name="IIndex">Type of the index.</typeparam>
    internal abstract class WriteableInner<IIndex> : ReadOnlyInner<IIndex>, IWriteableInner<IIndex>, IWriteableInner
        where IIndex : IWriteableBrowsingChildIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableInner{IIndex}"/> class.
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
        /// <param name="operation">Details of the operation performed.</param>
        public abstract void Replace(IWriteableReplaceOperation operation);
        #endregion
    }
}
