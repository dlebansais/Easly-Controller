namespace EaslyController.Frame
{
    /// <summary>
    /// Frame for a placeholder node in a block list displayed horizontally.
    /// </summary>
    public interface IFrameHorizontalCollectionPlaceholderFrame : IFrameCollectionPlaceholderFrame
    {
    }

    /// <summary>
    /// Frame for a placeholder node in a block list displayed horizontally.
    /// </summary>
    public class FrameHorizontalCollectionPlaceholderFrame : FrameCollectionPlaceholderFrame, IFrameHorizontalCollectionPlaceholderFrame
    {
        #region Create Methods
        /// <summary>
        /// Creates a IxxxCellViewCollection object.
        /// </summary>
        private protected override IFrameCellViewCollection CreateEmbeddingCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, IFrameCellViewList list)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameHorizontalCollectionPlaceholderFrame));
            return new FrameLine(stateView, parentCellView, list);
        }
        #endregion
    }
}
