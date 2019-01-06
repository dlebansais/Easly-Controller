namespace EaslyController.Frame
{
    /// <summary>
    /// Frame describing a value property (or string) in a node.
    /// </summary>
    public interface IFrameValueFrame : IFrameNamedFrame, IFrameNodeFrame
    {
    }

    /// <summary>
    /// Frame describing a value property (or string) in a node.
    /// </summary>
    public class FrameValueFrame : FrameNamedFrame, IFrameValueFrame
    {
        #region Client Interface
        /// <summary>
        /// Create cells for the provided state view.
        /// </summary>
        /// <param name="controllerView">The view in cells are created.</param>
        /// <param name="stateView">The state view for which to create cells.</param>
        /// <param name="parentCellView">The parent cell view.</param>
        public virtual IFrameCellView BuildNodeCells(IFrameControllerView controllerView, IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView)
        {
            IFrameVisibleCellView EmbeddingCellView = CreateFrameCellView(stateView);
            //stateView.AssignCellViewTable(PropertyName, EmbeddingCellView);

            return EmbeddingCellView;
        }

        /// <summary>
        /// Clears the cell view tree for this view.
        /// </summary>
        /// <param name="controllerView">The view in which the cell tree is cleared.</param>
        public virtual void ClearRootCellView(IFrameControllerView controllerView, IFrameNodeStateView stateView)
        {
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxVisibleCellView object.
        /// </summary>
        protected virtual IFrameVisibleCellView CreateFrameCellView(IFrameNodeStateView stateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameValueFrame));
            return new FrameVisibleCellView(stateView);
        }
        #endregion
    }
}
