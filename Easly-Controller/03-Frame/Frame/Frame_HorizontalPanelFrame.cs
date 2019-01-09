using System.Windows.Markup;

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
    [ContentProperty("Items")]
    public class FrameHorizontalPanelFrame : FramePanelFrame, IFrameHorizontalPanelFrame
    {
        #region Create Methods
        /// <summary>
        /// Creates a IxxxCellViewCollection object.
        /// </summary>
        protected override IFrameCellViewCollection CreateEmbeddingCellView(IFrameNodeStateView stateView, IFrameCellViewList list)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameHorizontalPanelFrame));
            return new FrameLine(stateView, list);
        }
        #endregion
    }
}
