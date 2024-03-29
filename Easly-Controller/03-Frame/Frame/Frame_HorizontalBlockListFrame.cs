﻿namespace EaslyController.Frame
{
    using NotNullReflection;

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
        /// Creates a IxxxCellViewCollection object.
        /// </summary>
        private protected override IFrameCellViewCollection CreateEmbeddingCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, FrameCellViewList list)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FrameHorizontalBlockListFrame>());
            return new FrameLine(stateView, parentCellView, list, this);
        }
        #endregion
    }
}
