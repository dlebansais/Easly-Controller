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
        protected override IFrameCellViewCollection CreateEmbeddingCellView(IFrameNodeStateView stateView, IFrameCellViewList list)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameVerticalBlockListFrame));
            return new FrameColumn(stateView, list);
        }
        #endregion
    }
}
