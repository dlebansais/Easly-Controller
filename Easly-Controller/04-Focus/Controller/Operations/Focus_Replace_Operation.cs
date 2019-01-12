﻿using EaslyController.Frame;

namespace EaslyController.Focus
{
    /// <summary>
    /// Operation details for replacing a node.
    /// </summary>
    public interface IFocusReplaceOperation : IFrameReplaceOperation, IFocusOperation
    {
        /// <summary>
        /// Inner where the replacement is taking place.
        /// </summary>
        new IFocusInner<IFocusBrowsingChildIndex> Inner { get; }

        /// <summary>
        /// Position where the node is replaced.
        /// </summary>
        new IFocusInsertionChildIndex ReplacementIndex { get; }

        /// <summary>
        /// Index of the state before it's replaced.
        /// </summary>
        new IFocusBrowsingChildIndex OldBrowsingIndex { get; }

        /// <summary>
        /// Index of the state after it's replaced.
        /// </summary>
        new IFocusBrowsingChildIndex NewBrowsingIndex { get; }

        /// <summary>
        /// The new state.
        /// </summary>
        new IFocusNodeState ChildState { get; }
    }

    /// <summary>
    /// Operation details for replacing a node in a list or block list.
    /// </summary>
    public class FocusReplaceOperation : FrameReplaceOperation, IFocusReplaceOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of <see cref="FocusReplaceOperation"/>.
        /// </summary>
        /// <param name="inner">Inner where the replacement is taking place.</param>
        /// <param name="replacementIndex">Position where the node is replaced.</param>
        public FocusReplaceOperation(IFocusInner<IFocusBrowsingChildIndex> inner, IFocusInsertionChildIndex replacementIndex)
            : base(inner, replacementIndex)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Inner where the replacement is taking place.
        /// </summary>
        public new IFocusInner<IFocusBrowsingChildIndex> Inner { get { return (IFocusInner<IFocusBrowsingChildIndex>)base.Inner; } }

        /// <summary>
        /// Position where the node is replaced.
        /// </summary>
        public new IFocusInsertionChildIndex ReplacementIndex { get { return (IFocusInsertionChildIndex)base.ReplacementIndex; } }

        /// <summary>
        /// Index of the state before it's replaced.
        /// </summary>
        public new IFocusBrowsingChildIndex OldBrowsingIndex { get { return (IFocusBrowsingChildIndex)base.OldBrowsingIndex; } }

        /// <summary>
        /// Index of the state after it's replaced.
        /// </summary>
        public new IFocusBrowsingChildIndex NewBrowsingIndex { get { return (IFocusBrowsingChildIndex)base.NewBrowsingIndex; } }

        /// <summary>
        /// The new state.
        /// </summary>
        public new IFocusNodeState ChildState { get { return (IFocusNodeState)base.ChildState; } }
        #endregion
    }
}