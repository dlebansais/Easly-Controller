using BaseNode;
using System;
using System.Diagnostics;

namespace EaslyController.Frame
{
    /// <summary>
    /// Base frame for displaying more frames.
    /// </summary>
    public interface IFramePanelFrame : IFrameFrame, IFrameNodeFrame, IFrameBlockFrame
    {
        /// <summary>
        /// List of frames within this frame.
        /// </summary>
        IFrameFrameList Items { get; }
    }

    /// <summary>
    /// Base frame for displaying more frames.
    /// </summary>
    public abstract class FramePanelFrame : FrameFrame, IFramePanelFrame
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of <see cref="FramePanelFrame"/>.
        /// </summary>
        /// <param name="state">The state that will be browsed.</param>
        public FramePanelFrame()
        {
            Items = CreateItems();
        }
        #endregion

        #region Properties
        /// <summary>
        /// List of frames within this frame.
        /// </summary>
        public IFrameFrameList Items { get; set; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Checks that a frame is correctly constructed.
        /// </summary>
        /// <param name="nodeType">Type of the node this frame can describe.</param>
        public override bool IsValid(Type nodeType)
        {
            if (!base.IsValid(nodeType))
                return false;

            if (Items == null || Items.Count == 0)
                return false;

            foreach (IFrameFrame Item in Items)
                if (!Item.IsValid(nodeType))
                    return false;

            return true;
        }

        /// <summary>
        /// Update the reference to the parent frame.
        /// </summary>
        /// <param name="parentTemplate">The parent template.</param>
        /// <param name="parentFrame">The parent frame.</param>
        public override void UpdateParent(IFrameTemplate parentTemplate, IFrameFrame parentFrame)
        {
            base.UpdateParent(parentTemplate, parentFrame);

            foreach (IFrameFrame Item in Items)
                Item.UpdateParent(parentTemplate, this);
        }

        /// <summary>
        /// Create cells for the provided state view.
        /// </summary>
        /// <param name="controllerView">The view in cells are created.</param>
        /// <param name="stateView">The state view for which to create cells.</param>
        /// <param name="parentCellView">The parent cell view.</param>
        public virtual IFrameCellView BuildNodeCells(IFrameControllerView controllerView, IFrameNodeStateView stateView, IFrameMutableCellViewCollection parentCellView)
        {
            IFrameCellViewList CellViewList = CreateCellViewList();
            IFrameMutableCellViewCollection EmbeddingCellView = CreateEmbeddingCellView(stateView, CellViewList);

            foreach (IFrameFrame Item in Items)
            {
                IFrameNodeFrame NodeFrame = Item as IFrameNodeFrame;
                Debug.Assert(NodeFrame != null);

                IFrameCellView ItemCellView = NodeFrame.BuildNodeCells(controllerView, stateView, EmbeddingCellView);
                CellViewList.Add(ItemCellView);
            }

            return EmbeddingCellView;
        }

        /// <summary>
        /// Create cells for the provided state view.
        /// </summary>
        /// <param name="controllerView">The view in cells are created.</param>
        /// <param name="stateView">The state view containing <paramref name="blockStateView"/> for which to create cells.</param>
        /// <param name="blockStateView">The block state view for which to create cells.</param>
        public virtual IFrameCellView BuildBlockCells(IFrameControllerView controllerView, IFrameNodeStateView stateView, IFrameBlockStateView blockStateView)
        {
            IFrameCellViewList CellViewList = CreateCellViewList();
            IFrameMutableCellViewCollection EmbeddingCellView = CreateEmbeddingCellView(stateView, CellViewList);

            foreach (IFrameFrame Item in Items)
                if (Item is IFrameBlockFrame AsBlockFrame)
                {
                    IFrameCellView ItemCellView = AsBlockFrame.BuildBlockCells(controllerView, stateView, blockStateView);
                    CellViewList.Add(ItemCellView);
                }
                else if (Item is FramePlaceholderFrame AsPlaceholderFrame)
                {
                    IFrameBlockState BlockState = blockStateView.BlockState;

                    if (AsPlaceholderFrame.PropertyName == nameof(IBlock.ReplicationPattern))
                        BuildPlaceholderCells(controllerView, stateView, EmbeddingCellView, BlockState.PatternState);

                    else if (AsPlaceholderFrame.PropertyName == nameof(IBlock.SourceIdentifier))
                        BuildPlaceholderCells(controllerView, stateView, EmbeddingCellView, BlockState.SourceState);

                    else
                        throw new ArgumentOutOfRangeException(nameof(Item));
                }
                else if (Item is IFrameNodeFrame AsNodeFrame)
                {
                    IFrameCellView ItemCellView = AsNodeFrame.BuildNodeCells(controllerView, stateView, EmbeddingCellView);
                    CellViewList.Add(ItemCellView);
                }
                else
                    throw new ArgumentOutOfRangeException(nameof(Item));

            return EmbeddingCellView;
        }

        protected virtual IFrameCellView BuildPlaceholderCells(IFrameControllerView controllerView, IFrameNodeStateView stateView, IFrameMutableCellViewCollection parentCellView, IFrameNodeState childState)
        {
            IFrameStateViewDictionary StateViewTable = controllerView.StateViewTable;
            Debug.Assert(StateViewTable.ContainsKey(childState));

            IFrameNodeStateView ChildStateView = StateViewTable[childState];

            Debug.Assert(ChildStateView.RootCellView == null);
            ChildStateView.BuildRootCellView(controllerView);

            return CreateFrameCellView(stateView, parentCellView, ChildStateView);
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxFrameList object.
        /// </summary>
        protected virtual IFrameFrameList CreateItems()
        {
            ControllerTools.AssertNoOverride(this, typeof(FramePanelFrame));
            return new FrameFrameList();
        }

        /// <summary>
        /// Creates a IxxxCellViewList object.
        /// </summary>
        protected virtual IFrameCellViewList CreateCellViewList()
        {
            ControllerTools.AssertNoOverride(this, typeof(FramePanelFrame));
            return new FrameCellViewList();
        }

        /// <summary>
        /// Creates a IxxxContainerCellView object.
        /// </summary>
        protected virtual IFrameContainerCellView CreateFrameCellView(IFrameNodeStateView stateView, IFrameMutableCellViewCollection parentCellView, IFrameNodeStateView childStateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(FramePanelFrame));
            return new FrameContainerCellView(stateView, parentCellView, childStateView);
        }

        /// <summary>
        /// Creates a IxxxMutableCellViewCollection object.
        /// </summary>
        protected abstract IFrameMutableCellViewCollection CreateEmbeddingCellView(IFrameNodeStateView stateView, IFrameCellViewList list);
        #endregion
    }
}
