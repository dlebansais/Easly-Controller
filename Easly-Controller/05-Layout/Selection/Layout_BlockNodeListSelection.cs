namespace EaslyController.Layout
{
    using System.Diagnostics;
    using BaseNode;
    using EaslyController.Controller;
    using EaslyController.Focus;

    /// <summary>
    /// A selection of nodes in a block of a list block.
    /// </summary>
    public interface ILayoutBlockNodeListSelection : IFocusBlockNodeListSelection, ILayoutContentSelection
    {
    }

    /// <summary>
    /// A selection of nodes in a block of a list block.
    /// </summary>
    public class LayoutBlockNodeListSelection : FocusBlockNodeListSelection, ILayoutBlockNodeListSelection
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutBlockNodeListSelection"/> class.
        /// </summary>
        /// <param name="stateView">The state view that encompasses the selection.</param>
        /// <param name="propertyName">The property name.</param>
        /// <param name="blockIndex">Index of the block.</param>
        /// <param name="startIndex">Index of the first selected node in the list.</param>
        /// <param name="endIndex">Index of the last selected node in the list.</param>
        public LayoutBlockNodeListSelection(ILayoutNodeStateView stateView, string propertyName, int blockIndex, int startIndex, int endIndex)
            : base(stateView, propertyName, blockIndex, startIndex, endIndex)
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
            ILayoutBlockListInner ParentInner = State.PropertyToInner(PropertyName) as ILayoutBlockListInner;
            Debug.Assert(ParentInner != null);
            Debug.Assert(BlockIndex >= 0 && BlockIndex < ParentInner.BlockStateList.Count);

            ILayoutBlockState BlockState = (ILayoutBlockState)ParentInner.BlockStateList[BlockIndex];

            LayoutControllerView ControllerView = StateView.ControllerView;
            Debug.Assert(ControllerView.PrintContext != null);
            ControllerView.UpdateLayout();

            Debug.Assert(StartIndex <= EndIndex);

            ILayoutNodeStateView FirstStateView = (ILayoutNodeStateView)ControllerView.StateViewTable[BlockState.StateList[StartIndex]];
            Point Origin = FirstStateView.CellOrigin.Opposite;

            for (int i = StartIndex; i < EndIndex; i++)
            {
                ILayoutNodeStateView StateView = (ILayoutNodeStateView)ControllerView.StateViewTable[BlockState.StateList[i]];
                Debug.Assert(RegionHelper.IsValid(StateView.ActualCellSize));

                StateView.PrintCells(Origin);
            }
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxInsertionExistingBlockNodeIndex object.
        /// </summary>
        private protected override IFocusInsertionExistingBlockNodeIndex CreateExistingBlockNodeIndex(Node parentNode, string propertyName, Node node, int blockIndex, int index)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutBlockNodeListSelection));
            return new LayoutInsertionExistingBlockNodeIndex(parentNode, propertyName, node, blockIndex, index);
        }
        #endregion
    }
}
