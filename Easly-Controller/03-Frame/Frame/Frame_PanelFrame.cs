namespace EaslyController.Frame
{
    using System;
    using System.Diagnostics;
    using BaseNode;

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
        /// Initializes a new instance of the <see cref="FramePanelFrame"/> class.
        /// </summary>
        public FramePanelFrame()
        {
            Items = CreateItems();
        }
        #endregion

        #region Properties
        /// <summary>
        /// List of frames within this frame.
        /// </summary>
        public IFrameFrameList Items { get; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Checks that a frame is correctly constructed.
        /// </summary>
        /// <param name="nodeType">Type of the node this frame can describe.</param>
        /// <param name="nodeTemplateTable">Table of templates with all frames.</param>
        public override bool IsValid(Type nodeType, IFrameTemplateReadOnlyDictionary nodeTemplateTable)
        {
            if (!base.IsValid(nodeType, nodeTemplateTable))
                return false;

            if (Items == null || Items.Count == 0)
                return false;

            foreach (IFrameFrame Item in Items)
                if (!Item.IsValid(nodeType, nodeTemplateTable))
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
        /// <param name="context">Context used to build the cell view tree.</param>
        /// <param name="parentCellView">The parent cell view.</param>
        public virtual IFrameCellView BuildNodeCells(IFrameCellViewTreeContext context, IFrameCellViewCollection parentCellView)
        {
            IFrameCellViewList CellViewList = CreateCellViewList();
            IFrameCellViewCollection EmbeddingCellView = CreateEmbeddingCellView(context.StateView, CellViewList);

            foreach (IFrameFrame Item in Items)
            {
                IFrameNodeFrame NodeFrame = Item as IFrameNodeFrame;
                Debug.Assert(NodeFrame != null);

                IFrameCellView ItemCellView = NodeFrame.BuildNodeCells(context, EmbeddingCellView);

                // Only add cell views that are not empty and that are not empty collections.
                if (ItemCellView is IFrameEmptyCellView)
                { }
                else if (ItemCellView is IFrameCellViewCollection AsCollection && AsCollection.CellViewList.Count == 0 && !AsCollection.IsAssignedToTable)
                { }
                else
                    CellViewList.Add(ItemCellView);
            }

            return EmbeddingCellView;
        }

        /// <summary>
        /// Create cells for the provided state view.
        /// </summary>
        /// <param name="context">Context used to build the cell view tree.</param>
        public virtual IFrameCellView BuildBlockCells(IFrameCellViewTreeContext context)
        {
            IFrameBlockStateView BlockStateView = context.BlockStateView;
            IFrameBlockState BlockState = BlockStateView.BlockState;
            IFrameCellViewList CellViewList = CreateCellViewList();
            IFrameCellViewCollection EmbeddingCellView = CreateEmbeddingCellView(context.StateView, CellViewList);
            IFrameCellView ItemCellView;

            foreach (IFrameFrame Item in Items)
            {
                if (Item is IFrameBlockFrame AsBlockFrame)
                    ItemCellView = AsBlockFrame.BuildBlockCells(context);
                else if (Item is FramePlaceholderFrame AsPlaceholderFrame)
                    ItemCellView = BuildBlockCellsForPlaceholderFrame(context, AsPlaceholderFrame, EmbeddingCellView, BlockState);
                else if (Item is IFrameNodeFrame AsNodeFrame)
                    ItemCellView = AsNodeFrame.BuildNodeCells(context, EmbeddingCellView);
                else
                    throw new ArgumentOutOfRangeException(nameof(Item));

                CellViewList.Add(ItemCellView);
            }

            return EmbeddingCellView;
        }

        /// <summary></summary>
        private protected virtual IFrameCellView BuildBlockCellsForPlaceholderFrame(IFrameCellViewTreeContext context, IFramePlaceholderFrame frame, IFrameCellViewCollection embeddingCellView, IFrameBlockState blockState)
        {
            IFrameCellView ItemCellView;

            if (frame.PropertyName == nameof(IBlock.ReplicationPattern))
                ItemCellView = BuildPlaceholderCells(context, embeddingCellView, blockState.PatternState);
            else if (frame.PropertyName == nameof(IBlock.SourceIdentifier))
                ItemCellView = BuildPlaceholderCells(context, embeddingCellView, blockState.SourceState);
            else
                throw new ArgumentOutOfRangeException(nameof(frame));

            return ItemCellView;
        }

        /// <summary></summary>
        private protected virtual IFrameCellView BuildPlaceholderCells(IFrameCellViewTreeContext context, IFrameCellViewCollection parentCellView, IFrameNodeState childState)
        {
            IFrameStateViewDictionary StateViewTable = context.ControllerView.StateViewTable;
            Debug.Assert(StateViewTable.ContainsKey(childState));

            IFrameNodeStateView StateView = context.StateView;
            IFrameNodeStateView ChildStateView = StateViewTable[childState];

            Debug.Assert(ChildStateView.RootCellView == null);
            context.SetChildStateView(ChildStateView);
            ChildStateView.BuildRootCellView(context);
            context.RestoreParentStateView(StateView);
            Debug.Assert(ChildStateView.RootCellView != null);

            return CreateFrameCellView(context.StateView, parentCellView, ChildStateView);
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxFrameList object.
        /// </summary>
        private protected virtual IFrameFrameList CreateItems()
        {
            ControllerTools.AssertNoOverride(this, typeof(FramePanelFrame));
            return new FrameFrameList();
        }

        /// <summary>
        /// Creates a IxxxCellViewList object.
        /// </summary>
        private protected virtual IFrameCellViewList CreateCellViewList()
        {
            ControllerTools.AssertNoOverride(this, typeof(FramePanelFrame));
            return new FrameCellViewList();
        }

        /// <summary>
        /// Creates a IxxxContainerCellView object.
        /// </summary>
        private protected virtual IFrameContainerCellView CreateFrameCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, IFrameNodeStateView childStateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(FramePanelFrame));
            return new FrameContainerCellView(stateView, parentCellView, childStateView);
        }

        /// <summary>
        /// Creates a IxxxCellViewCollection object.
        /// </summary>
        private protected abstract IFrameCellViewCollection CreateEmbeddingCellView(IFrameNodeStateView stateView, IFrameCellViewList list);
        #endregion
    }
}
