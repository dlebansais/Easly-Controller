namespace EaslyController.Frame
{
    using NotNullReflection;

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
        /// Creates a IxxxCellViewCollection object.
        /// </summary>
        private protected override IFrameCellViewCollection CreateEmbeddingCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, FrameCellViewList list)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FrameHorizontalListFrame>());
            return new FrameLine(stateView, parentCellView, list, this);
        }
        #endregion
    }
}
