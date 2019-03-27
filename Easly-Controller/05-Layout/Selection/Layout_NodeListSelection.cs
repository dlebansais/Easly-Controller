namespace EaslyController.Layout
{
    using System.Diagnostics;
    using BaseNode;
    using EaslyController.Controller;
    using EaslyController.Focus;

    /// <summary>
    /// A selection of nodes in a list.
    /// </summary>
    public interface ILayoutNodeListSelection : IFocusNodeListSelection, ILayoutContentSelection
    {
    }

    /// <summary>
    /// A selection of nodes in a list.
    /// </summary>
    public class LayoutNodeListSelection : FocusNodeListSelection, ILayoutNodeListSelection
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutNodeListSelection"/> class.
        /// </summary>
        /// <param name="stateView">The state view that encompasses the selection.</param>
        /// <param name="propertyName">The property name.</param>
        /// <param name="startIndex">Index of the first selected node in the list.</param>
        /// <param name="endIndex">Index of the last selected node in the list.</param>
        public LayoutNodeListSelection(ILayoutNodeStateView stateView, string propertyName, int startIndex, int endIndex)
            : base(stateView, propertyName, startIndex, endIndex)
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
            ILayoutNodeState State = StateView.State;
            ILayoutListInner ParentInner = State.PropertyToInner(PropertyName) as ILayoutListInner;
            Debug.Assert(ParentInner != null);

            ILayoutControllerView ControllerView = StateView.ControllerView;
            Debug.Assert(ControllerView.PrintContext != null);
            ControllerView.UpdateLayout();

            Debug.Assert(StartIndex <= EndIndex);

            ILayoutNodeStateView FirstStateView = ControllerView.StateViewTable[ParentInner.StateList[StartIndex]];
            Point Origin = FirstStateView.CellOrigin.Opposite;

            for (int i = StartIndex; i < EndIndex; i++)
            {
                ILayoutNodeStateView StateView = ControllerView.StateViewTable[ParentInner.StateList[i]];
                Debug.Assert(RegionHelper.IsValid(StateView.ActualCellSize));

                StateView.PrintCells(Origin);
            }
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxInsertionListNodeIndex object.
        /// </summary>
        private protected override IFocusInsertionListNodeIndex CreateListNodeIndex(INode parentNode, string propertyName, INode node, int index)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutNodeListSelection));
            return new LayoutInsertionListNodeIndex(parentNode, propertyName, node, index);
        }
        #endregion
    }
}
