namespace EaslyController.Frame
{
    using System.Diagnostics;

    /// <summary>
    /// Base frame for a placeholder node in a block list.
    /// </summary>
    public interface IFrameCollectionPlaceholderFrame : IFrameFrame, IFrameBlockFrame
    {
    }

    /// <summary>
    /// Base frame for a placeholder node in a block list.
    /// </summary>
    public abstract class FrameCollectionPlaceholderFrame : FrameFrame, IFrameCollectionPlaceholderFrame
    {
        #region Client Interface
        /// <summary>
        /// Create cells for the provided state view.
        /// </summary>
        /// <param name="context">Context used to build the cell view tree.</param>
        public virtual IFrameCellView BuildBlockCells(IFrameCellViewTreeContext context)
        {
            IFrameBlockStateView BlockStateView = context.BlockStateView;
            IFrameBlockState BlockState = BlockStateView.BlockState;
            Debug.Assert(BlockState != null);

            IFrameBlockTemplate BlockTemplate = ParentTemplate as IFrameBlockTemplate;
            Debug.Assert(BlockTemplate != null);

            IFrameStateViewDictionary StateViewTable = context.ControllerView.StateViewTable;
            IFrameCellViewList CellViewList = CreateCellViewList();
            IFrameCellViewCollection EmbeddingCellView = CreateEmbeddingCellView(context.StateView, CellViewList);

            foreach (IFrameNodeState ChildState in BlockState.StateList)
            {
                Debug.Assert(StateViewTable.ContainsKey(ChildState));

                IFrameNodeStateView StateView = context.StateView;
                IFrameNodeStateView ChildStateView = StateViewTable[ChildState];

                Debug.Assert(ChildStateView.RootCellView == null);
                context.SetChildStateView(ChildStateView);
                ChildStateView.BuildRootCellView(context);
                context.RestoreParentStateView(StateView);
                Debug.Assert(ChildStateView.RootCellView != null);

                IFrameCellView FrameCellView = CreateFrameCellView(context.StateView, EmbeddingCellView, ChildStateView);
                CellViewList.Add(FrameCellView);
            }

            AssignEmbeddingCellView(BlockStateView, EmbeddingCellView);

            return EmbeddingCellView;
        }

        /// <summary></summary>
        protected virtual void AssignEmbeddingCellView(IFrameBlockStateView blockStateView, IFrameCellViewCollection embeddingCellView)
        {
            blockStateView.AssignEmbeddingCellView(embeddingCellView);
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxCellViewList object.
        /// </summary>
        protected virtual IFrameCellViewList CreateCellViewList()
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameCollectionPlaceholderFrame));
            return new FrameCellViewList();
        }

        /// <summary>
        /// Creates a IxxxContainerCellView object.
        /// </summary>
        protected virtual IFrameContainerCellView CreateFrameCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, IFrameNodeStateView childStateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameCollectionPlaceholderFrame));
            return new FrameContainerCellView(stateView, parentCellView, childStateView);
        }

        /// <summary>
        /// Creates a IxxxCellViewCollection object.
        /// </summary>
        protected abstract IFrameCellViewCollection CreateEmbeddingCellView(IFrameNodeStateView stateView, IFrameCellViewList list);
        #endregion
    }
}
