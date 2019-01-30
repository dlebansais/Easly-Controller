namespace EaslyController.Frame
{
    using System;
    using System.Diagnostics;
    using BaseNodeHelper;

    /// <summary>
    /// Base frame for a block list.
    /// </summary>
    public interface IFrameBlockListFrame : IFrameNamedFrame, IFrameNodeFrame
    {
    }

    /// <summary>
    /// Base frame for a block list.
    /// </summary>
    public abstract class FrameBlockListFrame : FrameNamedFrame, IFrameBlockListFrame
    {
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

            if (!NodeTreeHelperBlockList.IsBlockListProperty(nodeType, PropertyName, out Type ChildInterfaceType, out Type ChildNodeType))
                return false;

            return true;
        }

        /// <summary>
        /// Create cells for the provided state view.
        /// </summary>
        /// <param name="context">Context used to build the cell view tree.</param>
        /// <param name="parentCellView">The parent cell view.</param>
        public virtual IFrameCellView BuildNodeCells(IFrameCellViewTreeContext context, IFrameCellViewCollection parentCellView)
        {
            IFrameNodeState State = context.StateView.State;
            Debug.Assert(State != null);
            Debug.Assert(State.InnerTable != null);
            Debug.Assert(State.InnerTable.ContainsKey(PropertyName));

            IFrameBlockListInner<IFrameBrowsingBlockNodeIndex> Inner = State.InnerTable[PropertyName] as IFrameBlockListInner<IFrameBrowsingBlockNodeIndex>;
            Debug.Assert(Inner != null);

            IFrameBlockStateViewDictionary BlockStateViewTable = context.ControllerView.BlockStateViewTable;
            IFrameCellViewList CellViewList = CreateCellViewList();

            IFrameCellViewCollection EmbeddingCellView = CreateEmbeddingCellView(context.StateView, CellViewList);

            Type BlockType = Inner.BlockType;
            IFrameTemplateSet TemplateSet = context.ControllerView.TemplateSet;
            IFrameBlockTemplate BlockTemplate = TemplateSet.BlockTypeToTemplate(BlockType);

            foreach (IFrameBlockState BlockState in Inner.BlockStateList)
            {
                Debug.Assert(context.ControllerView.BlockStateViewTable.ContainsKey(BlockState));
                IFrameBlockStateView BlockStateView = context.ControllerView.BlockStateViewTable[BlockState];

                context.SetBlockStateView(BlockStateView);
                BlockStateView.BuildRootCellView(context);
                IFrameBlockCellView BlockCellView = CreateBlockCellView(context.StateView, EmbeddingCellView, BlockStateView);

                CellViewList.Add(BlockCellView);
            }

            AssignEmbeddingCellView(context.StateView, EmbeddingCellView);

            return EmbeddingCellView;
        }

        /// <summary></summary>
        private protected virtual void AssignEmbeddingCellView(IFrameNodeStateView stateView, IFrameAssignableCellView embeddingCellView)
        {
            embeddingCellView.AssignToCellViewTable();
            stateView.AssignCellViewTable(PropertyName, embeddingCellView);
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxCellViewList object.
        /// </summary>
        private protected virtual IFrameCellViewList CreateCellViewList()
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameBlockListFrame));
            return new FrameCellViewList();
        }

        /// <summary>
        /// Creates a IxxxBlockCellView object.
        /// </summary>
        private protected virtual IFrameBlockCellView CreateBlockCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, IFrameBlockStateView blockStateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameBlockListFrame));
            return new FrameBlockCellView(stateView, parentCellView, blockStateView);
        }

        /// <summary>
        /// Creates a IxxxCellViewCollection object.
        /// </summary>
        private protected abstract IFrameCellViewCollection CreateEmbeddingCellView(IFrameNodeStateView stateView, IFrameCellViewList list);
        #endregion
    }
}
