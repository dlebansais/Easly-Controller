namespace EaslyController.Frame
{
    /// <summary>
    /// Frame for a block list displayed horizontally.
    /// </summary>
    public interface IFrameHorizontalBlockListFrame : IFrameBlockListFrame
    {
    }

    /// <summary>
    /// Frame for a block list displayed horizontally.
    /// </summary>
    public class FrameHorizontalBlockListFrame : FrameBlockListFrame, IFrameHorizontalBlockListFrame
    {
        #region Create Methods
        /// <summary>
        /// Creates a IxxxCellViewCollection object.
        /// </summary>
        private protected override IFrameCellViewCollection CreateEmbeddingCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, IFrameCellViewList list)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameHorizontalBlockListFrame));
            return new FrameLine(stateView, parentCellView, list);
        }
        #endregion
    }
}
