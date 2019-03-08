namespace EaslyController.Focus
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
        new IFocusControllerView ControllerView { get; }

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
        new IFocusAssignableCellViewReadOnlyDictionary<string> CellViewTable { get; }

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
        void UpdateFocusChain(IFocusFocusList focusChain, INode focusedNode, IFocusFrame focusedFrame, ref IFocusFocus matchingFocus);

        /// <summary>
        /// Sets the <see cref="IsUserVisible"/> flag.
        /// </summary>
        /// <param name="isUserVisible">The new value.</param>
        void SetIsUserVisible(bool isUserVisible);

        /// <summary>
        /// Gets the selector stack corresponding to this view and all its parent.
        /// </summary>
        IList<IFocusFrameSelectorList> GetSelectorStack();
    }

    /// <summary>
    /// View of a node state.
    /// </summary>
    internal abstract class FocusNodeStateView : FrameNodeStateView, IFocusNodeStateView
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusNodeStateView"/> class.
        /// </summary>
        /// <param name="controllerView">The controller view to which this object belongs.</param>
        /// <param name="state">The node state.</param>
        public FocusNodeStateView(IFocusControllerView controllerView, IFocusNodeState state)
            : base(controllerView, state)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The controller view to which this object belongs.
        /// </summary>
        public new IFocusControllerView ControllerView { get { return (IFocusControllerView)base.ControllerView; } }

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
        public new IFocusAssignableCellViewReadOnlyDictionary<string> CellViewTable { get { return (IFocusAssignableCellViewReadOnlyDictionary<string>)base.CellViewTable; } }

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
        public abstract void UpdateFocusChain(IFocusFocusList focusChain, INode focusedNode, IFocusFrame focusedFrame, ref IFocusFocus matchingFocus);

        /// <summary>
        /// Sets the <see cref="IsUserVisible"/> flag.
        /// </summary>
        /// <param name="isUserVisible">The new value.</param>
        public abstract void SetIsUserVisible(bool isUserVisible);

        /// <summary>
        /// Gets the selector stack corresponding to this view and all its parent.
        /// </summary>
        public abstract IList<IFocusFrameSelectorList> GetSelectorStack();
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IFocusNodeStateView"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out FocusNodeStateView AsNodeStateView))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsNodeStateView))
                return comparer.Failed();

            return true;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxAssignableCellViewDictionary{string} object.
        /// </summary>
        private protected override IFrameAssignableCellViewDictionary<string> CreateCellViewTable()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusNodeStateView));
            return new FocusAssignableCellViewDictionary<string>();
        }
        #endregion
    }
}
