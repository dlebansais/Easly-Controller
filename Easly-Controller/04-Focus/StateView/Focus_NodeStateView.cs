using EaslyController.Frame;
using System.Diagnostics;

namespace EaslyController.Focus
{
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
    }

    /// <summary>
    /// View of a node state.
    /// </summary>
    public abstract class FocusNodeStateView : FrameNodeStateView, IFocusNodeStateView
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

            if (!(other is IFocusNodeStateView AsNodeStateView))
                return false;

            if (!base.IsEqual(comparer, AsNodeStateView))
                return false;

            return true;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxAssignableCellViewDictionary{string} object.
        /// </summary>
        protected override IFrameAssignableCellViewDictionary<string> CreateCellViewTable()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusNodeStateView));
            return new FocusAssignableCellViewDictionary<string>();
        }
        #endregion
    }
}
