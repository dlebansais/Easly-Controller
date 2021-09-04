namespace EaslyController.Frame
{
    /// <summary>
    /// Frame for a placeholder node in a block list displayed vertically.
    /// </summary>
    public interface IFrameVerticalCollectionPlaceholderFrame : IFrameCollectionPlaceholderFrame
    {
    }

    /// <summary>
    /// Frame for a placeholder node in a block list displayed vertically.
    /// </summary>
    public class FrameVerticalCollectionPlaceholderFrame : FrameCollectionPlaceholderFrame, IFrameVerticalCollectionPlaceholderFrame
    {
        #region Create Methods
        /// <summary>
        /// Creates a IxxxCellViewCollection object.
        /// </summary>
        private protected override IFrameCellViewCollection CreateEmbeddingCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, FrameCellViewList list)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameVerticalCollectionPlaceholderFrame));
            return new FrameColumn(stateView, parentCellView, list, this);
        }
        #endregion
    }
}
