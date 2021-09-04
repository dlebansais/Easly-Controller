namespace EaslyController.Writeable
{
    using System.Collections.Generic;
    using BaseNode;
    using EaslyController.ReadOnly;

    /// <summary>
    /// State of an replication pattern node.
    /// </summary>
    public interface IWriteablePatternState : IReadOnlyPatternState, IWriteableNodeState
    {
        /// <summary>
        /// The parent block state.
        /// </summary>
        new IWriteableBlockState ParentBlockState { get; }

        /// <summary>
        /// The index that was used to create the state.
        /// </summary>
        new WriteableBrowsingPatternIndex ParentIndex { get; }
    }

    /// <summary>
    /// State of an replication pattern node.
    /// </summary>
    /// <typeparam name="IInner">Parent inner of the state.</typeparam>
    internal interface IWriteablePatternState<out IInner> : IReadOnlyPatternState<IInner>, IWriteablePlaceholderNodeState<IInner>
        where IInner : IWriteableInner<IWriteableBrowsingChildIndex>
    {
    }

    /// <summary>
    /// State of an replication pattern node.
    /// </summary>
    /// <typeparam name="IInner">Parent inner of the state.</typeparam>
    internal class WriteablePatternState<IInner> : ReadOnlyPatternState<IInner>, IWriteablePatternState<IInner>, IWriteablePatternState
        where IInner : IWriteableInner<IWriteableBrowsingChildIndex>
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteablePatternState{IInner}"/> class.
        /// </summary>
        /// <param name="parentBlockState">The parent block state.</param>
        /// <param name="index">The index used to create the state.</param>
        public WriteablePatternState(IWriteableBlockState parentBlockState, IWriteableBrowsingPatternIndex index)
            : base(parentBlockState, index)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The parent block state.
        /// </summary>
        public new IWriteableBlockState ParentBlockState { get { return (IWriteableBlockState)base.ParentBlockState; } }

        /// <summary>
        /// The index that was used to create the state.
        /// </summary>
        public new WriteableBrowsingPatternIndex ParentIndex { get { return (WriteableBrowsingPatternIndex)base.ParentIndex; } }
        IWriteableIndex IWriteableNodeState.ParentIndex { get { return ParentIndex; } }

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

        #region Create Methods
        /// <summary>
        /// Creates a IxxxNodeStateList object.
        /// </summary>
        private protected override ReadOnlyNodeStateList CreateNodeStateList()
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteablePatternState<IInner>));
            return new WriteableNodeStateList();
        }
        #endregion
    }
}
