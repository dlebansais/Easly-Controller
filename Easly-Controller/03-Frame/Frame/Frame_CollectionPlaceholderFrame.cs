using System.Diagnostics;

namespace EaslyController.Frame
{
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
        /// <param name="controllerView">The view in cells are created.</param>
        /// <param name="stateView">The state view containing <paramref name="blockStateView"/> for which to create cells.</param>
        /// <param name="blockStateView">The block state view for which to create cells.</param>
        public virtual IFrameCellView BuildBlockCells(IFrameControllerView controllerView, IFrameNodeStateView stateView, IFrameBlockStateView blockStateView)
        {
            IFrameBlockState BlockState = blockStateView.BlockState;
            Debug.Assert(BlockState != null);

            IFrameBlockTemplate BlockTemplate = ParentTemplate as IFrameBlockTemplate;
            Debug.Assert(BlockTemplate != null);

            IFrameStateViewDictionary StateViewTable = controllerView.StateViewTable;
            IFrameCellViewList CellViewList = CreateCellViewList();

            foreach (IFrameNodeState ChildState in BlockState.StateList)
            {
                Debug.Assert(StateViewTable.ContainsKey(ChildState));

                IFrameNodeStateView ChildStateView = StateViewTable[ChildState];
                IFrameCellView FrameCellView = CreateFrameCellView(stateView, ChildStateView);
                CellViewList.Add(FrameCellView);
            }

            IFrameMutableCellViewCollection EmbeddingCellView = CreateEmbeddingCellView(stateView, CellViewList);
            blockStateView.AssignEmbeddingCellView(EmbeddingCellView);

            return EmbeddingCellView;
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
        protected virtual IFrameContainerCellView CreateFrameCellView(IFrameNodeStateView stateView, IFrameNodeStateView childStateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameCollectionPlaceholderFrame));
            return new FrameContainerCellView(stateView, childStateView);
        }

        /// <summary>
        /// Creates a IxxxMutableCellViewCollection object.
        /// </summary>
        protected abstract IFrameMutableCellViewCollection CreateEmbeddingCellView(IFrameNodeStateView stateView, IFrameCellViewList list);
        #endregion
    }
}
