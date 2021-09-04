namespace EaslyController.Frame
{
    using System.Collections.Generic;
    using BaseNode;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;

    /// <summary>
    /// State of a source identifier node.
    /// </summary>
    public interface IFrameSourceState : IWriteableSourceState, IFrameNodeState
    {
        /// <summary>
        /// The parent block state.
        /// </summary>
        new IFrameBlockState ParentBlockState { get; }

        /// <summary>
        /// The index that was used to create the state.
        /// </summary>
        new IFrameBrowsingSourceIndex ParentIndex { get; }
    }

    /// <summary>
    /// State of a source identifier node.
    /// </summary>
    /// <typeparam name="IInner">Parent inner of the state.</typeparam>
    internal interface IFrameSourceState<out IInner> : IWriteableSourceState<IInner>, IFramePlaceholderNodeState<IInner>
        where IInner : IFrameInner<IFrameBrowsingChildIndex>
    {
    }

    /// <summary>
    /// State of a source identifier node.
    /// </summary>
    /// <typeparam name="IInner">Parent inner of the state.</typeparam>
    internal class FrameSourceState<IInner> : WriteableSourceState<IInner>, IFrameSourceState<IInner>, IFrameSourceState
        where IInner : IFrameInner<IFrameBrowsingChildIndex>
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameSourceState{IInner}"/> class.
        /// </summary>
        /// <param name="parentBlockState">The parent block state.</param>
        /// <param name="index">The index used to create the state.</param>
        public FrameSourceState(IFrameBlockState parentBlockState, IFrameBrowsingSourceIndex index)
            : base(parentBlockState, index)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The parent block state.
        /// </summary>
        public new IFrameBlockState ParentBlockState { get { return (IFrameBlockState)base.ParentBlockState; } }

        /// <summary>
        /// The index that was used to create the state.
        /// </summary>
        public new IFrameBrowsingSourceIndex ParentIndex { get { return (IFrameBrowsingSourceIndex)base.ParentIndex; } }
        IFrameIndex IFrameNodeState.ParentIndex { get { return ParentIndex; } }

        /// <summary>
        /// Inner containing this state.
        /// </summary>
        public new IFrameInner ParentInner { get { return (IFrameInner)base.ParentInner; } }

        /// <summary>
        /// State of the parent.
        /// </summary>
        public new IFrameNodeState ParentState { get { return (IFrameNodeState)base.ParentState; } }

        /// <summary>
        /// Table for all inners in this state.
        /// </summary>
        public new FrameInnerReadOnlyDictionary<string> InnerTable { get { return (FrameInnerReadOnlyDictionary<string>)base.InnerTable; } }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxNodeStateList object.
        /// </summary>
        private protected override ReadOnlyNodeStateList CreateNodeStateList()
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameSourceState<IInner>));
            return new FrameNodeStateList();
        }
        #endregion
    }
}
