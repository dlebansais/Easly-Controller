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
        /// <param name="commentFrameCount">Number of comment frames found so far.</param>
        public override bool IsValid(Type nodeType, FrameTemplateReadOnlyDictionary nodeTemplateTable, ref int commentFrameCount)
        {
            bool IsValid = true;

            IsValid &= base.IsValid(nodeType, nodeTemplateTable, ref commentFrameCount);
            IsValid &= NodeTreeHelperBlockList.IsBlockListProperty(nodeType, PropertyName, /*out Type ChildInterfaceType,*/ out Type ChildNodeType);

            Debug.Assert(IsValid);
            return IsValid;
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

            FrameBlockStateViewDictionary BlockStateViewTable = context.ControllerView.BlockStateViewTable;
            FrameCellViewList CellViewList = CreateCellViewList();

            IFrameCellViewCollection EmbeddingCellView = CreateEmbeddingCellView(context.StateView, parentCellView, CellViewList);
            ValidateEmbeddingCellView(context, EmbeddingCellView);

            Type BlockType = Inner.BlockType;
            IFrameTemplateSet TemplateSet = context.ControllerView.TemplateSet;
            IFrameBlockTemplate BlockTemplate = TemplateSet.BlockTypeToTemplate(BlockType);

            foreach (IFrameBlockState BlockState in Inner.BlockStateList)
            {
                Debug.Assert(context.ControllerView.BlockStateViewTable.ContainsKey(BlockState));
                FrameBlockStateView BlockStateView = (FrameBlockStateView)context.ControllerView.BlockStateViewTable[BlockState];

                context.SetBlockStateView(BlockStateView);
                BlockStateView.BuildRootCellView(context);
                IFrameBlockCellView BlockCellView = CreateBlockCellView(context.StateView, EmbeddingCellView, BlockStateView);
                ValidateBlockCellView(context.StateView, EmbeddingCellView, BlockStateView, BlockCellView);

                CellViewList.Add(BlockCellView);
            }

            AssignEmbeddingCellView(context.StateView, EmbeddingCellView);

            return EmbeddingCellView;
        }

        private protected virtual void ValidateEmbeddingCellView(IFrameCellViewTreeContext context, IFrameCellViewCollection embeddingCellView)
        {
            Debug.Assert(embeddingCellView.StateView == context.StateView);
            IFrameCellViewCollection ParentCellView = embeddingCellView.ParentCellView;
        }

        private protected virtual void ValidateBlockCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, FrameBlockStateView blockStateView, IFrameBlockCellView blockCellView)
        {
            Debug.Assert(blockCellView.StateView == stateView);
            Debug.Assert(blockCellView.ParentCellView == parentCellView);
            Debug.Assert(blockCellView.BlockStateView == blockStateView);
        }

        private protected virtual void AssignEmbeddingCellView(IFrameNodeStateView stateView, IFrameAssignableCellView embeddingCellView)
        {
            embeddingCellView.AssignToCellViewTable();
            ((IFrameReplaceableStateView)stateView).AssignCellViewTable(PropertyName, embeddingCellView);
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxCellViewList object.
        /// </summary>
        private protected virtual FrameCellViewList CreateCellViewList()
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameBlockListFrame));
            return new FrameCellViewList();
        }

        /// <summary>
        /// Creates a IxxxBlockCellView object.
        /// </summary>
        private protected virtual IFrameBlockCellView CreateBlockCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, FrameBlockStateView blockStateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameBlockListFrame));
            return new FrameBlockCellView(stateView, parentCellView, blockStateView);
        }

        /// <summary>
        /// Creates a IxxxCellViewCollection object.
        /// </summary>
        private protected abstract IFrameCellViewCollection CreateEmbeddingCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, FrameCellViewList list);
        #endregion
    }
}
