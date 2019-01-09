using System.Windows.Markup;

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
    [ContentProperty("Items")]
    public class FrameVerticalPanelFrame : FramePanelFrame, IFrameVerticalPanelFrame
    {
        #region Create Methods
        /// <summary>
        /// Creates a IxxxCellViewCollection object.
        /// </summary>
        protected override IFrameCellViewCollection CreateEmbeddingCellView(IFrameNodeStateView stateView, IFrameCellViewList list)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameVerticalPanelFrame));
            return new FrameColumn(stateView, list);
        }
        #endregion
    }
}
