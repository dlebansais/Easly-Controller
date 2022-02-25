namespace EaslyController.Frame
{
    using NotNullReflection;

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
        private protected override IFrameCellViewCollection CreateEmbeddingCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, FrameCellViewList list)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FrameVerticalListFrame>());
            return new FrameColumn(stateView, parentCellView, list, this);
        }
        #endregion
    }
}
