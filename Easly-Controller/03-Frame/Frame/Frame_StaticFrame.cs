namespace EaslyController.Frame
{
    /// <summary>
    /// Frame for decoration purpose only.
    /// </summary>
    public interface IFrameStaticFrame : IFrameFrame, IFrameNodeFrame
    {
    }

    /// <summary>
    /// Frame for decoration purpose only.
    /// </summary>
    public abstract class FrameStaticFrame : FrameFrame, IFrameStaticFrame
    {
        #region Client Interface
        /// <summary>
        /// Create cells for the provided state view.
        /// </summary>
        /// <param name="context">Context used to build the cell view tree.</param>
        /// <param name="parentCellView">The parent cell view.</param>
        public virtual IFrameCellView BuildNodeCells(IFrameCellViewTreeContext context, IFrameCellViewCollection parentCellView)
        {
            IFrameVisibleCellView CellView = CreateVisibleCellView(context.StateView);
            ValidateVisibleCellView(context, CellView);
            return CellView;
        }

        /// <summary></summary>
        private protected abstract void ValidateVisibleCellView(IFrameCellViewTreeContext context, IFrameVisibleCellView cellView);
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxVisibleCellView object.
        /// </summary>
        private protected abstract IFrameVisibleCellView CreateVisibleCellView(IFrameNodeStateView stateView);
        #endregion
    }
}
