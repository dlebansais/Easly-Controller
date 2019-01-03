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
        /// Creates a IxxxMutableCellViewCollection object.
        /// </summary>
        protected override IFrameMutableCellViewCollection CreateEmbeddingCellView(IFrameNodeStateView stateView, IFrameCellViewList list)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameHorizontalCollectionPlaceholderFrame));
            return new FrameMutableLine(stateView, list);
        }
        #endregion
    }
}
