﻿namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using BaseNode;
    using EaslyController.Frame;

    /// <summary>
    /// View of a node state.
    /// </summary>
    public interface IFocusNodeStateView : IFrameNodeStateView
    {
        /// <summary>
        /// The controller view to which this object belongs.
        /// </summary>
        new FocusControllerView ControllerView { get; }

        /// <summary>
        /// The node state.
        /// </summary>
        new IFocusNodeState State { get; }

        /// <summary>
        /// The template used to display the state.
        /// </summary>
        new IFocusTemplate Template { get; }

        /// <summary>
        /// Root cell for the view.
        /// </summary>
        new IFocusCellView RootCellView { get; }

        /// <summary>
        /// Table of cell views that are mutable lists of cells.
        /// </summary>
        new FocusAssignableCellViewReadOnlyDictionary<string> CellViewTable { get; }

        /// <summary>
        /// The cell view that is embedding this state view. Can be null.
        /// </summary>
        new IFocusContainerCellView ParentContainer { get; }

        /// <summary>
        /// Indicates if this view has all its frames forced to visible.
        /// </summary>
        bool IsUserVisible { get; }

        /// <summary>
        /// Updates the focus chain with cells in the tree.
        /// </summary>
        /// <param name="focusChain">The list of focusable cell views found in the tree.</param>
        /// <param name="focusedNode">The currently focused node.</param>
        /// <param name="focusedFrame">The currently focused frame in the template associated to <paramref name="focusedNode"/>.</param>
        /// <param name="matchingFocus">The focus in <paramref name="focusChain"/> that match <paramref name="focusedNode"/> and <paramref name="focusedFrame"/> upon return.</param>
        void UpdateFocusChain(FocusFocusList focusChain, Node focusedNode, IFocusFrame focusedFrame, ref IFocusFocus matchingFocus);

        /// <summary>
        /// Sets the <see cref="IsUserVisible"/> flag.
        /// </summary>
        /// <param name="isUserVisible">The new value.</param>
        void SetIsUserVisible(bool isUserVisible);

        /// <summary>
        /// Gets the selector stack corresponding to this view and all its parent.
        /// </summary>
        IList<FocusFrameSelectorList> GetSelectorStack();
    }

    /// <summary>
    /// View of a node state.
    /// </summary>
    internal abstract class FocusNodeStateView : FrameNodeStateView, IFocusNodeStateView
    {
        #region Init
        /// <summary>
        /// Gets the empty <see cref="FocusNodeStateView"/> object.
        /// </summary>
        public static new IFocusNodeStateView Empty { get; } = new FocusEmptyNodeStateView();

        /// <summary>
        /// Initializes a new instance of the <see cref="FocusNodeStateView"/> class.
        /// </summary>
        protected FocusNodeStateView()
            : base(FocusControllerView.Empty, FocusNodeState<IFocusInner<IFocusBrowsingChildIndex>>.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FocusNodeStateView"/> class.
        /// </summary>
        /// <param name="controllerView">The controller view to which this object belongs.</param>
        /// <param name="state">The node state.</param>
        public FocusNodeStateView(FocusControllerView controllerView, IFocusNodeState state)
            : base(controllerView, state)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The controller view to which this object belongs.
        /// </summary>
        public new FocusControllerView ControllerView { get { return (FocusControllerView)base.ControllerView; } }

        /// <summary>
        /// The node state.
        /// </summary>
        public new IFocusNodeState State { get { return (IFocusNodeState)base.State; } }

        /// <summary>
        /// The template used to display the state.
        /// </summary>
        public new IFocusTemplate Template { get { return (IFocusTemplate)base.Template; } }

        /// <summary>
        /// Root cell for the view.
        /// </summary>
        public new IFocusCellView RootCellView { get { return (IFocusCellView)base.RootCellView; } }

        /// <summary>
        /// Table of cell views that are mutable lists of cells.
        /// </summary>
        public new FocusAssignableCellViewReadOnlyDictionary<string> CellViewTable { get { return (FocusAssignableCellViewReadOnlyDictionary<string>)base.CellViewTable; } }

        /// <summary>
        /// The cell view that is embedding this state view. Can be null.
        /// </summary>
        public new IFocusContainerCellView ParentContainer { get { return (IFocusContainerCellView)base.ParentContainer; } }

        /// <summary>
        /// Indicates if this view has all its frames forced to visible.
        /// </summary>
        public abstract bool IsUserVisible { get; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Updates the focus chain with cells in the tree.
        /// </summary>
        /// <param name="focusChain">The list of focusable cell views found in the tree.</param>
        /// <param name="focusedNode">The currently focused node.</param>
        /// <param name="focusedFrame">The currently focused frame in the template associated to <paramref name="focusedNode"/>.</param>
        /// <param name="matchingFocus">The focus in <paramref name="focusChain"/> that match <paramref name="focusedNode"/> and <paramref name="focusedFrame"/> upon return.</param>
        public abstract void UpdateFocusChain(FocusFocusList focusChain, Node focusedNode, IFocusFrame focusedFrame, ref IFocusFocus matchingFocus);

        /// <summary>
        /// Sets the <see cref="IsUserVisible"/> flag.
        /// </summary>
        /// <param name="isUserVisible">The new value.</param>
        public abstract void SetIsUserVisible(bool isUserVisible);

        /// <summary>
        /// Gets the selector stack corresponding to this view and all its parent.
        /// </summary>
        public abstract IList<FocusFrameSelectorList> GetSelectorStack();
        #endregion
    }
}
