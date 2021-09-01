namespace EaslyController.Writeable
{
    using EaslyController.ReadOnly;

    /// <summary>
    /// Base inner for a single node inner.
    /// </summary>
    public interface IWriteableSingleInner : IReadOnlySingleInner, IWriteableInner
    {
    }


    /// <summary>
    /// Base inner for a single node inner.
    /// </summary>
    /// <typeparam name="IIndex">Type of the index.</typeparam>
    internal interface IWriteableSingleInner<out IIndex> : IReadOnlySingleInner<IIndex>, IWriteableInner<IIndex>
        where IIndex : IWriteableBrowsingChildIndex
    {
    }

    /// <summary>
    /// Base inner for a single node inner.
    /// </summary>
    /// <typeparam name="IIndex">Type of the index.</typeparam>
    internal abstract class WriteableSingleInner<IIndex> : ReadOnlySingleInner<IIndex>, IWriteableSingleInner<IIndex>, IWriteableSingleInner
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
        #endregion

        #region Client Interface
        /// <summary>
        /// Replaces a node.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        public abstract void Replace(WriteableReplaceOperation operation);
        #endregion
    }
}
