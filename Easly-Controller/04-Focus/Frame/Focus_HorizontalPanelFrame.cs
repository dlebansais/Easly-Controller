﻿using EaslyController.Frame;
using System.Windows.Markup;

namespace EaslyController.Focus
{
    /// <summary>
    /// Focus for displaying more frames horizontally.
    /// </summary>
    public interface IFocusHorizontalPanelFrame : IFrameHorizontalPanelFrame, IFocusPanelFrame
    {
    }

    /// <summary>
    /// Focus for displaying more frames horizontally.
    /// </summary>
    [ContentProperty("Items")]
    public class FocusHorizontalPanelFrame : FrameHorizontalPanelFrame, IFocusHorizontalPanelFrame
    {
        #region Properties
        /// <summary>
        /// Parent template.
        /// </summary>
        public new IFocusTemplate ParentTemplate { get { return (IFocusTemplate)base.ParentTemplate; } }

        /// <summary>
        /// Parent frame, or null for the root frame in a template.
        /// </summary>
        public new IFocusFrame ParentFrame { get { return (IFocusFrame)base.ParentFrame; } }

        /// <summary>
        /// List of frames within this frame.
        /// </summary>
        public new IFocusFrameList Items { get { return (IFocusFrameList)base.Items; } }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxFrameList object.
        /// </summary>
        protected override IFrameFrameList CreateItems()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusHorizontalPanelFrame));
            return new FocusFrameList();
        }

        /// <summary>
        /// Creates a IxxxCellViewList object.
        /// </summary>
        protected override IFrameCellViewList CreateCellViewList()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusHorizontalPanelFrame));
            return new FocusCellViewList();
        }

        /// <summary>
        /// Creates a IxxxContainerCellView object.
        /// </summary>
        protected override IFrameContainerCellView CreateFrameCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, IFrameNodeStateView childStateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusHorizontalPanelFrame));
            return new FocusContainerCellView((IFocusNodeStateView)stateView, (IFocusCellViewCollection)parentCellView, (IFocusNodeStateView)childStateView);
        }

        /// <summary>
        /// Creates a IxxxCellViewCollection object.
        /// </summary>
        protected override IFrameCellViewCollection CreateEmbeddingCellView(IFrameNodeStateView stateView, IFrameCellViewList list)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusHorizontalPanelFrame));
            return new FocusLine((IFocusNodeStateView)stateView, (IFocusCellViewList)list);
        }
        #endregion
    }
}
