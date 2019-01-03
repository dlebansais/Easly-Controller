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
        /// Creates a IxxxMutableCellViewCollection object.
        /// </summary>
        protected override IFrameMutableCellViewCollection CreateEmbeddingCellView(IFrameNodeStateView stateView, IFrameCellViewList list)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameHorizontalBlockListFrame));
            return new FrameMutableLine(stateView, list);
        }
        #endregion
    }
}
