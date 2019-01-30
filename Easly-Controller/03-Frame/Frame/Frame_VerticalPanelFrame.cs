namespace EaslyController.Frame
{
    using System.Windows.Markup;

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
        private protected override IFrameCellViewCollection CreateEmbeddingCellView(IFrameNodeStateView stateView, IFrameCellViewList list)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameVerticalPanelFrame));
            return new FrameColumn(stateView, list);
        }
        #endregion
    }
}
