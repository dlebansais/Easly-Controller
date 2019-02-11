namespace EaslyController.Layout
{
    using System.Diagnostics;
    using EaslyController.Focus;
    using EaslyController.Frame;

    /// <summary>
    /// View of a node state.
    /// </summary>
    public interface ILayoutNodeStateView : IFocusNodeStateView
    {
        /// <summary>
        /// The controller view to which this object belongs.
        /// </summary>
        new ILayoutControllerView ControllerView { get; }

        /// <summary>
        /// The node state.
        /// </summary>
        new ILayoutNodeState State { get; }

        /// <summary>
        /// The template used to display the state.
        /// </summary>
        new ILayoutTemplate Template { get; }

        /// <summary>
        /// Root cell for the view.
        /// </summary>
        new ILayoutCellView RootCellView { get; }

        /// <summary>
        /// Table of cell views that are mutable lists of cells.
        /// </summary>
        new ILayoutAssignableCellViewReadOnlyDictionary<string> CellViewTable { get; }
    }

    /// <summary>
    /// View of a node state.
    /// </summary>
    internal abstract class LayoutNodeStateView : FocusNodeStateView, ILayoutNodeStateView
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutNodeStateView"/> class.
        /// </summary>
        /// <param name="controllerView">The controller view to which this object belongs.</param>
        /// <param name="state">The node state.</param>
        public LayoutNodeStateView(ILayoutControllerView controllerView, ILayoutNodeState state)
            : base(controllerView, state)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The controller view to which this object belongs.
        /// </summary>
        public new ILayoutControllerView ControllerView { get { return (ILayoutControllerView)base.ControllerView; } }

        /// <summary>
        /// The node state.
        /// </summary>
        public new ILayoutNodeState State { get { return (ILayoutNodeState)base.State; } }

        /// <summary>
        /// The template used to display the state.
        /// </summary>
        public new ILayoutTemplate Template { get { return (ILayoutTemplate)base.Template; } }

        /// <summary>
        /// Root cell for the view.
        /// </summary>
        public new ILayoutCellView RootCellView { get { return (ILayoutCellView)base.RootCellView; } }

        /// <summary>
        /// Table of cell views that are mutable lists of cells.
        /// </summary>
        public new ILayoutAssignableCellViewReadOnlyDictionary<string> CellViewTable { get { return (ILayoutAssignableCellViewReadOnlyDictionary<string>)base.CellViewTable; } }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="ILayoutNodeStateView"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out LayoutNodeStateView AsNodeStateView))
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
            ControllerTools.AssertNoOverride(this, typeof(LayoutNodeStateView));
            return new LayoutAssignableCellViewDictionary<string>();
        }
        #endregion
    }
}
