namespace EaslyController.Frame
{
    using System.Collections.Generic;
    using BaseNode;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;

    /// <summary>
    /// State of an replication pattern node.
    /// </summary>
    public interface IFramePatternState : IWriteablePatternState, IFramePlaceholderNodeState
    {
        /// <summary>
        /// The parent block state.
        /// </summary>
        new IFrameBlockState ParentBlockState { get; }

        /// <summary>
        /// The index that was used to create the state.
        /// </summary>
        new IFrameBrowsingPatternIndex ParentIndex { get; }
    }

    /// <summary>
    /// State of an replication pattern node.
    /// </summary>
    /// <typeparam name="IInner">Parent inner of the state.</typeparam>
    internal interface IFramePatternState<out IInner> : IWriteablePatternState<IInner>, IFramePlaceholderNodeState<IInner>
        where IInner : IFrameInner<IFrameBrowsingChildIndex>
    {
    }

    /// <summary>
    /// State of an replication pattern node.
    /// </summary>
    /// <typeparam name="IInner">Parent inner of the state.</typeparam>
    internal class FramePatternState<IInner> : WriteablePatternState<IInner>, IFramePatternState<IInner>, IFramePatternState
        where IInner : IFrameInner<IFrameBrowsingChildIndex>
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FramePatternState{IInner}"/> class.
        /// </summary>
        /// <param name="parentBlockState">The parent block state.</param>
        /// <param name="index">The index used to create the state.</param>
        public FramePatternState(IFrameBlockState parentBlockState, IFrameBrowsingPatternIndex index)
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
        public new IFrameBrowsingPatternIndex ParentIndex { get { return (IFrameBrowsingPatternIndex)base.ParentIndex; } }
        IFrameIndex IFrameNodeState.ParentIndex { get { return ParentIndex; } }
        IFrameNodeIndex IFramePlaceholderNodeState.ParentIndex { get { return ParentIndex; } }

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
        public new IFrameInnerReadOnlyDictionary<string> InnerTable { get { return (IFrameInnerReadOnlyDictionary<string>)base.InnerTable; } }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxNodeStateList object.
        /// </summary>
        private protected override IReadOnlyNodeStateList CreateNodeStateList()
        {
            ControllerTools.AssertNoOverride(this, typeof(FramePatternState<IInner>));
            return new FrameNodeStateList();
        }
        #endregion
    }
}
