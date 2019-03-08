namespace EaslyController.Layout
{
    using System.Diagnostics;
    using EaslyController.Controller;
    using EaslyController.Focus;

    /// <summary>
    /// A selection of a node an all its content and children.
    /// </summary>
    public interface ILayoutNodeSelection : IFocusNodeSelection, ILayoutSelection
    {
    }

    /// <summary>
    /// A selection of a node an all its content and children.
    /// </summary>
    public class LayoutNodeSelection : FocusNodeSelection, ILayoutNodeSelection
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutNodeSelection"/> class.
        /// </summary>
        /// <param name="stateView">The selected state view.</param>
        public LayoutNodeSelection(ILayoutNodeStateView stateView)
            : base(stateView)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The state view that encompasses the selection.
        /// </summary>
        public new ILayoutNodeStateView StateView { get { return (ILayoutNodeStateView)base.StateView; } }
        #endregion

        #region Client Interface
        /// <summary>
        /// Prints the selection.
        /// </summary>
        public virtual void Print()
        {
            ILayoutControllerView ControllerView = StateView.ControllerView;
            Debug.Assert(ControllerView.PrintContext != null);
            ControllerView.UpdateLayout();

            Debug.Assert(RegionHelper.IsValid(StateView.ActualCellSize));

            Point Origin = StateView.CellOrigin.Opposite;
            StateView.PrintCells(Origin);
        }
        #endregion
    }
}
