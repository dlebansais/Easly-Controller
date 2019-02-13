namespace EaslyController.Frame
{
    using System.Windows.Markup;

    /// <summary>
    /// Frame for displaying more frames horizontally.
    /// </summary>
    public interface IFrameHorizontalPanelFrame : IFramePanelFrame
    {
    }

    /// <summary>
    /// Frame for displaying more frames horizontally.
    /// </summary>
    [ContentProperty("Items")]
    public class FrameHorizontalPanelFrame : FramePanelFrame, IFrameHorizontalPanelFrame
    {
        #region Create Methods
        /// <summary>
        /// Creates a IxxxCellViewCollection object.
        /// </summary>
        private protected override IFrameCellViewCollection CreateEmbeddingCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, IFrameCellViewList list)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameHorizontalPanelFrame));
            return new FrameLine(stateView, parentCellView, list, this);
        }
        #endregion
    }
}
