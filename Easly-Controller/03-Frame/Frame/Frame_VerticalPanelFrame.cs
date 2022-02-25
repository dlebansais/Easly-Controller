namespace EaslyController.Frame
{
    using System.Windows.Markup;
    using NotNullReflection;

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
        private protected override IFrameCellViewCollection CreateEmbeddingCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, FrameCellViewList list)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FrameVerticalPanelFrame>());
            return new FrameColumn(stateView, parentCellView, list, this);
        }
        #endregion
    }
}
