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
        /// <param name="parentCellView">The collection of cell views containing this view. Null for the root of the cell tree.</param>
        public virtual IFrameCellView BuildBlockCells(IFrameCellViewTreeContext context, IFrameCellViewCollection parentCellView)
        {
            FrameBlockStateView BlockStateView = context.BlockStateView;
            IFrameBlockState BlockState = BlockStateView.BlockState;
            Debug.Assert(BlockState != null);

            IFrameBlockTemplate BlockTemplate = ParentTemplate as IFrameBlockTemplate;
            Debug.Assert(BlockTemplate != null);

            FrameNodeStateViewDictionary StateViewTable = context.ControllerView.StateViewTable;
            FrameCellViewList CellViewList = CreateCellViewList();
            IFrameCellViewCollection EmbeddingCellView = CreateEmbeddingCellView(context.StateView, parentCellView, CellViewList);
            ValidateEmbeddingCellView(context, EmbeddingCellView);

            foreach (IFrameNodeState ChildState in BlockState.StateList)
            {
                Debug.Assert(StateViewTable.ContainsKey(ChildState));

                IFrameNodeStateView StateView = context.StateView;
                IFrameNodeStateView ChildStateView = (IFrameNodeStateView)StateViewTable[ChildState];

                Debug.Assert(ChildStateView.RootCellView == null);
                context.SetChildStateView(ChildStateView);
                ChildStateView.BuildRootCellView(context);
                context.RestoreParentStateView(StateView);
                Debug.Assert(ChildStateView.RootCellView != null);

                IFrameContainerCellView FrameCellView = CreateFrameCellView(context.StateView, EmbeddingCellView, ChildStateView);
                ValidateContainerCellView(context.StateView, EmbeddingCellView, ChildStateView, FrameCellView);

                ChildStateView.SetContainerCellView(FrameCellView);

                CellViewList.Add(FrameCellView);
            }

            AssignEmbeddingCellView(BlockStateView, EmbeddingCellView);

            return EmbeddingCellView;
        }

        private protected virtual void ValidateEmbeddingCellView(IFrameCellViewTreeContext context, IFrameCellViewCollection embeddingCellView)
        {
            Debug.Assert(embeddingCellView.StateView == context.StateView);
            IFrameCellViewCollection ParentCellView = embeddingCellView.ParentCellView;
        }

        private protected virtual void ValidateContainerCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, IFrameNodeStateView childStateView, IFrameContainerCellView containerCellView)
        {
            Debug.Assert(containerCellView.StateView == stateView);
            Debug.Assert(containerCellView.ParentCellView == parentCellView);
            Debug.Assert(containerCellView.ChildStateView == childStateView);
        }

        private protected virtual void AssignEmbeddingCellView(FrameBlockStateView blockStateView, IFrameCellViewCollection embeddingCellView)
        {
            blockStateView.AssignEmbeddingCellView(embeddingCellView);
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxCellViewList object.
        /// </summary>
        private protected virtual FrameCellViewList CreateCellViewList()
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameCollectionPlaceholderFrame));
            return new FrameCellViewList();
        }

        /// <summary>
        /// Creates a IxxxContainerCellView object.
        /// </summary>
        private protected virtual IFrameContainerCellView CreateFrameCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, IFrameNodeStateView childStateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameCollectionPlaceholderFrame));
            return new FrameContainerCellView(stateView, parentCellView, childStateView, this);
        }

        /// <summary>
        /// Creates a IxxxCellViewCollection object.
        /// </summary>
        private protected abstract IFrameCellViewCollection CreateEmbeddingCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, FrameCellViewList list);
        #endregion
    }
}
