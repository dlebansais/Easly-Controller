namespace EaslyController.Writeable
{
    using System;
    using System.Diagnostics;
    using BaseNode;
    using BaseNodeHelper;
    using EaslyController.ReadOnly;

    /// <summary>
    /// Inner for a child node.
    /// </summary>
    public interface IWriteableEmptyInner : IReadOnlyEmptyInner, IWriteableSingleInner
    {
        /// <summary>
        /// The state of the node.
        /// </summary>
        new IWriteableEmptyNodeState ChildState { get; }
    }

    /// <summary>
    /// Inner for a child node.
    /// </summary>
    /// <typeparam name="TIndex">Type of the index.</typeparam>
    internal interface IWriteableEmptyInner<out TIndex> : IReadOnlyEmptyInner<TIndex>, IWriteableSingleInner<TIndex>
        where TIndex : IWriteableBrowsingChildIndex
    {
        /// <summary>
        /// The state of the node.
        /// </summary>
        new IWriteableEmptyNodeState ChildState { get; }
    }

    /// <summary>
    /// Inner for a child node.
    /// </summary>
    /// <typeparam name="TIndex">Type of the index as class.</typeparam>
    internal class WriteableEmptyInner<TIndex> : ReadOnlyEmptyInner<TIndex>, IWriteableEmptyInner<TIndex>, IWriteableEmptyInner
        where TIndex : IWriteableBrowsingChildIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableEmptyInner{TIndex}"/> class.
        /// </summary>
        public WriteableEmptyInner()
            : base()
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Parent containing the inner.
        /// </summary>
        public new IWriteableNodeState Owner { get { return (IWriteableNodeState)base.Owner; } }

        /// <summary>
        /// The state of the optional node.
        /// </summary>
        public new IWriteableEmptyNodeState ChildState { get { return (IWriteableEmptyNodeState)base.ChildState; } }
        #endregion

        #region Client Interface
        /// <summary>
        /// Replaces a node.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        public virtual void Replace(IWriteableReplaceOperation operation)
        {
            throw new NotSupportedException();
        }
        #endregion
    }
}
