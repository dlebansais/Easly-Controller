using BaseNodeHelper;
using System;
using System.Diagnostics;

namespace EaslyController.Frame
{
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
        public override bool IsValid(Type nodeType)
        {
            if (!base.IsValid(nodeType))
                return false;

            if (!NodeTreeHelperBlockList.IsBlockListProperty(nodeType, PropertyName, out Type ChildInterfaceType, out Type ChildNodeType))
                return false;

            return true;
        }

        /// <summary>
        /// Create cells for the provided state view.
        /// </summary>
        /// <param name="controllerView">The view in cells are created.</param>
        /// <param name="stateView">The state view for which to create cells.</param>
        /// <param name="parentCellView">The parent cell view.</param>
        public virtual IFrameCellView BuildNodeCells(IFrameControllerView controllerView, IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView)
        {
            IFrameNodeState State = stateView.State;
            Debug.Assert(State != null);
            Debug.Assert(State.InnerTable != null);
            Debug.Assert(State.InnerTable.ContainsKey(PropertyName));

            IFrameBlockListInner<IFrameBrowsingBlockNodeIndex> Inner = State.InnerTable[PropertyName] as IFrameBlockListInner<IFrameBrowsingBlockNodeIndex>;
            Debug.Assert(Inner != null);

            IFrameBlockStateViewDictionary BlockStateViewTable = controllerView.BlockStateViewTable;
            IFrameCellViewList CellViewList = CreateCellViewList();

            IFrameCellViewCollection EmbeddingCellView = CreateEmbeddingCellView(stateView, CellViewList);
            stateView.AssignCellViewTable(PropertyName, EmbeddingCellView);

            Type BlockType = Inner.BlockType;
            IFrameTemplateSet TemplateSet = controllerView.TemplateSet;
            IFrameBlockTemplate BlockTemplate = TemplateSet.BlockTypeToTemplate(BlockType);

            foreach (IFrameBlockState BlockState in Inner.BlockStateList)
            {
                Debug.Assert(controllerView.BlockStateViewTable.ContainsKey(BlockState));
                IFrameBlockStateView BlockStateView = controllerView.BlockStateViewTable[BlockState];

                BlockStateView.BuildRootCellView(stateView);
                IFrameBlockCellView BlockCellView = CreateBlockCellView(stateView, EmbeddingCellView, BlockStateView);

                CellViewList.Add(BlockCellView);
            }

            return EmbeddingCellView;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxCellViewList object.
        /// </summary>
        protected virtual IFrameCellViewList CreateCellViewList()
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameBlockListFrame));
            return new FrameCellViewList();
        }

        /// <summary>
        /// Creates a IxxxBlockCellView object.
        /// </summary>
        protected virtual IFrameBlockCellView CreateBlockCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, IFrameBlockStateView blockStateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameBlockListFrame));
            return new FrameBlockCellView(stateView, parentCellView, blockStateView);
        }

        /// <summary>
        /// Creates a IxxxCellViewCollection object.
        /// </summary>
        protected abstract IFrameCellViewCollection CreateEmbeddingCellView(IFrameNodeStateView stateView, IFrameCellViewList list);
        #endregion
    }
}
