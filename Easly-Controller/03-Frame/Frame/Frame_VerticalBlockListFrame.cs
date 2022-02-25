namespace EaslyController.Frame
{
    using NotNullReflection;

    /// <summary>
    /// Frame for a block list displayed vertically.
    /// </summary>
    public interface IFrameVerticalBlockListFrame : IFrameBlockListFrame
    {
    }

    /// <summary>
    /// Frame for a block list displayed vertically.
    /// </summary>
    public class FrameVerticalBlockListFrame : FrameBlockListFrame, IFrameVerticalBlockListFrame
    {
        #region Create Methods
        /// <summary>
        /// Creates a IxxxCellViewCollection object.
        /// </summary>
        private protected override IFrameCellViewCollection CreateEmbeddingCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, FrameCellViewList list)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FrameVerticalBlockListFrame>());
            return new FrameColumn(stateView, parentCellView, list, this);
        }
        #endregion
    }
}
