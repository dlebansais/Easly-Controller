namespace EaslyController.Frame
{
    using System.Collections.Generic;
    using BaseNode;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;

    /// <summary>
    /// Base interface for the state of a node.
    /// </summary>
    public interface IFrameNodeState : IWriteableNodeState
    {
        /// <summary>
        /// The index that was used to create the state.
        /// </summary>
        new IFrameIndex ParentIndex { get; }

        /// <summary>
        /// Inner containing this state.
        /// </summary>
        new IFrameInner ParentInner { get; }

        /// <summary>
        /// State of the parent.
        /// </summary>
        new IFrameNodeState ParentState { get; }

        /// <summary>
        /// Table for all inners in this state.
        /// </summary>
        new FrameInnerReadOnlyDictionary<string> InnerTable { get; }
    }

    /// <summary>
    /// Base interface for the state of a node.
    /// </summary>
    /// <typeparam name="IInner">Parent inner of the state.</typeparam>
    internal interface IFrameNodeState<out IInner> : IWriteableNodeState<IInner>
        where IInner : IFrameInner<IFrameBrowsingChildIndex>
    {
    }

    /// <summary>
    /// Base class for the state of a node.
    /// </summary>
    /// <typeparam name="IInner">Parent inner of the state.</typeparam>
    internal abstract class FrameNodeState<IInner> : WriteableNodeState<IInner>, IFrameNodeState<IInner>, IFrameNodeState
        where IInner : IFrameInner<IFrameBrowsingChildIndex>
    {
        #region Init
        /// <summary>
        /// Gets the empty <see cref="IFrameNodeState"/> object.
        /// </summary>
        public static new IFrameNodeState Empty { get; } = new FrameEmptyNodeState<IInner>();

        /// <summary>
        /// Initializes a new instance of the <see cref="FrameNodeState{IInner}"/> class.
        /// </summary>
        protected FrameNodeState()
            : this(FrameRootNodeIndex.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FrameNodeState{IInner}"/> class.
        /// </summary>
        /// <param name="parentIndex">The index used to create the state.</param>
        public FrameNodeState(IFrameIndex parentIndex)
            : base(parentIndex)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The index that was used to create the state.
        /// </summary>
        public new IFrameIndex ParentIndex { get { return (IFrameIndex)base.ParentIndex; } }

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
    }
}
