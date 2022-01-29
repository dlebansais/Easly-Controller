namespace EaslyController.Writeable
{
    using System.Collections.Generic;
    using BaseNode;
    using EaslyController.ReadOnly;

    /// <summary>
    /// Base interface for the state of a node.
    /// </summary>
    public interface IWriteableNodeState : IReadOnlyNodeState
    {
        /// <summary>
        /// The index that was used to create the state.
        /// </summary>
        new IWriteableIndex ParentIndex { get; }

        /// <summary>
        /// Inner containing this state.
        /// </summary>
        new IWriteableInner ParentInner { get; }

        /// <summary>
        /// State of the parent.
        /// </summary>
        new IWriteableNodeState ParentState { get; }

        /// <summary>
        /// Table for all inners in this state.
        /// </summary>
        new WriteableInnerReadOnlyDictionary<string> InnerTable { get; }
    }

    /// <summary>
    /// Base interface for the state of a node.
    /// </summary>
    /// <typeparam name="IInner">Parent inner of the state.</typeparam>
    internal interface IWriteableNodeState<out IInner> : IReadOnlyNodeState<IInner>
        where IInner : IWriteableInner<IWriteableBrowsingChildIndex>
    {
    }

    /// <summary>
    /// Base class for the state of a node.
    /// </summary>
    /// <typeparam name="IInner">Parent inner of the state.</typeparam>
    internal abstract class WriteableNodeState<IInner> : ReadOnlyNodeState<IInner>, IWriteableNodeState<IInner>, IWriteableNodeState
        where IInner : IWriteableInner<IWriteableBrowsingChildIndex>
    {
        #region Init
        /// <summary>
        /// Gets the empty <see cref="IWriteableNodeState"/> object.
        /// </summary>
        public static new IWriteableNodeState Empty { get; } = new WriteableEmptyNodeState<IInner>();

        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableNodeState{IInner}"/> class.
        /// </summary>
        protected WriteableNodeState()
            : this(WriteableRootNodeIndex.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableNodeState{IInner}"/> class.
        /// </summary>
        /// <param name="parentIndex">The index used to create the state.</param>
        public WriteableNodeState(IWriteableIndex parentIndex)
            : base(parentIndex)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The index that was used to create the state.
        /// </summary>
        public new IWriteableIndex ParentIndex { get { return (IWriteableIndex)base.ParentIndex; } }

        /// <summary>
        /// Inner containing this state.
        /// </summary>
        public new IWriteableInner ParentInner { get { return (IWriteableInner)base.ParentInner; } }

        /// <summary>
        /// State of the parent.
        /// </summary>
        public new IWriteableNodeState ParentState { get { return (IWriteableNodeState)base.ParentState; } }

        /// <summary>
        /// Table for all inners in this state.
        /// </summary>
        public new WriteableInnerReadOnlyDictionary<string> InnerTable { get { return (WriteableInnerReadOnlyDictionary<string>)base.InnerTable; } }
        #endregion
    }
}
