namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using BaseNode;
    using EaslyController.Focus;
    using EaslyController.ReadOnly;

    /// <summary>
    /// Base interface for the state of a node.
    /// </summary>
    public interface ILayoutNodeState : IFocusNodeState
    {
        /// <summary>
        /// The index that was used to create the state.
        /// </summary>
        new ILayoutIndex ParentIndex { get; }

        /// <summary>
        /// Inner containing this state.
        /// </summary>
        new ILayoutInner ParentInner { get; }

        /// <summary>
        /// State of the parent.
        /// </summary>
        new ILayoutNodeState ParentState { get; }

        /// <summary>
        /// Table for all inners in this state.
        /// </summary>
        new LayoutInnerReadOnlyDictionary<string> InnerTable { get; }
    }

    /// <summary>
    /// Base interface for the state of a node.
    /// </summary>
    /// <typeparam name="IInner">Parent inner of the state.</typeparam>
    internal interface ILayoutNodeState<out IInner> : IFocusNodeState<IInner>
        where IInner : ILayoutInner<ILayoutBrowsingChildIndex>
    {
    }

    /// <summary>
    /// Base class for the state of a node.
    /// </summary>
    /// <typeparam name="IInner">Parent inner of the state.</typeparam>
    internal abstract class LayoutNodeState<IInner> : FocusNodeState<IInner>, ILayoutNodeState<IInner>, ILayoutNodeState
        where IInner : ILayoutInner<ILayoutBrowsingChildIndex>
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutNodeState{IInner}"/> class.
        /// </summary>
        /// <param name="parentIndex">The index used to create the state.</param>
        public LayoutNodeState(ILayoutIndex parentIndex)
            : base(parentIndex)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The index that was used to create the state.
        /// </summary>
        public new ILayoutIndex ParentIndex { get { return (ILayoutIndex)base.ParentIndex; } }

        /// <summary>
        /// Inner containing this state.
        /// </summary>
        public new ILayoutInner ParentInner { get { return (ILayoutInner)base.ParentInner; } }

        /// <summary>
        /// State of the parent.
        /// </summary>
        public new ILayoutNodeState ParentState { get { return (ILayoutNodeState)base.ParentState; } }

        /// <summary>
        /// Table for all inners in this state.
        /// </summary>
        public new LayoutInnerReadOnlyDictionary<string> InnerTable { get { return (LayoutInnerReadOnlyDictionary<string>)base.InnerTable; } }
        #endregion
    }
}
