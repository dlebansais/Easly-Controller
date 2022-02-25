namespace EaslyController.Frame
{
    using NotNullReflection;

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
        private protected override IFrameCellViewCollection CreateEmbeddingCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, FrameCellViewList list)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FrameHorizontalCollectionPlaceholderFrame>());
            return new FrameLine(stateView, parentCellView, list, this);
        }
        #endregion
    }
}
