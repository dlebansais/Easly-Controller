﻿namespace EaslyController.Frame
{
    using System.Windows.Markup;
    using NotNullReflection;

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
        private protected override IFrameCellViewCollection CreateEmbeddingCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, FrameCellViewList list)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FrameHorizontalPanelFrame>());
            return new FrameLine(stateView, parentCellView, list, this);
        }
        #endregion
    }
}
