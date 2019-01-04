using EaslyController.Writeable;
using System.Diagnostics;

namespace EaslyController.Frame
{
    /// <summary>
    /// View of a node state.
    /// </summary>
    public interface IFrameNodeStateView : IWriteableNodeStateView
    {
        /// <summary>
        /// The controller view to which this object belongs.
        /// </summary>
        new IFrameControllerView ControllerView { get; }

        /// <summary>
        /// The node state.
        /// </summary>
        new IFrameNodeState State { get; }

        /// <summary>
        /// The template used to display the state.
        /// </summary>
        IFrameTemplate Template { get; }

        /// <summary>
        /// Root cell for the view.
        /// </summary>
        IFrameCellView RootCellView { get; }

        /// <summary>
        /// Table of cell views that are mutable lists of cells.
        /// </summary>
        IFrameCellViewReadOnlyDictionary<string> CellViewTable { get; }

        /// <summary>
        /// Builds the cell view tree for this view.
        /// </summary>
        /// <param name="controllerView">The view in which the state is initialized.</param>
        void BuildRootCellView(IFrameControllerView controllerView);

        /// <param name="propertyName">The property name of the inner.</param>
        /// <param name="cellView">The assigned cell view.</param>
        void AssignCellViewTable(string propertyName, IFrameCellView cellView);

        void RecalculateLineNumbers(IFrameController controller, ref int lineNumber, ref int columnNumber);
    }

    /// <summary>
    /// View of a node state.
    /// </summary>
    public abstract class FrameNodeStateView : WriteableNodeStateView, IFrameNodeStateView
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameNodeStateView"/> class.
        /// </summary>
        /// <param name="controllerView">The controller view to which this object belongs.</param>
        /// <param name="state">The node state.</param>
        /// <param name="templateSet">The template set used to display the state.</param>
        public FrameNodeStateView(IFrameControllerView controllerView, IFrameNodeState state, IFrameTemplateSet templateSet)
            : base(controllerView, state)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The controller view to which this object belongs.
        /// </summary>
        public new IFrameControllerView ControllerView { get { return (IFrameControllerView)base.ControllerView; } }

        /// <summary>
        /// The node state.
        /// </summary>
        public new IFrameNodeState State { get { return (IFrameNodeState)base.State; } }

        /// <summary>
        /// The template used to display the state.
        /// </summary>
        public abstract IFrameTemplate Template { get; }

        /// <summary>
        /// Root cell for the view.
        /// </summary>
        public abstract IFrameCellView RootCellView { get; }

        /// <summary>
        /// Table of cell views that are mutable lists of cells.
        /// </summary>
        public abstract IFrameCellViewReadOnlyDictionary<string> CellViewTable { get; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Builds the cell view tree for this view.
        /// </summary>
        /// <param name="controllerView">The view in which the state is initialized.</param>
        public abstract void BuildRootCellView(IFrameControllerView controllerView);

        /// <summary>
        /// Assign the cell view corresponding to an inner.
        /// </summary>
        /// <param name="propertyName">The property name of the inner.</param>
        /// <param name="cellView">The assigned cell view.</param>
        public abstract void AssignCellViewTable(string propertyName, IFrameCellView cellView);

        public abstract void RecalculateLineNumbers(IFrameController controller, ref int lineNumber, ref int columnNumber);
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IFrameNodeStateView"/> objects.
        /// </summary>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IFrameNodeStateView AsNodeStateView))
                return false;

            if (!base.IsEqual(comparer, AsNodeStateView))
                return false;

            if (Template != AsNodeStateView.Template)
                return false;

            if (!comparer.VerifyEqual(RootCellView, AsNodeStateView.RootCellView))
                return false;

            if (!comparer.VerifyEqual(CellViewTable, AsNodeStateView.CellViewTable))
                return false;

            return true;
        }
        #endregion
    }
}
