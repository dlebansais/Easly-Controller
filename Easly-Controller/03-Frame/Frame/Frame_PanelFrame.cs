namespace EaslyController.Frame
{
    using System.Diagnostics;
    using BaseNode;
    using NotNullReflection;

    /// <summary>
    /// Base frame for displaying more frames.
    /// </summary>
    public interface IFramePanelFrame : IFrameFrame, IFrameNodeFrame, IFrameBlockFrame
    {
        /// <summary>
        /// List of frames within this frame.
        /// </summary>
        FrameFrameList Items { get; }
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
        public FrameFrameList Items { get; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Checks that a frame is correctly constructed.
        /// </summary>
        /// <param name="nodeType">Type of the node this frame can describe.</param>
        /// <param name="nodeTemplateTable">Table of templates with all frames.</param>
        /// <param name="commentFrameCount">Number of comment frames found so far.</param>
        public override bool IsValid(Type nodeType, FrameTemplateReadOnlyDictionary nodeTemplateTable, ref int commentFrameCount)
        {
            bool IsValid = true;

            IsValid &= base.IsValid(nodeType, nodeTemplateTable, ref commentFrameCount);
            IsValid &= Items != null && Items.Count > 0;

            foreach (IFrameFrame Item in Items)
                IsValid &= Item.IsValid(nodeType, nodeTemplateTable, ref commentFrameCount);

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
            FrameCellViewList CellViewList = CreateCellViewList();
            IFrameCellViewCollection EmbeddingCellView = CreateEmbeddingCellView(context.StateView, parentCellView, CellViewList);
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

        private protected virtual void ValidateEmbeddingCellView(IFrameCellViewTreeContext context, IFrameCellViewCollection embeddingCellView)
        {
            Debug.Assert(embeddingCellView.StateView == context.StateView);
            IFrameCellViewCollection ParentCellView = embeddingCellView.ParentCellView;
        }

        /// <summary>
        /// Create cells for the provided state view.
        /// </summary>
        /// <param name="context">Context used to build the cell view tree.</param>
        /// <param name="parentCellView">The collection of cell views containing this view. Null for the root of the cell tree.</param>
        public virtual IFrameCellView BuildBlockCells(IFrameCellViewTreeContext context, IFrameCellViewCollection parentCellView)
        {
            FrameBlockStateView BlockStateView = context.BlockStateView;
            IFrameBlockState BlockState = BlockStateView.BlockState;
            FrameCellViewList CellViewList = CreateCellViewList();
            IFrameCellViewCollection EmbeddingCellView = CreateEmbeddingCellView(context.StateView, parentCellView, CellViewList);
            ValidateEmbeddingCellView(context, EmbeddingCellView);
            IFrameCellView ItemCellView = null;

            foreach (IFrameFrame Item in Items)
            {
                bool IsHandled = false;

                if (Item is IFrameBlockFrame AsBlockFrame)
                {
                    ItemCellView = AsBlockFrame.BuildBlockCells(context, EmbeddingCellView);
                    IsHandled = true;
                }
                else if (Item is FramePlaceholderFrame AsPlaceholderFrame)
                {
                    ItemCellView = BuildBlockCellsForPlaceholderFrame(context, AsPlaceholderFrame, EmbeddingCellView, BlockState);
                    IsHandled = true;
                }

                Debug.Assert(IsHandled);

                CellViewList.Add(ItemCellView);
            }

            return EmbeddingCellView;
        }

        private protected virtual IFrameCellView BuildBlockCellsForPlaceholderFrame(IFrameCellViewTreeContext context, IFramePlaceholderFrame frame, IFrameCellViewCollection embeddingCellView, IFrameBlockState blockState)
        {
            IFrameCellView ItemCellView = null;
            bool IsHandled = false;

            if (frame.PropertyName == nameof(IBlock.ReplicationPattern))
            {
                ItemCellView = BuildPlaceholderCells(context, embeddingCellView, blockState.PatternState, frame);
                IsHandled = true;
            }
            else if (frame.PropertyName == nameof(IBlock.SourceIdentifier))
            {
                ItemCellView = BuildPlaceholderCells(context, embeddingCellView, blockState.SourceState, frame);
                IsHandled = true;
            }

            Debug.Assert(IsHandled);

            return ItemCellView;
        }

        private protected virtual IFrameCellView BuildPlaceholderCells(IFrameCellViewTreeContext context, IFrameCellViewCollection parentCellView, IFrameNodeState childState, IFramePlaceholderFrame frame)
        {
            FrameNodeStateViewDictionary StateViewTable = context.ControllerView.StateViewTable;
            Debug.Assert(StateViewTable.ContainsKey(childState));

            IFrameNodeStateView StateView = context.StateView;
            IFrameNodeStateView ChildStateView = (IFrameNodeStateView)StateViewTable[childState];

            Debug.Assert(ChildStateView.RootCellView == null);
            context.SetChildStateView(ChildStateView);
            ChildStateView.BuildRootCellView(context);
            context.RestoreParentStateView(StateView);
            Debug.Assert(ChildStateView.RootCellView != null);

            IFrameContainerCellView Result = CreateFrameCellView(context.StateView, parentCellView, ChildStateView, frame);

            ChildStateView.SetContainerCellView(Result);

            return Result;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxFrameList object.
        /// </summary>
        private protected virtual FrameFrameList CreateItems()
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FramePanelFrame>());
            return new FrameFrameList();
        }

        /// <summary>
        /// Creates a IxxxCellViewList object.
        /// </summary>
        private protected virtual FrameCellViewList CreateCellViewList()
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FramePanelFrame>());
            return new FrameCellViewList();
        }

        /// <summary>
        /// Creates a IxxxContainerCellView object.
        /// </summary>
        private protected virtual IFrameContainerCellView CreateFrameCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, IFrameNodeStateView childStateView, IFramePlaceholderFrame frame)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FramePanelFrame>());
            return new FrameContainerCellView(stateView, parentCellView, childStateView, frame);
        }

        /// <summary>
        /// Creates a IxxxCellViewCollection object.
        /// </summary>
        private protected abstract IFrameCellViewCollection CreateEmbeddingCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, FrameCellViewList list);
        #endregion
    }
}
