namespace EaslyController.Frame
{
    /// <summary>
    /// Base frame for a list of nodes displayed vertically.
    /// </summary>
    public interface IFrameVerticalListFrame : IFrameListFrame
    {
    }

    /// <summary>
    /// Base frame for a list of nodes displayed vertically.
    /// </summary>
    public class FrameVerticalListFrame : FrameListFrame, IFrameVerticalListFrame
    {
        #region Create Methods
        /// <summary>
        /// Creates a IxxxCellViewCollection object.
        /// </summary>
        private protected override IFrameCellViewCollection CreateEmbeddingCellView(IFrameNodeStateView stateView, IFrameCellViewList list)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameHorizontalListFrame));
            return new FrameColumn(stateView, list);
        }
        #endregion
    }
}
