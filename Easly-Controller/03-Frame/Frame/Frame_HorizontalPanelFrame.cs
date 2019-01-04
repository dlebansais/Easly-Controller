namespace EaslyController.Frame
{
    /// <summary>
    /// Frame for displaying more frames horizontally.
    /// </summary>
    public interface IFrameHorizontalPanelFrame : IFramePanelFrame
    {
    }

    /// <summary>
    /// Frame for displaying more frames horizontally.
    /// </summary>
    public class FrameHorizontalPanelFrame : FramePanelFrame, IFrameHorizontalPanelFrame
    {
        #region Create Methods
        /// <summary>
        /// Creates a IxxxMutableCellViewCollection object.
        /// </summary>
        protected override IFrameMutableCellViewCollection CreateEmbeddingCellView(IFrameNodeStateView stateView, IFrameCellViewList list)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameHorizontalPanelFrame));
            return new FrameMutableLine(stateView, list);
        }
        #endregion
    }
}
