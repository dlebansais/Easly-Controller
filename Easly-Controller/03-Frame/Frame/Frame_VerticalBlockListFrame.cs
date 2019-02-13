namespace EaslyController.Frame
{
    /// <summary>
    /// Frame for a block list displayed vertically.
    /// </summary>
    public interface IFrameVerticalBlockListFrame : IFrameBlockListFrame
    {
    }

    /// <summary>
    /// Frame for a block list displayed vertically.
    /// </summary>
    public class FrameVerticalBlockListFrame : FrameBlockListFrame, IFrameVerticalBlockListFrame
    {
        #region Create Methods
        /// <summary>
        /// Creates a IxxxCellViewCollection object.
        /// </summary>
        private protected override IFrameCellViewCollection CreateEmbeddingCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, IFrameCellViewList list)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameVerticalBlockListFrame));
            return new FrameColumn(stateView, parentCellView, list);
        }
        #endregion
    }
}
