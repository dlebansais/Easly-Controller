namespace EaslyController.Frame
{
    /// <summary>
    /// Frame for displaying more frames vertically.
    /// </summary>
    public interface IFrameVerticalPanelFrame : IFramePanelFrame
    {
    }

    /// <summary>
    /// Frame for displaying more frames vertically.
    /// </summary>
    public class FrameVerticalPanelFrame : FramePanelFrame, IFrameVerticalPanelFrame
    {
        #region Create Methods
        /// <summary>
        /// Creates a IxxxMutableCellViewCollection object.
        /// </summary>
        protected override IFrameMutableCellViewCollection CreateEmbeddingCellView(IFrameNodeStateView stateView, IFrameCellViewList list)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameVerticalPanelFrame));
            return new FrameMutableColumn(stateView, list);
        }
        #endregion
    }
}
