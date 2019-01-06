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
        protected override IFrameCellViewCollection CreateEmbeddingCellView(IFrameNodeStateView stateView, IFrameCellViewList list)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameHorizontalCollectionPlaceholderFrame));
            return new FrameLine(stateView, list);
        }
        #endregion
    }
}
