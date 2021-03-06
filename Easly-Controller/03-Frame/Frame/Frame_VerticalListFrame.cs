﻿namespace EaslyController.Frame
{
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
        private protected override IFrameCellViewCollection CreateEmbeddingCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, IFrameCellViewList list)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameVerticalListFrame));
            return new FrameColumn(stateView, parentCellView, list, this);
        }
        #endregion
    }
}
