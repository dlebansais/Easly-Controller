namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using BaseNode;
    using EaslyController.Frame;
    using EaslyController.ReadOnly;

    /// <summary>
    /// Base interface for the state of a node.
    /// </summary>
    public interface IFocusNodeState : IFrameNodeState
    {
        /// <summary>
        /// The index that was used to create the state.
        /// </summary>
        new IFocusIndex ParentIndex { get; }

        /// <summary>
        /// Inner containing this state.
        /// </summary>
        new IFocusInner ParentInner { get; }

        /// <summary>
        /// State of the parent.
        /// </summary>
        new IFocusNodeState ParentState { get; }

        /// <summary>
        /// Table for all inners in this state.
        /// </summary>
        new FocusInnerReadOnlyDictionary<string> InnerTable { get; }
    }

    /// <summary>
    /// Base interface for the state of a node.
    /// </summary>
    /// <typeparam name="IInner">Parent inner of the state.</typeparam>
    internal interface IFocusNodeState<out IInner> : IFrameNodeState<IInner>
        where IInner : IFocusInner<IFocusBrowsingChildIndex>
    {
    }

    /// <summary>
    /// Base class for the state of a node.
    /// </summary>
    /// <typeparam name="IInner">Parent inner of the state.</typeparam>
    internal abstract class FocusNodeState<IInner> : FrameNodeState<IInner>, IFocusNodeState<IInner>, IFocusNodeState
        where IInner : IFocusInner<IFocusBrowsingChildIndex>
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusNodeState{IInner}"/> class.
        /// </summary>
        /// <param name="parentIndex">The index used to create the state.</param>
        public FocusNodeState(IFocusIndex parentIndex)
            : base(parentIndex)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The index that was used to create the state.
        /// </summary>
        public new IFocusIndex ParentIndex { get { return (IFocusIndex)base.ParentIndex; } }

        /// <summary>
        /// Inner containing this state.
        /// </summary>
        public new IFocusInner ParentInner { get { return (IFocusInner)base.ParentInner; } }

        /// <summary>
        /// State of the parent.
        /// </summary>
        public new IFocusNodeState ParentState { get { return (IFocusNodeState)base.ParentState; } }

        /// <summary>
        /// Table for all inners in this state.
        /// </summary>
        public new FocusInnerReadOnlyDictionary<string> InnerTable { get { return (FocusInnerReadOnlyDictionary<string>)base.InnerTable; } }
        #endregion
    }
}
