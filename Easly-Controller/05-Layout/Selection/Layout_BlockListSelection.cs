namespace EaslyController.Layout
{
    using System.Diagnostics;
    using BaseNode;
    using EaslyController.Controller;
    using EaslyController.Focus;

    /// <summary>
    /// A selection of blocks in a block list.
    /// </summary>
    public interface ILayoutBlockListSelection : IFocusBlockListSelection, ILayoutContentSelection
    {
    }

    /// <summary>
    /// A selection of blocks in a block list.
    /// </summary>
    public class LayoutBlockListSelection : FocusBlockListSelection, ILayoutBlockListSelection
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutBlockListSelection"/> class.
        /// </summary>
        /// <param name="stateView">The state view that encompasses the selection.</param>
        /// <param name="propertyName">The property name.</param>
        /// <param name="startIndex">Index of the first selected block.</param>
        /// <param name="endIndex">Index of the last selected block.</param>
        public LayoutBlockListSelection(ILayoutNodeStateView stateView, string propertyName, int startIndex, int endIndex)
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
            ILayoutBlockListInner ParentInner = State.PropertyToInner(PropertyName) as ILayoutBlockListInner;
            Debug.Assert(ParentInner != null);

            ILayoutControllerView ControllerView = StateView.ControllerView;
            Debug.Assert(ControllerView.PrintContext != null);
            ControllerView.UpdateLayout();

            Debug.Assert(StartIndex <= EndIndex);

            ILayoutBlockStateView FirstBlockStateView = ControllerView.BlockStateViewTable[ParentInner.BlockStateList[StartIndex]];
            Point Origin = FirstBlockStateView.CellOrigin.Opposite;

            for (int i = StartIndex; i < EndIndex; i++)
            {
                ILayoutBlockStateView BlockStateView = ControllerView.BlockStateViewTable[ParentInner.BlockStateList[i]];
                Debug.Assert(RegionHelper.IsValid(BlockStateView.ActualCellSize));

                BlockStateView.PrintCells(Origin);
            }
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxInsertionNewBlockNodeIndex object.
        /// </summary>
        private protected override IFocusInsertionNewBlockNodeIndex CreateNewBlockNodeIndex(INode parentNode, string propertyName, INode node, int blockIndex, IPattern patternNode, IIdentifier sourceNode)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutBlockListSelection));
            return new LayoutInsertionNewBlockNodeIndex(parentNode, propertyName, node, blockIndex, patternNode, sourceNode);
        }

        /// <summary>
        /// Creates a IxxxInsertionExistingBlockNodeIndex object.
        /// </summary>
        private protected override IFocusInsertionExistingBlockNodeIndex CreateExistingBlockNodeIndex(INode parentNode, string propertyName, INode node, int blockIndex, int index)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutBlockListSelection));
            return new LayoutInsertionExistingBlockNodeIndex(parentNode, propertyName, node, blockIndex, index);
        }
        #endregion
    }
}
