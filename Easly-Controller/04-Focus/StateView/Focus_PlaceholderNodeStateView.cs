namespace EaslyController.Focus
{
    using System.Diagnostics;
    using EaslyController.Frame;

    /// <summary>
    /// View of a child node.
    /// </summary>
    public interface IFocusPlaceholderNodeStateView : IFramePlaceholderNodeStateView, IFocusNodeStateView, IFocusReplaceableStateView
    {
        /// <summary>
        /// The child node.
        /// </summary>
        new IFocusPlaceholderNodeState State { get; }
    }

    /// <summary>
    /// View of a child node.
    /// </summary>
    internal class FocusPlaceholderNodeStateView : FramePlaceholderNodeStateView, IFocusPlaceholderNodeStateView
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusPlaceholderNodeStateView"/> class.
        /// </summary>
        /// <param name="controllerView">The controller view to which this object belongs.</param>
        /// <param name="state">The child node state.</param>
        public FocusPlaceholderNodeStateView(IFocusControllerView controllerView, IFocusPlaceholderNodeState state)
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
        /// The child node.
        /// </summary>
        public new IFocusPlaceholderNodeState State { get { return (IFocusPlaceholderNodeState)base.State; } }
        IFocusNodeState IFocusNodeStateView.State { get { return State; } }

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
        public bool IsUserVisible { get; private set; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Builds the cell view tree for this view.
        /// </summary>
        /// <param name="context">Context used to build the cell view tree.</param>
        public override void BuildRootCellView(IFrameCellViewTreeContext context)
        {
            ((IFocusCellViewTreeContext)context).ChangeIsUserVisible(IsUserVisible, out bool OldIsUserVisible);

            if (((IFocusCellViewTreeContext)context).IsVisible)
                base.BuildRootCellView(context);
            else
            {
                InitCellViewTable();
                SetRootCellView(CreateEmptyCellView(((IFocusCellViewTreeContext)context).StateView, null));
                SealCellViewTable();
            }

            ((IFocusCellViewTreeContext)context).RestoreIsUserVisible(OldIsUserVisible);
        }

        /// <summary>
        /// Updates the focus chain with cells in the tree.
        /// </summary>
        /// <param name="focusChain">The list of focusable cell views found in the tree.</param>
        public virtual void UpdateFocusChain(IFocusFocusList focusChain)
        {
            Debug.Assert(RootCellView != null);

            RootCellView.UpdateFocusChain(focusChain);
        }

        /// <summary>
        /// Sets the <see cref="IsUserVisible"/> flag.
        /// </summary>
        /// <param name="isUserVisible">The new value.</param>
        public virtual void SetIsUserVisible(bool isUserVisible)
        {
            IsUserVisible = isUserVisible;
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IFocusPlaceholderNodeStateView"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out FocusPlaceholderNodeStateView AsPlaceholderNodeStateView))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsPlaceholderNodeStateView))
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
            ControllerTools.AssertNoOverride(this, typeof(FocusPlaceholderNodeStateView));
            return new FocusAssignableCellViewDictionary<string>();
        }

        /// <summary>
        /// Creates a IxxxEmptyCellView object.
        /// </summary>
        private protected virtual IFocusEmptyCellView CreateEmptyCellView(IFocusNodeStateView stateView, IFocusCellViewCollection parentCellView)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusPlaceholderNodeStateView));
            return new FocusEmptyCellView(stateView, parentCellView);
        }
        #endregion
    }
}
