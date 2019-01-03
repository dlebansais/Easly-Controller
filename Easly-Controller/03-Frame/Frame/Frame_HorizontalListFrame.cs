namespace EaslyController.Frame
{
    /// <summary>
    /// Base frame for a list of nodes displayed horizontally.
    /// </summary>
    public interface IFrameHorizontalListFrame : IFrameListFrame
    {
    }

    /// <summary>
    /// Base frame for a list of nodes displayed horizontally.
    /// </summary>
    public class FrameHorizontalListFrame : FrameListFrame, IFrameHorizontalListFrame
    {
        #region Create Methods
        /// <summary>
        /// Creates a IxxxMutableCellViewCollection object.
        /// </summary>
        protected override IFrameMutableCellViewCollection CreateEmbeddingCellView(IFrameNodeStateView stateView, IFrameCellViewList list)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameHorizontalListFrame));
            return new FrameMutableLine(stateView, list);
        }
        #endregion
    }
}
