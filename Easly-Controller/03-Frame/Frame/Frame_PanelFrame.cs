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
            bool IsValid = true;

            IsValid &= base.IsValid(nodeType, nodeTemplateTable);
            IsValid &= Items != null && Items.Count > 0;

            foreach (IFrameFrame Item in Items)
                IsValid &= Item.IsValid(nodeType, nodeTemplateTable);

            Debug.Assert(IsValid);
            return IsValid;
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
            ValidateEmbeddingCellView(context, EmbeddingCellView);

            foreach (IFrameFrame Item in Items)
            {
                IFrameNodeFrame NodeFrame = Item as IFrameNodeFrame;
                Debug.Assert(NodeFrame != null);

                IFrameCellView ItemCellView = NodeFrame.BuildNodeCells(context, EmbeddingCellView);

                // Only add cell views that are not empty and that are not empty collections.
                if (!(ItemCellView is IFrameEmptyCellView) && !(ItemCellView is IFrameCellViewCollection AsCollection && AsCollection.CellViewList.Count == 0 && !AsCollection.IsAssignedToTable))
                    CellViewList.Add(ItemCellView);
            }

            return EmbeddingCellView;
        }

        /// <summary></summary>
        private protected virtual void ValidateEmbeddingCellView(IFrameCellViewTreeContext context, IFrameCellViewCollection embeddingCellView)
        {
            Debug.Assert(embeddingCellView.StateView == context.StateView);
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
            ValidateEmbeddingCellView(context, EmbeddingCellView);
            IFrameCellView ItemCellView = null;

            foreach (IFrameFrame Item in Items)
            {
                bool IsHandled = false;

                if (Item is IFrameBlockFrame AsBlockFrame)
                {
                    ItemCellView = AsBlockFrame.BuildBlockCells(context);
                    IsHandled = true;
                }
                else if (Item is FramePlaceholderFrame AsPlaceholderFrame)
                {
                    ItemCellView = BuildBlockCellsForPlaceholderFrame(context, AsPlaceholderFrame, EmbeddingCellView, BlockState);
                    IsHandled = true;
                }

                /*
                else if (Item is IFrameNodeFrame AsNodeFrame)
                {
                    ItemCellView = AsNodeFrame.BuildNodeCells(context, EmbeddingCellView);
                    IsHandled = true;
                }*/

                Debug.Assert(IsHandled);

                CellViewList.Add(ItemCellView);
            }

            return EmbeddingCellView;
        }

        /// <summary></summary>
        private protected virtual IFrameCellView BuildBlockCellsForPlaceholderFrame(IFrameCellViewTreeContext context, IFramePlaceholderFrame frame, IFrameCellViewCollection embeddingCellView, IFrameBlockState blockState)
        {
            IFrameCellView ItemCellView = null;
            bool IsHandled = false;

            if (frame.PropertyName == nameof(IBlock.ReplicationPattern))
            {
                ItemCellView = BuildPlaceholderCells(context, embeddingCellView, blockState.PatternState);
                IsHandled = true;
            }
            else if (frame.PropertyName == nameof(IBlock.SourceIdentifier))
            {
                ItemCellView = BuildPlaceholderCells(context, embeddingCellView, blockState.SourceState);
                IsHandled = true;
            }

            Debug.Assert(IsHandled);

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
